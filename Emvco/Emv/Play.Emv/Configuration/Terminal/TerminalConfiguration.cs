using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.DataElements;

namespace Play.Emv.Configuration;

public abstract record TerminalConfiguration
{
    #region Instance Values

    private readonly MerchantIdentifier _MerchantIdentifier;
    private readonly TerminalIdentification _TerminalIdentification;
    private readonly InterfaceDeviceSerialNumber _InterfaceDeviceSerialNumber;
    private readonly LanguagePreference _LanguagePreference;
    private readonly TerminalCapabilities _TerminalCapabilities;
    private readonly TerminalCountryCode _TerminalCountryCode;
    private readonly TerminalFloorLimit _TerminalFloorLimit;
    private readonly TerminalType _TerminalType;
    private readonly TransactionCurrencyCode _TransactionCurrencyCode;
    private readonly MerchantCategoryCode _MerchantCategoryCode;
    private readonly ApplicationVersionNumberTerminal _ApplicationVersionNumberTerminal;
    private readonly List<TagLengthValue> _TagLengthValues = new();

    #endregion

    #region Constructor

    protected TerminalConfiguration(
        TerminalIdentification terminalIdentification,
        MerchantIdentifier merchantIdentifier,
        InterfaceDeviceSerialNumber interfaceDeviceSerialNumber,
        TransactionCurrencyCode transactionCurrencyCode,
        TerminalCapabilities terminalCapabilities,
        TerminalFloorLimit terminalFloorLimit,
        TerminalType terminalType,
        TerminalCountryCode terminalCountryCode,
        MerchantCategoryCode merchantCategoryCode,
        LanguagePreference languagePreference,
        ApplicationVersionNumberTerminal applicationVersionNumberTerminal)
    {
        _TerminalIdentification = terminalIdentification;
        _TransactionCurrencyCode = transactionCurrencyCode;
        _TerminalCapabilities = terminalCapabilities;
        _TerminalFloorLimit = terminalFloorLimit;
        _TerminalType = terminalType;
        _TerminalCountryCode = terminalCountryCode;
        _MerchantCategoryCode = merchantCategoryCode;
        _LanguagePreference = languagePreference;
        _MerchantIdentifier = merchantIdentifier;
        _InterfaceDeviceSerialNumber = interfaceDeviceSerialNumber;
        _ApplicationVersionNumberTerminal = applicationVersionNumberTerminal;

        _TagLengthValues.Add(_TerminalIdentification);
        _TagLengthValues.Add(_TransactionCurrencyCode);
        _TagLengthValues.Add(_TerminalCapabilities);
        _TagLengthValues.Add(_TerminalFloorLimit);
        _TagLengthValues.Add(_TerminalType);
        _TagLengthValues.Add(_TerminalCountryCode);
        _TagLengthValues.Add(_MerchantCategoryCode);
        _TagLengthValues.Add(_LanguagePreference);
        _TagLengthValues.Add(_MerchantIdentifier);
        _TagLengthValues.Add(_InterfaceDeviceSerialNumber);
        _TagLengthValues.Add(_ApplicationVersionNumberTerminal);
    }

    #endregion

    #region Instance Members

    public ApplicationVersionNumberTerminal AsApplicationVersionNumberTerminal() => _ApplicationVersionNumberTerminal;
    public TagLengthValue[] AsTagLengthValues() => _TagLengthValues.ToArray();
    public MerchantCategoryCode GetMerchantCategoryCode() => _MerchantCategoryCode;
    public TransactionCurrencyCode GetTransactionCurrencyCode() => _TransactionCurrencyCode;
    public MerchantIdentifier GetMerchantIdentifier() => _MerchantIdentifier;
    public InterfaceDeviceSerialNumber GetInterfaceDeviceSerialNumber() => _InterfaceDeviceSerialNumber;
    public bool CardCapture() => _TerminalCapabilities.IsCardCaptureSupported();
    public bool CombinedDataAuthentication() => _TerminalCapabilities.IsCombinedDataAuthenticationSupported();
    public bool DynamicDataAuthentication() => _TerminalCapabilities.IsDynamicDataAuthenticationSupported();
    public bool EncipheredPinForOfflineVerification() => _TerminalCapabilities.IsEncipheredPinForOfflineVerificationSupported();
    public bool EncipheredPinForOnlineVerification() => _TerminalCapabilities.IsEncipheredPinForOnlineVerificationSupported();
    public LanguagePreference GetLanguagePreference() => _LanguagePreference;

    //public LanguageCodes[] GetLanguageCodes() =>
    //    _SupportedCultures.Select(a => a.GetAlpha2LanguageCode()).ToArray(); 

    //public LanguageCodes GetPreferredLanguageCode() => _SupportedCultures[0].GetAlpha2LanguageCode();

    //public CurrencyCodes[] GetSupportedCurrencies() =>
    //    _SupportedCultures.Select(a => a.GetNumericCurrencyCode()).ToArray();
    public TerminalCountryCode GetTerminalCountryCode() => _TerminalCountryCode;
    public TerminalFloorLimit GetTerminalFloorLimit() => _TerminalFloorLimit;
    public TerminalIdentification GetTerminalIdentification() => _TerminalIdentification;
    public TerminalType GetTerminalType() => _TerminalType;
    public bool IcWithContacts() => _TerminalCapabilities.IsIcWithContactsSupported();
    public bool MagneticStripe() => _TerminalCapabilities.IsMagneticStripeSupported();
    public bool ManualKeyEntry() => _TerminalCapabilities.IsManualKeyEntrySupported();
    public bool NoCardVerificationMethodRequired() => _TerminalCapabilities.IsNoCardVerificationMethodRequiredSupported();
    public bool PlaintextPinForIccVerification() => _TerminalCapabilities.IsPlaintextPinForIccVerificationSupported();
    public bool SignaturePaper() => _TerminalCapabilities.IsSignaturePaperSupported();
    public bool StaticDataAuthentication() => _TerminalCapabilities.IsStaticDataAuthenticationSupported();

    #endregion
}