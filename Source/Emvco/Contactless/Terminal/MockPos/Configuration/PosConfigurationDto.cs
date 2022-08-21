using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MockPos.Configuration
{
    public class TerminalConfigurationDto
    {
        #region Instance Values

        public string TerminalIdentification { get; set; }
        public string MerchantIdentifier { get; set; }
        public string MerchantCategoryCode { get; set; }
        public string MerchantNameAndLocation { get; set; }
        public string AcquirerIdentifier { get; set; }
        public string InterfaceDeviceSerialNumber { get; set; }
        public string TerminalType { get; set; }
        public string TerminalCapabilities { get; set; }
        public string AdditionalTerminalCapabilities { get; set; }
        public string TerminalCountryCode { get; set; }
        public string LanguagePreference { get; set; }
        public string TransactionCurrencyCode { get; set; }
        public string TransactionCurrencyExponent { get; set; }
        public string TransactionReferenceCurrencyCode { get; set; }
        public string TransactionReferenceCurrencyExponent { get; set; }
        public string TerminalFloorLimit { get; set; }
        public string TerminalRiskManagementData { get; set; }
        public string DataStorageRequestedOperatorId { get; set; }
        public string BiasedRandomSelectionProbability { get; set; }
        public string RandomSelectionTargetProbability { get; set; }
        public string ThresholdValueForBiasedRandomSelection { get; set; }
        public string MaxNumberOfTornTransactionLogRecords { get; set; }
        public string MaxLifetimeOfTornTransactionLogRecords { get; set; }
        public SequenceConfiguration SequenceConfiguration { get; set; }

        #endregion
    }

    public class SequenceConfiguration
    {
        #region Instance Values

        public int Threshold { get; set; }
        public int InitializationValue { get; set; }

        #endregion
    }

    public class ReaderConfigurationDto
    { }

    public class TransactionProfileDto
    {
        #region Instance Values

        public int KernelId { get; set; }
        public string? ApplicationId { get; set; }
        public int TransactionType { get; set; }
        public int ApplicationPriorityIndicator { get; set; }
        public int ContactlessTransactionLimit { get; set; }
        public int ContactlessFloorLimit { get; set; }
        public int CvmRequiredLimit { get; set; }
        public int KernelConfiguration { get; set; }
        public int TerminalTransactionQualifiers { get; set; }
        public bool IsStatusCheckSupported { get; set; }
        public bool IsZeroAmountAllowed { get; set; }
        public bool IsZeroAmountAllowedForOffline { get; set; }
        public bool IsExtendedSelectionSupported { get; set; }

        #endregion
    }

    public class KernelConfigurationDto
    {
        #region Instance Values

        public int KernelId { get; set; }
        public List<TagLengthValueDto> TagLengthValues { get; set; }

        #endregion
    }

    public class TagLengthValueDto
    {
        #region Instance Values

        public string? Name { get; set; }
        public string? Tag { get; set; }
        public string? Value { get; set; }

        #endregion
    }

    public class DisplayConfigurationDto
    {
        #region Instance Values

        public string? MessageHoldTime { get; set; }

        [JsonPropertyName(nameof(DisplayMessageSets))]
        public List<DisplayMessageSet> DisplayMessageSets { get; set; }

        #endregion
    }

    public class DisplayMessageSet
    {
        #region Instance Values

        public string LanguageCode { get; set; }
        public string CountryCode { get; set; }

        [JsonPropertyName(nameof(DisplayMessages))]
        public List<DisplayMessageDto> DisplayMessages { get; set; }

        #endregion
    }

    public class DisplayMessageDto
    {
        #region Instance Values

        public string MessageIdentifier { get; set; }
        public string Message { get; set; }

        #endregion
    }

    public class ProximityCouplingDeviceConfigurationDto
    {
        #region Instance Values

        public int TimeoutValue { get; set; }

        #endregion
    }

    public class CertificateAuthorityConfigurationDto
    {
        #region Instance Values

        [JsonPropertyName(nameof(Certificates))] public List<CertificateDto> Certificates { get; set; }

        #endregion
    }

    public class CertificateDto
    {
        #region Instance Values

        public string RegisteredApplicationProviderIndicator { get; set; }
        public string PublicKeyIndex { get; set; }
        public int HashAlgorithmIndicator { get; set; }
        public int PublicKeyAlgorithmIndicator { get; set; }
        public string ActivationDate { get; set; }
        public string ExpirationDate { get; set; }
        public int Exponent { get; set; }
        public string Modulus { get; set; }
        public string Checksum { get; set; }

        #endregion
    }

    internal class PosConfigurationDto
    {
        #region Instance Values

        public int CompanyId { get; set; }
        public int MerchantId { get; set; }
        public int StoreId { get; set; }
        public int TerminalId { get; set; }

        [JsonPropertyName(nameof(TerminalConfiguration))]
        public TerminalConfigurationDto? TerminalConfiguration { get; set; }

        [JsonPropertyName(nameof(TransactionProfiles))]
        public List<TransactionProfileDto> TransactionProfiles { get; set; } = new();

        [JsonPropertyName(nameof(KernelConfigurations))]
        public List<KernelConfigurationDto> KernelConfigurations { get; set; } = new();

        [JsonPropertyName(nameof(DisplayConfiguration))]
        public DisplayConfigurationDto DisplayConfiguration { get; set; } = new();

        [JsonPropertyName(nameof(ProximityCouplingDeviceConfiguration))]
        public ProximityCouplingDeviceConfigurationDto ProximityCouplingDeviceConfiguration { get; set; }

        [JsonPropertyName(nameof(CertificateAuthorityConfiguration))]
        public CertificateAuthorityConfigurationDto CertificateAuthorityConfiguration { get; set; }

        #endregion
    }
}