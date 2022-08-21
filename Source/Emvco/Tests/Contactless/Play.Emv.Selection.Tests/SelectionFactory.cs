using AutoFixture;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Selection.Tests;

public class SelectionFactory
{
    #region Instance Members

    public static TransactionProfile CreateTransactionProfile(
        IFixture fixture, bool isStatusCheckSupported, bool isZeroAmountAllowed, bool isZeroAmountAllowedForOffline, bool isExtendedSelectionSupported)
    {
        CombinationCompositeKey combinationCompositeKey = fixture.Create<CombinationCompositeKey>();
        ApplicationPriorityIndicator applicationPriorityIndicator = fixture.Create<ApplicationPriorityIndicator>();
        ReaderContactlessTransactionLimit readerContactlessTransactionLimit = fixture.Create<ReaderContactlessTransactionLimit>();
        ReaderContactlessFloorLimit readerContactlessFloorLimit = fixture.Create<ReaderContactlessFloorLimit>();
        ReaderCvmRequiredLimit readerCvmRequiredLimit = fixture.Create<ReaderCvmRequiredLimit>();
        TerminalFloorLimit terminalFloorLimit = fixture.Create<TerminalFloorLimit>();
        TerminalTransactionQualifiers ttq = fixture.Create<TerminalTransactionQualifiers>();
        KernelConfiguration kernelConfiguration = fixture.Create<KernelConfiguration>();

        TransactionProfile transactionProfile = new(combinationCompositeKey, applicationPriorityIndicator, readerContactlessTransactionLimit,
            readerContactlessFloorLimit, terminalFloorLimit, readerCvmRequiredLimit, ttq, kernelConfiguration, isStatusCheckSupported, isZeroAmountAllowed,
            isZeroAmountAllowedForOffline, isExtendedSelectionSupported);

        return transactionProfile;
    }

    #endregion
}