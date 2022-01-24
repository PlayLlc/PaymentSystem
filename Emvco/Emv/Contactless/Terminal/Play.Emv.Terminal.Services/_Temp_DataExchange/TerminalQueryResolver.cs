using System;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Security.Cryptograms;
using Play.Emv.Terminal.Configuration.Transaction;

namespace Play.Emv.Terminal.Services;

// WARNING: THIS IS A GIANT HACK. We should be queueing up configuration items in the Terminal and dequeue-ing them with a tag when we want to retrieve it. This pattern is pretty error prone
internal class TerminalQueryResolver
{
    #region Instance Members

    public TagLengthValue Resolve(TerminalStateMachine.TerminalSessionLock sessionLock, Tag tag)
    {
        if (tag == PunatcTrack2.Tag)
            return GetPunatcTrack2(sessionLock.Session!);

        if (tag == AmountAuthorizedNumeric.Tag)
            return GetAmountAuthorizedNumeric(sessionLock.Session!);

        if (tag == AmountOtherNumeric.Tag)
            return GetAmountOtherNumeric(sessionLock.Session!);

        if (tag == TerminalCountryCode.Tag)
            return GetTerminalCountryCode(sessionLock.Session!);

        if (tag == TerminalVerificationResults.Tag)
            return GetTerminalVerificationResults(sessionLock.Session!);

        if (tag == TransactionCurrencyCode.Tag)
            return GetTransactionCurrencyCode(sessionLock.Session!);

        if (tag == TransactionDate.Tag)
            return GetTransactionDate(sessionLock.Session!);

        if (tag == TransactionType.Tag)
            return GetTransactionType(sessionLock.Session!);

        if (tag == UnpredictableNumber.Tag)
            return GetUnpredictableNumber(sessionLock.Session!);

        if (tag == MerchantNameAndLocation.Tag)
            return GetMerchantNameAndLocation(sessionLock.Session!);

        if (tag == PoiInformation.Tag)
            return GetPoiInformation(sessionLock.Session!);

        if (tag == TerminalActionCodeDenial.Tag)
            return GetTerminalActionCodeDenial(sessionLock.Session!);

        if (tag == TerminalActionCodeDefault.Tag)
            return GetTerminalActionCodeDefault(sessionLock.Session!);

        if (tag == TerminalActionCodeOnline.Tag)
            return GetTerminalActionCodeOnline(sessionLock.Session!);

        if (tag == TerminalFloorLimit.Tag)
            return GetTerminalFloorLimit(sessionLock.Session!);

        if (tag == TransactionCurrencyExponent.Tag)
            return GetTransactionCurrencyExponent(sessionLock.Session!);
        if (tag == TransactionReferenceCurrencyCode.Tag)
            return GetTransactionReferenceCurrencyCode(sessionLock.Session!);
        if (tag == TransactionReferenceCurrencyExponent.Tag)
            return GetTransactionReferenceCurrencyExponent(sessionLock.Session!);

        if (tag == TerminalType.Tag)
            return GetTerminalType(sessionLock.Session!);
        if (tag == TerminalCapabilities.Tag)
            return GetTerminalCapabilities(sessionLock.Session!);
        if (tag == AcquirerIdentifier.Tag)
            return GetAcquirerIdentifier(sessionLock.Session!);
        if (tag == CvmResults.Tag)
            return GetCvmResults(sessionLock.Session!);
        if (tag == CryptogramInformationData.Tag)
            return GetCryptogramInformationData(sessionLock.Session!);
        if (tag == InterfaceDeviceSerialNumber.Tag)
            return GetInterfaceDeviceSerialNumber(sessionLock.Session!);
        if (tag == MerchantCategoryCode.Tag)
            return GetMerchantCategoryCode(sessionLock.Session!);
        if (tag == TerminalIdentification.Tag)
            return GetTerminalIdentification(sessionLock.Session!);
        if (tag == TerminalRiskManagementData.Tag)
            return GetTerminalRiskManagementData(sessionLock.Session!);

        return new TagLengthValue(tag, ReadOnlySpan<byte>.Empty);
    }

    private TagLengthValue GetPunatcTrack2(TerminalSession session) =>

        // BUG: Resolve this before implementing Magstripe
        new(PunatcTrack2.Tag, ReadOnlySpan<byte>.Empty);

    private TagLengthValue GetAmountAuthorizedNumeric(TerminalSession session) =>
        session.Transaction.GetAmountAuthorizedNumeric().AsTagLengthValue();

    private TagLengthValue GetAmountOtherNumeric(TerminalSession session) => session.Transaction.GetAmountOtherNumeric().AsTagLengthValue();

    private TagLengthValue GetTerminalCountryCode(TerminalSession session) =>
        session.Transaction.GetTerminalCountryCode().AsTagLengthValue();

    private TagLengthValue GetTerminalVerificationResults(TerminalSession session) =>
        session.Transaction.GetTerminalVerificationResults().AsTagLengthValue();

    private TagLengthValue GetTransactionCurrencyCode(TerminalSession session) =>
        session.Transaction.GetTransactionCurrencyCode().AsTagLengthValue();

    private TagLengthValue GetTransactionDate(TerminalSession session) => session.Transaction.GetTransactionDate().AsTagLengthValue();
    private TagLengthValue GetTransactionType(TerminalSession session) => session.Transaction.GetTransactionType().AsTagLengthValue();
    private TagLengthValue GetUnpredictableNumber(TerminalSession session) => new UnpredictableNumber().AsTagLengthValue();

    private TagLengthValue GetMerchantNameAndLocation(TerminalSession session) =>
        session.TerminalConfiguration.GetMerchantNameAndLocation();

    private TagLengthValue GetPoiInformation(TerminalSession session) => session.TerminalConfiguration.GetPoiInformation();

    // /////////////////////////////////////////////////////////////////////////////

    // BUG: Resolve this through a an interface provided by the Issuer module
    private TagLengthValue GetTerminalActionCodeDenial(TerminalSession session) => throw new NotImplementedException();

    // BUG: Resolve this through a an interface provided by the Issuer module
    private TagLengthValue GetTerminalActionCodeDefault(TerminalSession session) => throw new NotImplementedException();
    private TagLengthValue GetTerminalActionCodeOnline(TerminalSession session) => throw new NotImplementedException();
    private TagLengthValue GetTerminalFloorLimit(TerminalSession session) => session.TerminalConfiguration.GetTerminalFloorLimit();
    private TagLengthValue GetTransactionCurrencyExponent(TerminalSession session) => session.Transaction.GetTransactionCurrencyExponent();

    private TagLengthValue GetTransactionReferenceCurrencyCode(TerminalSession session) =>
        session.TerminalConfiguration.GetTransactionReferenceCurrencyCode();

    private TagLengthValue GetTransactionReferenceCurrencyExponent(TerminalSession session) =>
        session.TerminalConfiguration.GetTransactionReferenceCurrencyExponent();

    private TagLengthValue GetTerminalType(TerminalSession session) => session.TerminalConfiguration.GetTerminalType();
    private TagLengthValue GetTerminalCapabilities(TerminalSession session) => session.TerminalConfiguration.GetTerminalCapabilities();
    private TagLengthValue GetAcquirerIdentifier(TerminalSession session) => session.TerminalConfiguration.GetAcquirerIdentifier();

    // BUG: Resolve common terminal services
    private TagLengthValue GetCvmResults(TerminalSession session) => throw new NotImplementedException();
    private TagLengthValue GetCryptogramInformationData(TerminalSession session) => throw new NotImplementedException();

    private TagLengthValue GetInterfaceDeviceSerialNumber(TerminalSession session) =>
        session.TerminalConfiguration.GetInterfaceDeviceSerialNumber();

    private TagLengthValue GetMerchantCategoryCode(TerminalSession session) => session.TerminalConfiguration.GetMerchantCategoryCode();
    private TagLengthValue GetMerchantIdentifier(TerminalSession session) => session.TerminalConfiguration.GetMerchantIdentifier();
    private TagLengthValue GetTerminalIdentification(TerminalSession session) => session.TerminalConfiguration.GetTerminalIdentification();

    private TagLengthValue GetTerminalRiskManagementData(TerminalSession session) =>
        session.TerminalConfiguration.GetTerminalRiskManagementData();

    #endregion
}