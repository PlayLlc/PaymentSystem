using System.Text.Json.Serialization;

using Play.Ber.DataObjects;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Configuration;
using Play.Emv.Display.Configuration;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;

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

        #region Instance Members

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        public TerminalConfiguration GetTerminalConfiguration() => TerminalConfiguration!.Decode();

        public KernelPersistentConfiguration[] GetKernelPersistent()
        {
            return KernelPersistentConfigurations.Select(a => a.Decode()).ToArray();
        }

        public DisplayConfiguration GetDisplayConfiguration() => DisplayConfiguration!.Decode();
        public PcdConfiguration GetPcdConfiguration() => ProximityCouplingDeviceConfiguration!.Decode();
        public CertificateAuthorityDataset[] GetCertificateAuthorityDatasets() => CertificateAuthorityConfiguration!.Decode();

        #endregion
    }
}