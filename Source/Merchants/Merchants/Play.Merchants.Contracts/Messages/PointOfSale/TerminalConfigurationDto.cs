namespace Play.MerchantPortal.Contracts.Messages.PointOfSale;

public record TerminalConfigurationDto
{
    public string TerminalIdentification { get; set; } = string.Empty;
    public string MerchantIdentifier { get; set; } = string.Empty;
    public string MerchantCategoryCode { get; set; } = string.Empty;
    public string MerchantNameAndLocation { get; set; } = string.Empty;
    public string AcquirerIdentifier { get; set; } = string.Empty;
    public string InterfaceDeviceSerialNumber { get; set; } = string.Empty;
    public string TerminalType { get; set; } = string.Empty;
    public string TerminalCapabilities { get; set; } = string.Empty;//Capabilities
    public string AdditionalTerminalCapabilities { get; set; } = string.Empty; //AdditionalCapabilities
    public string TerminalCountryCode { get; set; } = string.Empty; //CountryCode
    public string LanguagePreference { get; set; } = string.Empty;
    public string TransactionCurrencyCode { get; set; } = string.Empty;
    public string TransactionCurrencyExponent { get; set; } = string.Empty;
    public string TransactionReferenceCurrencyCode { get; set; } = string.Empty;
    public string TransactionReferenceCurrencyExponent { get; set; } = string.Empty;
    public string TerminalFloorLimit { get; set; } = string.Empty;
    public string TerminalRiskManagementData { get; set; } = string.Empty;
    public int DataStorageRequestedOperatorId { get; set; }
    public string BiasedRandomSelectionProbability { get; set; } = string.Empty;
    public string BiasedRandomSelectionTargetProbability { get; set; } = string.Empty;
    public string ThresholdValueForBiasedRandomSelection { get; set; } = string.Empty;
    public string MaxNumberOfTornTransactionLogRecords { get; set; } = string.Empty;
    public string MaxLifetimeOfTornTransactionLogRecords { get; set; } = string.Empty;
    public SequenceConfigurationDto SequenceConfiguration { get; set; } = default!;
}
