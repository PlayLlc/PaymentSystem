using System;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Security.Cryptograms;

namespace Play.Emv.Terminal.Services.DataExchange;

// WARNING: THIS IS A GIANT HACK. We should be queueing up configuration items in the Terminal and dequeue-ing them with a tag when we want to retrieve it. This pattern is pretty error prone
internal class TerminalQueryResolver
{
    #region Instance Members

    public TagLengthValue Resolve(in TerminalSession terminalSession, Tag tag)
    {
        if (tag == PunatcTrack2.Tag)
            return GetPunatcTrack2(terminalSession);

        if (tag == AmountAuthorizedNumeric.Tag)
            return GetAmountAuthorizedNumeric(terminalSession);

        if (tag == AmountOtherNumeric.Tag)
            return GetAmountOtherNumeric(terminalSession);

        if (tag == TerminalCountryCode.Tag)
            return GetTerminalCountryCode(terminalSession);

        if (tag == TerminalVerificationResults.Tag)
            return GetTerminalVerificationResults(terminalSession);

        if (tag == TransactionCurrencyCode.Tag)
            return GetTransactionCurrencyCode(terminalSession);

        if (tag == TransactionDate.Tag)
            return GetTransactionDate(terminalSession);

        if (tag == TransactionType.Tag)
            return GetTransactionType(terminalSession);

        if (tag == UnpredictableNumber.Tag)
            return GetUnpredictableNumber(terminalSession);

        if (tag == MerchantNameAndLocation.Tag)
            return GetMerchantNameAndLocation(terminalSession);

        if (tag == PoiInformation.Tag)
            return GetPoiInformation(terminalSession);

        if (tag == TerminalActionCodeDenial.Tag)
            return GetTerminalActionCodeDenial(terminalSession);

        if (tag == TerminalActionCodeDefault.Tag)
            return GetTerminalActionCodeDefault(terminalSession);

        if (tag == TerminalActionCodeOnline.Tag)
            return GetTerminalActionCodeOnline(terminalSession);

        if (tag == TerminalFloorLimit.Tag)
            return GetTerminalFloorLimit(terminalSession);

        if (tag == TransactionCurrencyExponent.Tag)
            return GetTransactionCurrencyExponent(terminalSession);
        if (tag == TransactionReferenceCurrencyCode.Tag)
            return GetTransactionReferenceCurrencyCode(terminalSession);
        if (tag == TransactionReferenceCurrencyExponent.Tag)
            return GetTransactionReferenceCurrencyExponent(terminalSession);

        if (tag == TerminalType.Tag)
            return GetTerminalType(terminalSession);
        if (tag == TerminalCapabilities.Tag)
            return GetTerminalCapabilities(terminalSession);
        if (tag == AcquirerIdentifier.Tag)
            return GetAcquirerIdentifier(terminalSession);
        if (tag == CvmResults.Tag)
            return GetCvmResults(terminalSession);
        if (tag == CryptogramInformationData.Tag)
            return GetCryptogramInformationData(terminalSession);
        if (tag == InterfaceDeviceSerialNumber.Tag)
            return GetInterfaceDeviceSerialNumber(terminalSession);
        if (tag == MerchantCategoryCode.Tag)
            return GetMerchantCategoryCode(terminalSession);
        if (tag == TerminalIdentification.Tag)
            return GetTerminalIdentification(terminalSession);
        if (tag == TerminalRiskManagementData.Tag)
            return GetTerminalRiskManagementData(terminalSession);

        return new TagLengthValue(tag, ReadOnlySpan<byte>.Empty);
    }

    private TagLengthValue GetPunatcTrack2(in TerminalSession terminalSession) =>

        // BUG: Resolve this before implementing Magstripe
        new(PunatcTrack2.Tag, ReadOnlySpan<byte>.Empty);

    private TagLengthValue GetAmountAuthorizedNumeric(in TerminalSession terminalSession) =>
        terminalSession.Transaction.GetAmountAuthorizedNumeric().AsTagLengthValue();

    private TagLengthValue GetAmountOtherNumeric(in TerminalSession terminalSession) =>
        terminalSession.Transaction.GetAmountOtherNumeric().AsTagLengthValue();

    private TagLengthValue GetTerminalCountryCode(in TerminalSession terminalSession) =>
        terminalSession.Transaction.GetTerminalCountryCode().AsTagLengthValue();

    private TagLengthValue GetTerminalVerificationResults(in TerminalSession terminalSession) =>
        terminalSession.Transaction.GetTerminalVerificationResults().AsTagLengthValue();

    private TagLengthValue GetTransactionCurrencyCode(in TerminalSession terminalSession) =>
        terminalSession.Transaction.GetTransactionCurrencyCode().AsTagLengthValue();

    private TagLengthValue GetTransactionDate(in TerminalSession terminalSession) =>
        terminalSession.Transaction.GetTransactionDate().AsTagLengthValue();

    private TagLengthValue GetTransactionType(in TerminalSession terminalSession) =>
        terminalSession.Transaction.GetTransactionType().AsTagLengthValue();

    private TagLengthValue GetUnpredictableNumber(in TerminalSession terminalSession) => new UnpredictableNumber().AsTagLengthValue();

    private TagLengthValue GetMerchantNameAndLocation(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetMerchantNameAndLocation();

    private TagLengthValue GetPoiInformation(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetPoiInformation();

    // /////////////////////////////////////////////////////////////////////////////

    // BUG: Resolve this through a an interface provided by the Issuer module
    private TagLengthValue GetTerminalActionCodeDenial(in TerminalSession terminalSession) => throw new NotImplementedException();

    // BUG: Resolve this through a an interface provided by the Issuer module
    private TagLengthValue GetTerminalActionCodeDefault(in TerminalSession terminalSession) => throw new NotImplementedException();
    private TagLengthValue GetTerminalActionCodeOnline(in TerminalSession terminalSession) => throw new NotImplementedException();

    private TagLengthValue GetTerminalFloorLimit(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetTerminalFloorLimit();

    private TagLengthValue GetTransactionCurrencyExponent(in TerminalSession terminalSession) =>
        terminalSession.Transaction.GetTransactionCurrencyExponent();

    private TagLengthValue GetTransactionReferenceCurrencyCode(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetTransactionReferenceCurrencyCode();

    private TagLengthValue GetTransactionReferenceCurrencyExponent(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetTransactionReferenceCurrencyExponent();

    private TagLengthValue GetTerminalType(in TerminalSession terminalSession) => terminalSession.TerminalConfiguration.GetTerminalType();

    private TagLengthValue GetTerminalCapabilities(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetTerminalCapabilities();

    private TagLengthValue GetAcquirerIdentifier(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetAcquirerIdentifier();

    // BUG: Resolve common terminal services
    private TagLengthValue GetCvmResults(in TerminalSession terminalSession) => throw new NotImplementedException();
    private TagLengthValue GetCryptogramInformationData(in TerminalSession terminalSession) => throw new NotImplementedException();

    private TagLengthValue GetInterfaceDeviceSerialNumber(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetInterfaceDeviceSerialNumber();

    private TagLengthValue GetMerchantCategoryCode(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetMerchantCategoryCode();

    private TagLengthValue GetMerchantIdentifier(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetMerchantIdentifier();

    private TagLengthValue GetTerminalIdentification(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetTerminalIdentification();

    private TagLengthValue GetTerminalRiskManagementData(in TerminalSession terminalSession) =>
        terminalSession.TerminalConfiguration.GetTerminalRiskManagementData();

    #endregion
}