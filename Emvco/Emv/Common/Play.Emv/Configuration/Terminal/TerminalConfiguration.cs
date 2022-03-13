using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Core.Math;
using Play.Emv.DataElements;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.Configuration;

public abstract record TerminalConfiguration
{
    #region Instance Values

    private readonly MerchantIdentifier _MerchantIdentifier;
    private readonly MerchantCategoryCode _MerchantCategoryCode;
    private readonly MerchantNameAndLocation _MerchantNameAndLocation;
    private readonly AcquirerIdentifier _AcquirerIdentifier;
    private readonly TerminalIdentification _TerminalIdentification;
    private readonly InterfaceDeviceSerialNumber _InterfaceDeviceSerialNumber;
    private readonly LanguagePreference _LanguagePreference;
    private readonly TerminalCapabilities _TerminalCapabilities;
    private readonly TerminalCountryCode _TerminalCountryCode;
    private readonly TerminalFloorLimit _TerminalFloorLimit;
    private readonly TerminalType _TerminalType;
    private readonly TransactionCurrencyCode _TransactionCurrencyCode;
    private readonly ApplicationVersionNumberTerminal _ApplicationVersionNumberTerminal;
    private readonly DataStorageRequestedOperatorId _DataStorageRequestedOperatorId;

    // BUG: TerminalRiskManagementData is transient per transaction. This should live with the transaction session, not the terminal configuration
    private readonly TerminalRiskManagementData _TerminalRiskManagementData;
    private readonly PoiInformation _PoiInformation;
    private readonly AdditionalTerminalCapabilities _AdditionalTerminalCapabilities;
    private readonly TransactionReferenceCurrencyCode _TransactionReferenceCurrencyCode;
    private readonly TransactionReferenceCurrencyExponent _TransactionReferenceCurrencyExponent;
    private readonly Percentage _BiasedRandomSelectionPercentage;
    private readonly Percentage _RandomSelectionTargetPercentage;

    /// <summary>
    ///     This is a threshold amount, simply referred to as the threshold value, which can be zero or a positive number
    ///     smaller than the Terminal Floor Limit
    /// </summary>
    private readonly ulong _ThresholdValueForBiasedRandomSelection;

    private readonly List<TagLengthValue> _TagLengthValues = new List<TagLengthValue>();

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
        ApplicationVersionNumberTerminal applicationVersionNumberTerminal,
        MerchantNameAndLocation merchantNameAndLocation,
        TerminalRiskManagementData terminalRiskManagementData,
        Percentage biasedRandomSelectionPercentage,
        Percentage randomSelectionTargetPercentage,
        ulong thresholdValueForBiasedRandomSelection,
        PoiInformation poiInformation,
        AdditionalTerminalCapabilities additionalTerminalCapabilities,
        TransactionReferenceCurrencyCode transactionReferenceCurrencyCode,
        TransactionReferenceCurrencyExponent transactionReferenceCurrencyExponent,
        AcquirerIdentifier acquirerIdentifier,
        DataStorageRequestedOperatorId dataStorageRequestedOperatorId)
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
        _MerchantNameAndLocation = merchantNameAndLocation;
        _TerminalRiskManagementData = terminalRiskManagementData;
        _BiasedRandomSelectionPercentage = biasedRandomSelectionPercentage;
        _RandomSelectionTargetPercentage = randomSelectionTargetPercentage;
        _ThresholdValueForBiasedRandomSelection = thresholdValueForBiasedRandomSelection;
        _PoiInformation = poiInformation;
        _AdditionalTerminalCapabilities = additionalTerminalCapabilities;
        _TransactionReferenceCurrencyCode = transactionReferenceCurrencyCode;
        _TransactionReferenceCurrencyExponent = transactionReferenceCurrencyExponent;
        _AcquirerIdentifier = acquirerIdentifier;
        _DataStorageRequestedOperatorId = dataStorageRequestedOperatorId;

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
        _TagLengthValues.Add(_MerchantNameAndLocation);
        _TagLengthValues.Add(_TerminalRiskManagementData);
        _TagLengthValues.Add(_PoiInformation);
        _TagLengthValues.Add(_AdditionalTerminalCapabilities);
        _TagLengthValues.Add(_TransactionReferenceCurrencyCode);
        _TagLengthValues.Add(_TransactionReferenceCurrencyExponent);
        _TagLengthValues.Add(_AcquirerIdentifier);
    }

    #endregion

    #region Instance Members

    public TerminalRiskManagementData GetTerminalRiskManagementData() => _TerminalRiskManagementData;
    public AcquirerIdentifier GetAcquirerIdentifier() => _AcquirerIdentifier;
    public TerminalCapabilities GetTerminalCapabilities() => _TerminalCapabilities;
    public TransactionReferenceCurrencyCode GetTransactionReferenceCurrencyCode() => _TransactionReferenceCurrencyCode;
    public TransactionReferenceCurrencyExponent GetTransactionReferenceCurrencyExponent() => _TransactionReferenceCurrencyExponent;
    public AdditionalTerminalCapabilities GetAdditionalTerminalCapabilities() => _AdditionalTerminalCapabilities;
    public PoiInformation GetPoiInformation() => _PoiInformation;

    // HACK: This is not implemented. Find how we're supposed to store the random and biased percentages and biased threshold value
    public TerminalRiskConfiguration GetTerminalRiskConfiguration(CultureProfile culture) =>
        new TerminalRiskConfiguration(culture, _TerminalRiskManagementData, _BiasedRandomSelectionPercentage,
            new Money(_ThresholdValueForBiasedRandomSelection, culture), _RandomSelectionTargetPercentage, _TerminalFloorLimit);

    public MerchantNameAndLocation GetMerchantNameAndLocation() => _MerchantNameAndLocation;
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
    public DataStorageRequestedOperatorId GetDataStorageRequestedOperatorId() => _DataStorageRequestedOperatorId;

    #endregion
}