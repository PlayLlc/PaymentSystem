using System;
using System.Threading.Tasks;

using Play.Core;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Terminal.RiskManagement;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Configuration;
using Play.Emv.Kernel.Databases;
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
public class TerminalRiskManager : IManageTerminalRisk
{
    #region Instance Values

    private readonly IProbabilitySelectionQueue _ProbabilitySelectionQueue;
    private readonly IStoreApprovedTransactions _SplitPaymentCoordinator;

    #endregion

    #region Constructor

    public TerminalRiskManager(IStoreApprovedTransactions splitPaymentCoordinator, IProbabilitySelectionQueue probabilitySelectionQueue)
    {
        _SplitPaymentCoordinator = splitPaymentCoordinator;
        _ProbabilitySelectionQueue = probabilitySelectionQueue;
    }

    #endregion

    #region Instance Members

    private static bool IsLastAtcZero(ushort lastOnlineApplicationTransactionCount) => lastOnlineApplicationTransactionCount == 0;

    // HACK: There's probably no real reason that you're using async here
    /// <exception cref="InvalidOperationException"></exception>
    public void Process(KernelDatabase database, TerminalRiskManagementConfiguration configuration)
    {
        ApplicationPan pan = database.Get<ApplicationPan>(ApplicationPan.Tag);
        ApplicationCurrencyCode currencyCode = database.Get<ApplicationCurrencyCode>(ApplicationCurrencyCode.Tag);
        Money amountAuthorizedNumeric = database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag).AsMoney(currencyCode);
        Money terminalFloorLimit = database.Get<TerminalFloorLimit>(TerminalFloorLimit.Tag).AsMoney(currencyCode);
        LowerConsecutiveOfflineLimit lowerConsecutiveOfflineLimit = database.Get<LowerConsecutiveOfflineLimit>(LowerConsecutiveOfflineLimit.Tag);
        UpperConsecutiveOfflineLimit upperConsecutiveOfflineLimit = database.Get<UpperConsecutiveOfflineLimit>(UpperConsecutiveOfflineLimit.Tag);

        database.TryGet(ApplicationTransactionCounter.Tag, out ApplicationTransactionCounter? applicationTransactionCount);
        database.TryGet(LastOnlineApplicationTransactionCounterRegister.Tag,
            out LastOnlineApplicationTransactionCounterRegister? lastOnlineApplicationTransactionCount);

        if (IsFloorLimitExceeded(pan, amountAuthorizedNumeric, terminalFloorLimit))
        {
            database.Update(TerminalVerificationResultCodes.TransactionExceedsFloorLimit);

            return;
        }

        if (IsRandomSelection(amountAuthorizedNumeric, configuration.BiasedRandomSelectionThreshold, configuration.BiasedRandomSelectionTargetPercentage))
        {
            database.Update(TerminalVerificationResultCodes.TransactionSelectedRandomlyForOnlineProcessing);

            return;
        }

        if (IsBiasedRandomSelection(amountAuthorizedNumeric, configuration.BiasedRandomSelectionThreshold, terminalFloorLimit,
            configuration._BiasedRandomSelectionMaximumPercentage, configuration.BiasedRandomSelectionTargetPercentage))
        {
            database.Update(TerminalVerificationResultCodes.TransactionSelectedRandomlyForOnlineProcessing);

            return;
        }

        if (!IsVelocityCheckSupported(upperConsecutiveOfflineLimit, lowerConsecutiveOfflineLimit))
            return;

        if (!DoesVelocityCheckHaveRequiredItems(applicationTransactionCount, lastOnlineApplicationTransactionCount))
        {
            database.Update(TerminalVerificationResultCodes.UpperConsecutiveOfflineLimitExceeded);
            database.Update(TerminalVerificationResultCodes.LowerConsecutiveOfflineLimitExceeded);

            return;
        }

        if (IsLowerVelocityThresholdExceeded(lowerConsecutiveOfflineLimit, applicationTransactionCount!, lastOnlineApplicationTransactionCount!))
            database.Update(TerminalVerificationResultCodes.LowerConsecutiveOfflineLimitExceeded);

        if (IsUpperVelocityThresholdExceeded(upperConsecutiveOfflineLimit, applicationTransactionCount!, lastOnlineApplicationTransactionCount!))
            database.Update(TerminalVerificationResultCodes.UpperConsecutiveOfflineLimitExceeded);

        if (IsLastAtcZero(applicationTransactionCount!))
            database.Update(TerminalVerificationResultCodes.NewCard);
    }

    #region Book 3 Section 10.6.1

    /// <summary>
    ///     During terminal risk management floor limit checking, the terminal checks the transaction log (if available) to
    ///     determine if there is a log entry with the same Application PAN, and, optionally, the same Application PAN Sequence
    ///     Number. If there are several log entries with the same PAN, the terminal selects the most recent entry. The
    ///     terminal adds the Amount, Authorized for the current transaction to the amount stored in the log for that PAN to
    ///     determine if the sum exceeds the Terminal Floor Limit. If the sum is greater than or equal to the Terminal Floor
    ///     Limit, the terminal shall set the ‘Transaction exceeds floor limit’ bit in the TVR to 1.
    /// </summary>
    /// <param name="primaryAccountNumber"></param>
    /// <param name="amountAuthorizedNumeric"></param>
    /// <param name="terminalFloorLimit"></param>
    /// <remarks>
    ///     Book 3 Section 10.6.1
    /// </remarks>
    /// <exception cref="InvalidOperationException"></exception>
    private bool IsFloorLimitExceeded(ApplicationPan primaryAccountNumber, Money amountAuthorizedNumeric, Money terminalFloorLimit)
    {
        if (!_SplitPaymentCoordinator.TryGetSplitPaymentLogItem(primaryAccountNumber, out SplitPaymentLogItem result))
            return amountAuthorizedNumeric > terminalFloorLimit;

        return amountAuthorizedNumeric.Add(result.GetSubtotal()) > terminalFloorLimit;
    }

    #endregion

    #endregion

    #region Book 3 Section 10.6.2

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

        if (!_ProbabilitySelectionQueue.IsRandomSelection(GetTransactionTargetPercentage(amountAuthorizedNumeric, terminalFloorLimit,
            biasedRandomSelectionThreshold, biasedRandomSelectionMaximumTargetProbability, randomSelectionTargetProbability)))
            return false;

        return true;
    }

    /// <remarks>
    ///     Book 3 Section 10.6.2
    /// </remarks>
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
    ///     Any transaction with a transaction amount less than the Threshold Value for Biased Random Selection
    ///     will be subject to selection at random .‘Target Percentage to be Used for Random Selection’ (in the range of 0 to
    ///     99)
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6.2
    /// </remarks>
    private bool IsRandomSelection(Money amountAuthorizedNumeric, Money biasedRandomSelectionThreshold, Probability randomSelectionTargetProbability)
    {
        if (amountAuthorizedNumeric >= biasedRandomSelectionThreshold)
            return false;

        return _ProbabilitySelectionQueue.IsRandomSelection(randomSelectionTargetProbability);
    }

    #endregion

    #region Book 3 Section 10.6.3

    /// <summary>
    ///     Checks the Issuer supplied values indicating if this transaction should be authorized online
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6.3
    /// </remarks>
    private static bool IsLowerVelocityThresholdExceeded(
        byte lowerConsecutiveOfflineLimit, ushort applicationTransactionCount, ushort lastOnlineApplicationTransactionCount)
    {
        if ((applicationTransactionCount - lastOnlineApplicationTransactionCount) > lowerConsecutiveOfflineLimit)
            return true;

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
        if ((applicationTransactionCount - lastOnlineApplicationTransactionCount) > upperConsecutiveOfflineLimit)
            return true;

        return false;
    }

    /// <remarks>
    ///     Book 3 Section 10.6.3
    /// </remarks>
    private bool IsVelocityCheckSupported(
        UpperConsecutiveOfflineLimit? upperConsecutiveOfflineLimit, LowerConsecutiveOfflineLimit? lowerConsecutiveOfflineLimit)
    {
        if ((upperConsecutiveOfflineLimit == null) || (lowerConsecutiveOfflineLimit == null))
            return false;

        return true;
    }

    /// <remarks>
    ///     Book 3 Section 10.6.3
    /// </remarks>
    private static bool DoesVelocityCheckHaveRequiredItems(
        ApplicationTransactionCounter? applicationTransactionCount, LastOnlineApplicationTransactionCounterRegister? lastOnlineApplicationTransactionCount)
    {
        if (applicationTransactionCount == null)
            return false;

        if (lastOnlineApplicationTransactionCount == null)
            return false;

        if ((ushort) applicationTransactionCount! <= (ushort) lastOnlineApplicationTransactionCount!)
            return false;

        return true;
    }

    #endregion
}