using Play.Ber.DataObjects;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Globalization;
using Play.Globalization.Currency;

namespace _TempConfiguration;

public abstract record TerminalConfiguration
{
    #region Instance Values

    private readonly MerchantIdentifier _MerchantIdentifier;
    private readonly MerchantCategoryCode _MerchantCategoryCode;
    private readonly MerchantNameAndLocation _MerchantNameAndLocation;
    private readonly AcquirerIdentifier _AcquirerIdentifier;
    private readonly InterfaceDeviceSerialNumber _InterfaceDeviceSerialNumber;
    private readonly TerminalIdentification _TerminalIdentification;
    private readonly TerminalCapabilities _TerminalCapabilities;
    private readonly TerminalCountryCode _TerminalCountryCode;
    private readonly TerminalFloorLimit _TerminalFloorLimit;
    private readonly TerminalType _TerminalType;
    private readonly AdditionalTerminalCapabilities _AdditionalTerminalCapabilities;
    private readonly TransactionReferenceCurrencyCode _TransactionReferenceCurrencyCode;
    private readonly TransactionReferenceCurrencyExponent _TransactionReferenceCurrencyExponent;
    private readonly PoiInformation _PoiInformation;
    private readonly LanguagePreference _LanguagePreference;
    private readonly TransactionCurrencyCode _TransactionCurrencyCode;
    private readonly TransactionCurrencyExponent _TransactionCurrencyExponent;
    private readonly TerminalActionCodeDefault _TerminalActionCodeDefault;
    private readonly TerminalActionCodeOnline _TerminalActionCodeOnline;
    private readonly TerminalActionCodeDenial _TerminalActionCodeDenial;

    // BUG: TerminalRiskManagementData is transient per transaction. This should live with the transaction session, not the terminal configuration
    private readonly TerminalRiskManagementData _TerminalRiskManagementData;
    private readonly DataStorageRequestedOperatorId _DataStorageRequestedOperatorId;
    private readonly Probability _BiasedRandomSelectionProbability;
    private readonly Probability _RandomSelectionTargetProbability;

    /// <summary>
    ///     This is a threshold amount, simply referred to as the threshold value, which can be zero or a positive number
    ///     smaller than the Terminal Floor Limit
    /// </summary>
    private readonly ulong _ThresholdValueForBiasedRandomSelection;

    private readonly List<TagLengthValue> _TagLengthValues = new();

    #endregion

    #region Constructor

    protected TerminalConfiguration(
        TerminalIdentification terminalIdentification, MerchantIdentifier merchantIdentifier, InterfaceDeviceSerialNumber interfaceDeviceSerialNumber,
        TransactionCurrencyCode transactionCurrencyCode, TerminalCapabilities terminalCapabilities, TerminalFloorLimit terminalFloorLimit,
        TerminalType terminalType, TerminalCountryCode terminalCountryCode, MerchantCategoryCode merchantCategoryCode, LanguagePreference languagePreference,
        MerchantNameAndLocation merchantNameAndLocation, TerminalRiskManagementData terminalRiskManagementData, Probability biasedRandomSelectionProbability,
        Probability randomSelectionTargetProbability, ulong thresholdValueForBiasedRandomSelection, PoiInformation poiInformation,
        AdditionalTerminalCapabilities additionalTerminalCapabilities, TransactionReferenceCurrencyCode transactionReferenceCurrencyCode,
        TransactionReferenceCurrencyExponent transactionReferenceCurrencyExponent, AcquirerIdentifier acquirerIdentifier,
        DataStorageRequestedOperatorId dataStorageRequestedOperatorId, TransactionCurrencyExponent transactionCurrencyExponent,
        TerminalActionCodeDefault terminalActionCodeDefault, TerminalActionCodeOnline terminalActionCodeOnline,
        TerminalActionCodeDenial terminalActionCodeDenial)
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
        _MerchantNameAndLocation = merchantNameAndLocation;
        _TerminalRiskManagementData = terminalRiskManagementData;
        _BiasedRandomSelectionProbability = biasedRandomSelectionProbability;
        _RandomSelectionTargetProbability = randomSelectionTargetProbability;
        _ThresholdValueForBiasedRandomSelection = thresholdValueForBiasedRandomSelection;
        _PoiInformation = poiInformation;
        _AdditionalTerminalCapabilities = additionalTerminalCapabilities;
        _TransactionReferenceCurrencyCode = transactionReferenceCurrencyCode;
        _TransactionReferenceCurrencyExponent = transactionReferenceCurrencyExponent;
        _AcquirerIdentifier = acquirerIdentifier;
        _DataStorageRequestedOperatorId = dataStorageRequestedOperatorId;
        _TransactionCurrencyExponent = transactionCurrencyExponent;
        _TerminalActionCodeDefault = terminalActionCodeDefault;
        _TerminalActionCodeOnline = terminalActionCodeOnline;
        _TerminalActionCodeDenial = terminalActionCodeDenial;

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
        _TagLengthValues.Add(_MerchantNameAndLocation);
        _TagLengthValues.Add(_TerminalRiskManagementData);
        _TagLengthValues.Add(_PoiInformation);
        _TagLengthValues.Add(_AdditionalTerminalCapabilities);
        _TagLengthValues.Add(_TransactionReferenceCurrencyCode);
        _TagLengthValues.Add(_TransactionReferenceCurrencyExponent);
        _TagLengthValues.Add(_AcquirerIdentifier);
        _TagLengthValues.Add(_TransactionCurrencyExponent);
    }

    #endregion

    #region Instance Members

    public TransactionCurrencyExponent GetTransactionCurrencyExponent() => _TransactionCurrencyExponent;
    public TerminalRiskManagementData GetTerminalRiskManagementData() => _TerminalRiskManagementData;
    public AcquirerIdentifier GetAcquirerIdentifier() => _AcquirerIdentifier;
    public TerminalCapabilities GetTerminalCapabilities() => _TerminalCapabilities;
    public TransactionReferenceCurrencyCode GetTransactionReferenceCurrencyCode() => _TransactionReferenceCurrencyCode;
    public TransactionReferenceCurrencyExponent GetTransactionReferenceCurrencyExponent() => _TransactionReferenceCurrencyExponent;
    public AdditionalTerminalCapabilities GetAdditionalTerminalCapabilities() => _AdditionalTerminalCapabilities;
    public PoiInformation GetPoiInformation() => _PoiInformation;

    // HACK: This is not implemented. Find how we're supposed to store the random and biased percentages and biased threshold value
    public TerminalRiskConfiguration GetTerminalRiskConfiguration(CultureProfile culture) =>
        new(culture, _TerminalRiskManagementData, _BiasedRandomSelectionProbability,
            new Money(_ThresholdValueForBiasedRandomSelection, culture.GetNumericCurrencyCode()), _RandomSelectionTargetProbability, _TerminalFloorLimit);

    public MerchantNameAndLocation GetMerchantNameAndLocation() => _MerchantNameAndLocation;
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
    public bool NoCardVerificationMethodRequired() => _TerminalCapabilities.IsNoCardVerificationMethodRequiredSet();
    public bool PlaintextPinForIccVerification() => _TerminalCapabilities.IsPlaintextPinForIccVerificationSupported();
    public bool SignaturePaper() => _TerminalCapabilities.IsSignaturePaperSupported();
    public bool StaticDataAuthentication() => _TerminalCapabilities.IsStaticDataAuthenticationSupported();
    public DataStorageRequestedOperatorId GetDataStorageRequestedOperatorId() => _DataStorageRequestedOperatorId;

    #endregion
}