using System;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.Terminal.Services;

// HACK: The Terminal should probably implement a database style repository where the requested tags can be retrieved by Tag
internal class TerminalQueryResolver
{
    #region Instance Values

    private readonly IPerformTerminalActionAnalysis _TerminalActionAnalysisService;
    private readonly IManageTerminalRisk _TerminalRiskManager;

    #endregion

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

    #endregion

    // services requested by DEK?
    // 9F5B Issuer Script Results
    // TerminalActionCodeDefault
    // TerminalActionCodeOnline
    // TerminalActionCodeOnline
    // 9F34    Cardholder Verification Method (CVM) Results
}

/*                 
9F22    Certification Authority Public Key Index (PKI)
9F21    Transaction Time
9F1E    Interface Device (IFD) Serial Number
9F1D    Terminal Risk Management Data
9F1C    Terminal Identification
9F1B    Terminal Floor Limit
9F1B    Terminal Floor Limit
9F1A    Terminal Country Code
9F1A    Terminal Country Code
9F1A    Terminal Country Code
9F1A    Terminal Country Code
9F1A    Terminal Country Code
9F16    Merchant Identifier
9F15    Merchant Category Code (MCC)
9F09    Application Version Number
9F06    Application Identifier (AID), Terminal
9F06    Application Identifier (AID), Terminal
9F04    Amount, Other (Binary)
9F03    Amount, Other (Numeric)
9F03    Amount, Other (Numeric)
9F03    Amount, Other (Numeric)
9F03    Amount, Other (Numeric)
9F03    Amount, Other (Numeric)
9F02    Amount, Authorised (Numeric)
9F02    Amount, Authorised (Numeric)
9F02    Amount, Authorised (Numeric)
9F02    Amount, Authorised (Numeric)
9F02    Amount, Authorised (Numeric)
9F01    Acquirer Identifier
9F01    Acquirer Identifier
5F57    Account Type
5F3D    Transaction Reference Currency Exponent
5F3C    Transaction Reference Currency Code
5F36    Transaction Currency Exponent
5F2A    Transaction Currency Code
5F2A    Transaction Currency Code
5F2A    Transaction Currency Code
5F2A    Transaction Currency Code
9C      Transaction Type
9C      Transaction Type
9C      Transaction Type
9C      Transaction Type
9B      Transaction Status Information (TSI)
9A      Transaction Date
9A      Transaction Date
9A      Transaction Date
9A      Transaction Date
99      Transaction Personal Identification Number (PIN) Data
98      Transaction Certificate (TC) Hash Value
95      Terminal Verification Results (TVR)
95      Terminal Verification Results (TVR)
95      Terminal Verification Results (TVR)
95      Terminal Verification Results (TVR)
8A      Authorisation Response Code (ARC)
8A      Authorisation Response Code (ARC)
83      Command Template
81      Amount, Authorised (Binary) 
 
 
 */