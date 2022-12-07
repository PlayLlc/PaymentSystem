using System.Text.Json.Serialization;

using Play.Ber.DataObjects;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Configuration;
using Play.Emv.Display.Configuration;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader;
using Play.Emv.Reader.Configuration;
using Play.Emv.Selection.Configuration;

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

        [JsonPropertyName(nameof(ReaderPersistentConfiguration))]
        public ReaderPersistentConfigurationDto? ReaderPersistentConfiguration { get; set; }

        #endregion

        #region Instance Members

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        public TerminalConfiguration GetTerminalConfiguration() => TerminalConfiguration!.Decode();

        public KernelPersistentConfigurations GetKernelPersistent(IResolveKnownObjectsAtRuntime runtimeCodec)
        {
            return new KernelPersistentConfigurations(KernelPersistentConfigurations.Select(a => a.Decode(runtimeCodec)).ToArray());
        }

        public ReaderPersistentConfiguration GetReaderPersistentConfiguration(IResolveKnownObjectsAtRuntime runtimeCodec) =>
            ReaderPersistentConfiguration!.Decode(runtimeCodec);

        public DisplayConfigurations GetDisplayConfiguration() => DisplayConfiguration!.Decode();
        public PcdConfiguration GetPcdConfiguration() => ProximityCouplingDeviceConfiguration!.Decode();
        public CertificateAuthorityDatasets GetCertificateAuthorityDatasets() => new(CertificateAuthorityConfiguration!.Decode());

        public TransactionProfiles GetTransactionProfiles()
        {
            return new TransactionProfiles(TransactionProfiles.Select(a => a.Decode()).ToArray());
        }

        #endregion
    }
}