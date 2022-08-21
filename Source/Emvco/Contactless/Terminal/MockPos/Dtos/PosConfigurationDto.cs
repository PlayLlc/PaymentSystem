using System.Text.Json.Serialization;

namespace MockPos.Dtos
{
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

        [JsonPropertyName(nameof(KernelPersistentConfigurations))]
        public List<KernelPersistentConfigurationDto> KernelPersistentConfigurations { get; set; } = new();

        [JsonPropertyName(nameof(DisplayConfiguration))]
        public DisplayConfigurationDto DisplayConfiguration { get; set; } = new();

        [JsonPropertyName(nameof(ProximityCouplingDeviceConfiguration))]
        public ProximityCouplingDeviceConfigurationDto? ProximityCouplingDeviceConfiguration { get; set; }

        [JsonPropertyName(nameof(CertificateAuthorityConfiguration))]
        public CertificateAuthorityConfigurationDto? CertificateAuthorityConfiguration { get; set; }

        #endregion
    }
}