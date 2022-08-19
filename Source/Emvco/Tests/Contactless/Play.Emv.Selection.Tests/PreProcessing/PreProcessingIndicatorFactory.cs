using AutoFixture;

using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Selection.Configuration;

namespace Play.Emv.Selection.Tests.PreProcessing;

public class PreProcessingIndicatorFactory
{
    public static TransactionProfile CreateTransactionProfile(
        IFixture fixture, bool isStatusCheckSupported, bool isZeroAmountAllowed, bool isZeroAmountAllowedForOffline, bool isExtendedSelectionSupported)
    {
        MerchantIdentifier merchantIdentifier = fixture.Create<MerchantIdentifier>();
        TerminalIdentification terminalIdentification = fixture.Create<TerminalIdentification>();
        InterfaceDeviceSerialNumber interfaceDeviceSerialNumber = fixture.Create<InterfaceDeviceSerialNumber>();
        CombinationCompositeKey combinationCompositeKey = fixture.Create<CombinationCompositeKey>();
        ApplicationPriorityIndicator applicationPriorityIndicator = fixture.Create<ApplicationPriorityIndicator>();
        ReaderContactlessTransactionLimit readerContactlessTransactionLimit = fixture.Create<ReaderContactlessTransactionLimit>();
        ReaderCvmRequiredLimit readerCvmRequiredLimit = fixture.Create<ReaderCvmRequiredLimit>();
        TerminalFloorLimit terminalFloorLimit = fixture.Create<TerminalFloorLimit>();
        TerminalTransactionQualifiers ttq = fixture.Create<TerminalTransactionQualifiers>();
        TerminalCategoriesSupportedList terminalCategoriesSupportedList = fixture.Create<TerminalCategoriesSupportedList>();
        ReaderContactlessFloorLimit readerContactlessFloorLimit = fixture.Create<ReaderContactlessFloorLimit>();
        KernelConfiguration kernelConfiguration = fixture.Create<KernelConfiguration>();
        TransactionProfile transactionProfile = new(combinationCompositeKey, applicationPriorityIndicator, readerContactlessTransactionLimit,
            readerContactlessFloorLimit, terminalFloorLimit, readerCvmRequiredLimit, ttq, kernelConfiguration, isStatusCheckSupported, isZeroAmountAllowed,
            isZeroAmountAllowedForOffline, isExtendedSelectionSupported);

        return transactionProfile;
    }
}