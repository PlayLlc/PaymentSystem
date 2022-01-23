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
    private readonly TerminalIdentification _TerminalIdentification;
    private readonly InterfaceDeviceSerialNumber _InterfaceDeviceSerialNumber;
    private readonly LanguagePreference _LanguagePreference;
    private readonly TerminalCapabilities _TerminalCapabilities;
    private readonly TerminalCountryCode _TerminalCountryCode;
    private readonly TerminalFloorLimit _TerminalFloorLimit;
    private readonly TerminalType _TerminalType;
    private readonly TransactionCurrencyCode _TransactionCurrencyCode;
    private readonly ApplicationVersionNumberTerminal _ApplicationVersionNumberTerminal;
    private readonly TerminalRiskManagementData _TerminalRiskManagementData;
    private readonly Percentage _BiasedRandomSelectionPercentage;
    private readonly Percentage _RandomSelectionTargetPercentage;
    private readonly ulong _ThresholdValueForBiasedRandomSelection;

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
        ApplicationVersionNumberTerminal applicationVersionNumberTerminal,
        MerchantNameAndLocation merchantNameAndLocation,
        TerminalRiskManagementData terminalRiskManagementData,
        Percentage biasedRandomSelectionPercentage,
        Percentage randomSelectionTargetPercentage,
        ulong thresholdValueForBiasedRandomSelection)
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
    }

    #endregion

    #region Instance Members

    // HACK: This is not implemented. Find how we're supposed to store the random and biased percentages and biased threshold value
    public TerminalRiskConfiguration GetTerminalRiskConfiguration(CultureProfile culture)
    {
        return new TerminalRiskConfiguration(culture, _TerminalRiskManagementData, _BiasedRandomSelectionPercentage,
                                             new Money(_ThresholdValueForBiasedRandomSelection, culture), _RandomSelectionTargetPercentage,
                                             _TerminalFloorLimit);
    }

    public MerchantNameAndLocation GetMerchantNameAndLocation()
    {
        return _MerchantNameAndLocation;
    }

    public ApplicationVersionNumberTerminal AsApplicationVersionNumberTerminal()
    {
        return _ApplicationVersionNumberTerminal;
    }

    public TagLengthValue[] AsTagLengthValues()
    {
        return _TagLengthValues.ToArray();
    }

    public MerchantCategoryCode GetMerchantCategoryCode()
    {
        return _MerchantCategoryCode;
    }

    public TransactionCurrencyCode GetTransactionCurrencyCode()
    {
        return _TransactionCurrencyCode;
    }

    public MerchantIdentifier GetMerchantIdentifier()
    {
        return _MerchantIdentifier;
    }

    public InterfaceDeviceSerialNumber GetInterfaceDeviceSerialNumber()
    {
        return _InterfaceDeviceSerialNumber;
    }

    public bool CardCapture()
    {
        return _TerminalCapabilities.IsCardCaptureSupported();
    }

    public bool CombinedDataAuthentication()
    {
        return _TerminalCapabilities.IsCombinedDataAuthenticationSupported();
    }

    public bool DynamicDataAuthentication()
    {
        return _TerminalCapabilities.IsDynamicDataAuthenticationSupported();
    }

    public bool EncipheredPinForOfflineVerification()
    {
        return _TerminalCapabilities.IsEncipheredPinForOfflineVerificationSupported();
    }

    public bool EncipheredPinForOnlineVerification()
    {
        return _TerminalCapabilities.IsEncipheredPinForOnlineVerificationSupported();
    }

    public LanguagePreference GetLanguagePreference()
    {
        return _LanguagePreference;
    }

    //public LanguageCodes[] GetLanguageCodes() =>
    //    _SupportedCultures.Select(a => a.GetAlpha2LanguageCode()).ToArray(); 

    //public LanguageCodes GetPreferredLanguageCode() => _SupportedCultures[0].GetAlpha2LanguageCode();

    //public CurrencyCodes[] GetSupportedCurrencies() =>
    //    _SupportedCultures.Select(a => a.GetNumericCurrencyCode()).ToArray();
    public TerminalCountryCode GetTerminalCountryCode()
    {
        return _TerminalCountryCode;
    }

    public TerminalFloorLimit GetTerminalFloorLimit()
    {
        return _TerminalFloorLimit;
    }

    public TerminalIdentification GetTerminalIdentification()
    {
        return _TerminalIdentification;
    }

    public TerminalType GetTerminalType()
    {
        return _TerminalType;
    }

    public bool IcWithContacts()
    {
        return _TerminalCapabilities.IsIcWithContactsSupported();
    }

    public bool MagneticStripe()
    {
        return _TerminalCapabilities.IsMagneticStripeSupported();
    }

    public bool ManualKeyEntry()
    {
        return _TerminalCapabilities.IsManualKeyEntrySupported();
    }

    public bool NoCardVerificationMethodRequired()
    {
        return _TerminalCapabilities.IsNoCardVerificationMethodRequiredSupported();
    }

    public bool PlaintextPinForIccVerification()
    {
        return _TerminalCapabilities.IsPlaintextPinForIccVerificationSupported();
    }

    public bool SignaturePaper()
    {
        return _TerminalCapabilities.IsSignaturePaperSupported();
    }

    public bool StaticDataAuthentication()
    {
        return _TerminalCapabilities.IsStaticDataAuthenticationSupported();
    }

    #endregion
}