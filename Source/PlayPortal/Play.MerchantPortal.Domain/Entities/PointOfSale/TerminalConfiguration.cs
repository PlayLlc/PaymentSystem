namespace MerchantPortal.Core.Entities.PointOfSale;

public class TerminalConfiguration
{
    public string TerminalIdentification { get; set; }
    public string MerchantIdentifier { get; set; }
    public string MerchantCategoryCode { get; set; }
    public string MerchantNameAndLocation { get; set; }
    public string AcquirerIdentifier { get; set; }
    public string InterfaceDeviceSerialNumber { get; set; }
    public string TerminalType { get; set; }
    public string TerminalCapabilities { get; set; } //Capabilities
    public string AdditionalTerminalCapabilities { get; set; } //AdditionalCapabilities
    public string TerminalCountryCode { get; set; } //CountryCode
    public string LanguagePreference { get; set; }
    public string TransactionCurrencyCode { get; set; }
    public string TransactionCurrencyExponent { get; set; }
    public string TransactionReferenceCurrencyCode { get; set; }
    public string TransactionReferenceCurrencyExponent { get; set; }
    public string TerminalFloorLimit { get; set; }
    public string TerminalRiskManagementData { get; set; }
    public int DataStorageRequestedOperatorId { get; set; }
    public string BiasedRandomSelectionProbability { get; set; }
    public string BiasedRandomSelectionTargetProbability { get; set; }
    public string ThresholdValueForBiasedRandomSelection { get; set; }
    public string MaxNumberOfTornTransactionLogRecords { get; set; }
    public string MaxLifetimeOfTornTransactionLogRecords { get; set; }
    public SequenceConfiguration SequenceConfiguration { get; set; }
}
