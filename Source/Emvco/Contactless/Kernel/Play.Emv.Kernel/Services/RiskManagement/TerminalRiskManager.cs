using System;
using System.Threading.Tasks;

using Play.Core;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Services._TempLogShit;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services;

// CHECK: Not sure if we're supposed to cross reference the sequence number and only check that, or if we're supposed to be stopping some kind of rapid attack. Both would make sense separately but checking sequence wouldn't make sense for risk
/// <summary>
///     Terminal risk management is that portion of risk management performed by the terminal to protect the acquirer,
///     issuer, and system from fraud.
///     It provides positive issuer authorization for high-value transactions and ensures that transactions initiated from
///     ICCs go online periodically
///     to protect against threats that might be undetectable in an offline environment.The result of terminal risk
///     management is the setting of
///     appropriate bits in the TVR.
///     Terminal risk management consists of:
///     • Floor limit checking
///     • Random transaction selection
///     • Velocity checking
/// </summary>

// DEPRECATING: We're refactoring Terminal Risk Management a bit. No commands sent. We'll add kernel DB interfaces as method injection
internal class TerminalRiskManager : IManageTerminalRisk
{
    #region Instance Values

    private readonly IProbabilitySelectionQueue _ProbabilitySelectionQueue;
    private readonly ICoordinateSplitPayments _SplitPaymentCoordinator;

    /// <summary>
    ///     This is a threshold amount, simply referred to as the threshold value, which can be zero or a positive number
    ///     smaller than the Terminal Floor Limit
    /// </summary>
    private readonly Money _BiasedRandomSelectionThreshold;

    private readonly Money _TerminalFloorLimit;
    private readonly Probability _BiasedRandomSelectionMaximumProbability;
    private readonly Probability _RandomSelectionTargetProbability;
    private readonly byte _LowerConsecutiveOfflineLimit;
    private readonly byte _UpperConsecutiveOfflineLimit;

    #endregion

    #region Constructor

    public TerminalRiskManager(
        ICoordinateSplitPayments splitPaymentCoordinator, IProbabilitySelectionQueue probabilitySelectionQueue,
        Probability biasedRandomSelectionMaximumProbability, Probability randomSelectionTargetProbability, Money terminalFloorLimit,
        Money biasedRandomSelectionThreshold, byte lowerConsecutiveOfflineLimit, byte upperConsecutiveOfflineLimit)
    {
        _SplitPaymentCoordinator = splitPaymentCoordinator;
        _ProbabilitySelectionQueue = probabilitySelectionQueue;
        _BiasedRandomSelectionMaximumProbability = biasedRandomSelectionMaximumProbability;
        _RandomSelectionTargetProbability = randomSelectionTargetProbability;
        _TerminalFloorLimit = terminalFloorLimit;
        _LowerConsecutiveOfflineLimit = lowerConsecutiveOfflineLimit;
        _UpperConsecutiveOfflineLimit = upperConsecutiveOfflineLimit;
        _BiasedRandomSelectionThreshold = biasedRandomSelectionThreshold;
    }

    #endregion

    #region Instance Members

    public bool IsVelocityCheckSupported() => (_UpperConsecutiveOfflineLimit != null) && (_LowerConsecutiveOfflineLimit != null);

    private static void CreateFloorLimitExceededResponse(ITlvReaderAndWriter database)
    {
        TerminalVerificationResults tvr = database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
        TransactionStatusInformation tsi = database.Get<TransactionStatusInformation>(TransactionStatusInformation.Tag);

        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();
        TerminalVerificationResult tvrBit = TerminalVerificationResult.Create();
        tvrBit.SetTransactionExceedsFloorLimit();

        builder.Reset(tvr);
        builder.Set(tvrBit);

        database.Update(tsi.Set(TransactionStatusInformationFlags.TerminalRiskManagementPerformed));
        database.Update(builder.Complete());
    }

    private static void CreateRandomlySelectedForOnlineProcessResponse(ITlvReaderAndWriter database)
    {
        TerminalVerificationResults tvr = database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
        TransactionStatusInformation tsi = database.Get<TransactionStatusInformation>(TransactionStatusInformation.Tag);

        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();
        TerminalVerificationResult tvrBit = TerminalVerificationResult.Create();
        tvrBit.SetTransactionSelectedRandomlyForOnlineProcessing();
        builder.Reset(tvr);
        builder.Set(tvrBit);
        database.Update(tsi.Set(TransactionStatusInformationFlags.TerminalRiskManagementPerformed));
        database.Update(builder.Complete());
    }

    private static void CreateVelocityCheckDoesNotHaveRequiredItemsResponse(ITlvReaderAndWriter database)
    {
        TerminalVerificationResults tvr = database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
        TransactionStatusInformation tsi = database.Get<TransactionStatusInformation>(TransactionStatusInformation.Tag);

        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();
        TerminalVerificationResult tvrBit = TerminalVerificationResult.Create();
        tvrBit.SetUpperConsecutiveOfflineLimitExceeded();
        tvrBit.SetLowerConsecutiveOfflineLimitExceeded();
        builder.Reset(tvr);
        builder.Set(tvrBit);
        database.Update(tsi.Set(TransactionStatusInformationFlags.TerminalRiskManagementPerformed));
        database.Update(builder.Complete());
    }

    private static void CreateVelocityLowerThresholdExceededResponse(ITlvReaderAndWriter database)
    {
        TerminalVerificationResults tvr = database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
        TransactionStatusInformation tsi = database.Get<TransactionStatusInformation>(TransactionStatusInformation.Tag);

        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();
        TerminalVerificationResult tvrBit = TerminalVerificationResult.Create();
        tvrBit.SetLowerConsecutiveOfflineLimitExceeded();
        builder.Reset(tvr);
        builder.Set(tvrBit);
        database.Update(tsi.Set(TransactionStatusInformationFlags.TerminalRiskManagementPerformed));
        database.Update(builder.Complete());
    }

    private static void CreateVelocityUpperThresholdExceededResponse(ITlvReaderAndWriter database)
    {
        TerminalVerificationResults tvr = database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
        TransactionStatusInformation tsi = database.Get<TransactionStatusInformation>(TransactionStatusInformation.Tag);

        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();
        TerminalVerificationResult tvrBit = TerminalVerificationResult.Create();
        tvrBit.SetUpperConsecutiveOfflineLimitExceeded();
        builder.Reset(tvr);
        builder.Set(tvrBit);
        database.Update(tsi.Set(TransactionStatusInformationFlags.TerminalRiskManagementPerformed));
        database.Update(builder.Complete());
    }

    private static bool DoesVelocityCheckHaveRequiredItems(ushort? applicationTransactionCount, ushort? lastOnlineApplicationTransactionCount)
    {
        if (applicationTransactionCount is null)
            return false;
        if (lastOnlineApplicationTransactionCount is null)
            return false;

        return true;
    }

    private static Probability GetTransactionTargetPercentage(
        Money amountAuthorized, Money terminalFloorLimit, Money biasedRandomSelectionThreshold, Probability biasedRandomSelectionMaximumTargetProbability,
        Probability randomSelectionTargetProbability)
    {
        ulong interpolationFactor = (ulong) ((amountAuthorized - biasedRandomSelectionThreshold) / (terminalFloorLimit - biasedRandomSelectionThreshold));

        int transactionTargetPercent =
            (((byte) biasedRandomSelectionMaximumTargetProbability - (byte) randomSelectionTargetProbability) * (byte) interpolationFactor)
            + (byte) randomSelectionTargetProbability;

        return new Probability((byte) transactionTargetPercent);
    }

    /// <summary>
    ///     Any transaction with a transaction amount less than the Threshold Value for Biased Random Selection will be subject
    ///     to selection at
    ///     random without further regard for the value of the transaction
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6.2
    /// </remarks>
    private bool IsBiasedRandomSelection(
        Money amountAuthorizedNumeric, Money biasedRandomSelectionThreshold, Money terminalFloorLimit,
        Probability biasedRandomSelectionMaximumTargetProbability, Probability randomSelectionTargetProbability)
    {
        if (amountAuthorizedNumeric < biasedRandomSelectionThreshold)
            return false;

        if (amountAuthorizedNumeric > terminalFloorLimit)
            return false;

        return _ProbabilitySelectionQueue.IsRandomSelection(GetTransactionTargetPercentage(amountAuthorizedNumeric, terminalFloorLimit,
            biasedRandomSelectionThreshold, biasedRandomSelectionMaximumTargetProbability, randomSelectionTargetProbability));
    }

    // TODO: Not sure if we're supposed to be looking at sequence number here 
    /// <summary>
    ///     IsFloorLimitExceeded
    /// </summary>
    /// <param name="primaryAccountNumber"></param>
    /// <param name="amountAuthorizedNumeric"></param>
    /// <param name="terminalFloorLimit"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsFloorLimitExceeded(
        ApplicationPan primaryAccountNumber, /*uint sequenceNumber,*/
        Money amountAuthorizedNumeric, Money terminalFloorLimit)
    {
        if (!_SplitPaymentCoordinator.TryGetSplitPaymentLogItem(primaryAccountNumber, out SplitPaymentLogItem result))
            return terminalFloorLimit > amountAuthorizedNumeric;

        return terminalFloorLimit > amountAuthorizedNumeric.Add(result.GetSubtotal());
    }

    /// <summary>
    ///     Checks the Issuer supplied values indicating if this transaction should be authorized online
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6.3
    /// </remarks>
    private static bool IsLowerVelocityThresholdExceeded(
        byte lowerConsecutiveOfflineLimit, ushort applicationTransactionCount, ushort lastOnlineApplicationTransactionCount)
    {
        if (applicationTransactionCount <= lastOnlineApplicationTransactionCount)
            return true;

        if ((applicationTransactionCount - lastOnlineApplicationTransactionCount) > lowerConsecutiveOfflineLimit)
            return true;

        return false;
    }

    /// <summary>
    ///     ‘Target Percentage to be Used for Random Selection’ (in the range of 0 to 99)
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6.2
    /// </remarks>
    private bool IsRandomSelection(Money amountAuthorizedNumeric, Money biasedRandomSelectionThreshold, Probability randomSelectionTargetProbability)
    {
        if (amountAuthorizedNumeric < biasedRandomSelectionThreshold)
            return _ProbabilitySelectionQueue.IsRandomSelection(randomSelectionTargetProbability);

        return false;
    }

    /// <summary>
    ///     Checks the Issuer supplied values indicating if this transaction should be authorized online
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6.3
    /// </remarks>
    private static bool IsUpperVelocityThresholdExceeded(
        byte upperConsecutiveOfflineLimit, ushort applicationTransactionCount, ushort lastOnlineApplicationTransactionCount)
    {
        if (applicationTransactionCount <= lastOnlineApplicationTransactionCount)
            return true;

        if ((applicationTransactionCount - lastOnlineApplicationTransactionCount) > upperConsecutiveOfflineLimit)
            return true;

        return false;
    }

    /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void Process(ITlvReaderAndWriter database, ushort applicationTransactionCount, ushort lastOnlineApplicationTransactionCount)
    {
        ApplicationPan applicationPan = database.Get<ApplicationPan>(ApplicationPan.Tag);
        TransactionCurrencyCode transactionCurrencyCode = database.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);
        Money amountAuthorizedNumeric = database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag).AsMoney(transactionCurrencyCode);
        TerminalVerificationResults tvr = database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
        TransactionStatusInformation tsi = database.Get<TransactionStatusInformation>(TransactionStatusInformation.Tag);
        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();
        builder.Reset(tvr);

        if (IsFloorLimitExceeded(applicationPan, amountAuthorizedNumeric, _TerminalFloorLimit))
        {
            CreateFloorLimitExceededResponse(database);

            return;
        }

        if (IsRandomSelection(amountAuthorizedNumeric, _TerminalFloorLimit, _RandomSelectionTargetProbability))
        {
            CreateRandomlySelectedForOnlineProcessResponse(database);

            return;
        }

        if (IsBiasedRandomSelection(amountAuthorizedNumeric, _BiasedRandomSelectionThreshold, _TerminalFloorLimit, _BiasedRandomSelectionMaximumProbability,
            _RandomSelectionTargetProbability))
        {
            CreateRandomlySelectedForOnlineProcessResponse(database);

            return;
        }

        if (!IsVelocityCheckSupported())
        {
            database.Update(tsi.Set(TransactionStatusInformationFlags.NotAvailable));

            return;
        }

        if (!DoesVelocityCheckHaveRequiredItems(applicationTransactionCount, lastOnlineApplicationTransactionCount))
        {
            CreateVelocityCheckDoesNotHaveRequiredItemsResponse(database);

            return;
        }

        if (IsLowerVelocityThresholdExceeded(_LowerConsecutiveOfflineLimit, applicationTransactionCount, lastOnlineApplicationTransactionCount))
        {
            CreateVelocityLowerThresholdExceededResponse(database);

            return;
        }

        if (IsUpperVelocityThresholdExceeded(_UpperConsecutiveOfflineLimit, applicationTransactionCount, lastOnlineApplicationTransactionCount))
        {
            CreateVelocityUpperThresholdExceededResponse(database);

            return;
        }

        database.Update(tsi.Set(TransactionStatusInformationFlags.NotAvailable));
    }

    #endregion
}