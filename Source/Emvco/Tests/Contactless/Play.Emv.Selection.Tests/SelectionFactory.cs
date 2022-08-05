using AutoFixture;

using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Selection.Tests;

public class SelectionFactory
{
    public static TransactionProfile CreateTransactionProfile(IFixture fixture, bool isStatusCheckSupported, bool isZeroAmountAllowed,
    bool isZeroAmountAllowedForOffline, bool isExtendedSelectionSupported)
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

        TransactionProfile transactionProfile = new(merchantIdentifier, terminalIdentification, interfaceDeviceSerialNumber,
            combinationCompositeKey, applicationPriorityIndicator, readerContactlessTransactionLimit, readerCvmRequiredLimit, terminalFloorLimit,
            ttq, terminalCategoriesSupportedList, isStatusCheckSupported, isZeroAmountAllowed, isZeroAmountAllowedForOffline, isExtendedSelectionSupported);

        return transactionProfile;
    }
}
