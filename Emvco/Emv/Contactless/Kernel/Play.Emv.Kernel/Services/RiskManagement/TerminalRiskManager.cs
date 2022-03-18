﻿using System.Threading.Tasks;

using Play.Core.Math;
using Play.Emv.DataElements;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Globalization.Currency;

using PrimaryAccountNumber = Play.Emv.DataElements.PrimaryAccountNumber;

namespace Play.Emv.Kernel.Services;

// TODO: Not sure if we're supposed to cross reference the sequence number and only check that, or if we're supposed to be stopping some kind of rapid attack. Both would make sense separately but checking sequence wouldn't make sense for risk
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
internal class TerminalRiskManager : IManageTerminalRisk
{
    #region Instance Values

    private readonly IPercentageSelectionQueue _PercentageSelectionQueue;
    private readonly ICoordinateSplitPayments _SplitPaymentCoordinator;

    #endregion

    #region Constructor

    public TerminalRiskManager(ICoordinateSplitPayments splitPaymentCoordinator, IPercentageSelectionQueue percentageSelectionQueue)
    {
        _SplitPaymentCoordinator = splitPaymentCoordinator;
        _PercentageSelectionQueue = percentageSelectionQueue;
    }

    #endregion

    #region Instance Members

    private static TerminalRiskManagementResponse CreateFloorLimitExceededResponse()
    {
        TerminalVerificationResult terminalVerificationResult = TerminalVerificationResult.Create();
        terminalVerificationResult.SetTransactionExceedsFloorLimit();

        return new TerminalRiskManagementResponse(terminalVerificationResult,
                                                  TransactionStatusInformationFlags.TerminalRiskManagementPerformed);
    }

    private static TerminalRiskManagementResponse CreateRandomlySelectedForOnlineProcessResponse()
    {
        TerminalVerificationResult terminalVerificationResult = TerminalVerificationResult.Create();
        terminalVerificationResult.SetTransactionSelectedRandomlyForOnlineProcessing();

        return new TerminalRiskManagementResponse(terminalVerificationResult,
                                                  TransactionStatusInformationFlags.TerminalRiskManagementPerformed);
    }

    private static TerminalRiskManagementResponse CreateVelocityCheckDoesNotHaveRequiredItemsResponse()
    {
        TerminalVerificationResult terminalVerificationResult = TerminalVerificationResult.Create();
        terminalVerificationResult.SetUpperConsecutiveOfflineLimitExceeded();
        terminalVerificationResult.SetLowerConsecutiveOfflineLimitExceeded();

        return new TerminalRiskManagementResponse(terminalVerificationResult,
                                                  TransactionStatusInformationFlags.TerminalRiskManagementPerformed);
    }

    private static TerminalRiskManagementResponse CreateVelocityLowerThresholdExceededResponse()
    {
        TerminalVerificationResult terminalVerificationResult = TerminalVerificationResult.Create();
        terminalVerificationResult.SetLowerConsecutiveOfflineLimitExceeded();

        return new TerminalRiskManagementResponse(terminalVerificationResult,
                                                  TransactionStatusInformationFlags.TerminalRiskManagementPerformed);
    }

    private static TerminalRiskManagementResponse CreateVelocityUpperThresholdExceededResponse()
    {
        TerminalVerificationResult terminalVerificationResult = TerminalVerificationResult.Create();
        terminalVerificationResult.SetUpperConsecutiveOfflineLimitExceeded();

        return new TerminalRiskManagementResponse(terminalVerificationResult,
                                                  TransactionStatusInformationFlags.TerminalRiskManagementPerformed);
    }

    private static bool DoesVelocityCheckHaveRequiredItems(
        ushort? applicationTransactionCount,
        ushort? lastOnlineApplicationTransactionCount)
    {
        if (applicationTransactionCount is null)
            return false;
        if (lastOnlineApplicationTransactionCount is null)
            return false;

        return true;
    }

    private static Percentage GetTransactionTargetPercentage(
        Money amountAuthorized,
        Money terminalFloorLimit,
        Money biasedRandomSelectionThreshold,
        Percentage biasedRandomSelectionMaximumTargetPercentage,
        Percentage randomSelectionTargetPercentage)
    {
        ulong interpolationFactor = (ulong) ((amountAuthorized - biasedRandomSelectionThreshold)
            / (terminalFloorLimit - biasedRandomSelectionThreshold));
        int transactionTargetPercent =
            (((byte) biasedRandomSelectionMaximumTargetPercentage - (byte) randomSelectionTargetPercentage) * (byte) interpolationFactor)
            + (byte) randomSelectionTargetPercentage;

        return new Percentage((byte) transactionTargetPercent);
    }

    /// <summary>
    ///     Any transaction with a transaction amount less than the Threshold Value for Biased Random Selection will be subject
    ///     to selection at
    ///     random without further regard for the value of the transaction
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6.2
    /// </remarks>
    private async Task<bool> IsBiasedRandomSelection(
        Money amountAuthorizedNumeric,
        Money biasedRandomSelectionThreshold,
        Money terminalFloorLimit,
        Percentage biasedRandomSelectionMaximumTargetPercentage,
        Percentage randomSelectionTargetPercentage)
    {
        if (amountAuthorizedNumeric < biasedRandomSelectionThreshold)
            return false;

        if (amountAuthorizedNumeric > terminalFloorLimit)
            return false;

        return await _PercentageSelectionQueue.IsRandomSelection(GetTransactionTargetPercentage(amountAuthorizedNumeric, terminalFloorLimit,
                                                                  biasedRandomSelectionThreshold,
                                                                  biasedRandomSelectionMaximumTargetPercentage,
                                                                  randomSelectionTargetPercentage)).ConfigureAwait(false);
    }

    // TODO: Not sure if we're supposed to be looking at sequence number here 
    /// <summary>
    ///     IsFloorLimitExceeded
    /// </summary>
    /// <param name="primaryAccountNumber"></param>
    /// <param name="amountAuthorizedNumeric"></param>
    /// <param name="terminalFloorLimit"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    private bool IsFloorLimitExceeded(
        PrimaryAccountNumber primaryAccountNumber, /*uint sequenceNumber,*/
        Money amountAuthorizedNumeric,
        Money terminalFloorLimit)
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
        byte lowerConsecutiveOfflineLimit,
        ushort applicationTransactionCount,
        ushort lastOnlineApplicationTransactionCount)
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
    private async Task<bool> IsRandomSelection(
        Money amountAuthorizedNumeric,
        Money biasedRandomSelectionThreshold,
        Percentage randomSelectionTargetPercentage)
    {
        if (amountAuthorizedNumeric < biasedRandomSelectionThreshold)
            return await _PercentageSelectionQueue.IsRandomSelection(randomSelectionTargetPercentage);

        return false;
    }

    /// <summary>
    ///     Checks the Issuer supplied values indicating if this transaction should be authorized online
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.6.3
    /// </remarks>
    private static bool IsUpperVelocityThresholdExceeded(
        byte upperConsecutiveOfflineLimit,
        ushort applicationTransactionCount,
        ushort lastOnlineApplicationTransactionCount)
    {
        if (applicationTransactionCount <= lastOnlineApplicationTransactionCount)
            return true;

        if ((applicationTransactionCount - lastOnlineApplicationTransactionCount) > upperConsecutiveOfflineLimit)
            return true;

        return false;
    }

    /// <summary>
    ///     Process
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    public async Task<TerminalRiskManagementResponse> Process(TerminalRiskManagementCommand command)
    {
        if (IsFloorLimitExceeded(command.GetPrimaryAccountNumber(), command.GetAmountAuthorizedNumeric(), command.GetTerminalFloorLimit()))
            return CreateFloorLimitExceededResponse();

        if (await IsRandomSelection(command.GetAmountAuthorizedNumeric(), command.GetTerminalFloorLimit(),
                                    command.GetRandomSelectionTargetPercentage()).ConfigureAwait(false))
            return CreateRandomlySelectedForOnlineProcessResponse();

        if (await IsBiasedRandomSelection(command.GetAmountAuthorizedNumeric(), command.GetBiasedRandomSelectionThreshold(),
                                          command.GetTerminalFloorLimit(), command.GetBiasedRandomSelectionMaximumPercentage(),
                                          command.GetRandomSelectionTargetPercentage()))
            return CreateRandomlySelectedForOnlineProcessResponse();

        if (!command.IsVelocityCheckSupported())
            return new TerminalRiskManagementResponse(TerminalVerificationResult.Create(), TransactionStatusInformationFlags.NotAvailable);

        if (!DoesVelocityCheckHaveRequiredItems(command.GetApplicationTransactionCount(),
                                                command.GetLastOnlineApplicationTransactionCount()))
            return CreateVelocityCheckDoesNotHaveRequiredItemsResponse();

        if (IsLowerVelocityThresholdExceeded(command.GetLowerConsecutiveOfflineLimit()!.Value,
                                             command.GetApplicationTransactionCount()!.Value,
                                             command.GetLastOnlineApplicationTransactionCount()!.Value))
            return CreateVelocityLowerThresholdExceededResponse();

        if (IsUpperVelocityThresholdExceeded(command.GetUpperConsecutiveOfflineLimit()!.Value,
                                             command.GetApplicationTransactionCount()!.Value,
                                             command.GetLastOnlineApplicationTransactionCount()!.Value))
            return CreateVelocityUpperThresholdExceededResponse();

        return new TerminalRiskManagementResponse(TerminalVerificationResult.Create(), TransactionStatusInformationFlags.NotAvailable);
    }

    #endregion
}