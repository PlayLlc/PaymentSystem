using System.Collections.Generic;
using System.Linq;

using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Reader.Database;

internal class ReaderDatabase
{
    #region Instance Values

    private readonly Dictionary<KernelId, KernelConfiguration> _KernelConfigurations;
    private readonly Dictionary<KernelId, PersistentValues> _PersistentKernelValues;
    private readonly Dictionary<CombinationCompositeKey, TransactionProfile> _TransactionProfileConfigurations;
    private readonly CertificateAuthorityDataset[] _CertificateAuthorityDatasets;
    private readonly PcdProtocolConfiguration _PcdProtocolConfiguration;
    private readonly DisplayMessages _DisplayMessages;

    #endregion

    #region Constructor

    public ReaderDatabase(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification,
        LanguagePreference languagePreference,
        ICertificateAuthorityDatasetRepository certificateAuthorityDatasetRepository,
        IDisplayMessageRepository displayMessageRepository,
        IKernelConfigurationRepository kernelConfigurationRepository,
        IPcdProtocolConfigurationRepository pcdProtocolConfigurationRepository,
        IPersistentKernelValuesRepository persistentKernelValuesRepository,
        ITransactionProfileRepository transactionProfileRepository)
    {
        _KernelConfigurations = kernelConfigurationRepository.Get(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
        _PersistentKernelValues =
            persistentKernelValuesRepository.Get(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
        _CertificateAuthorityDatasets =
            certificateAuthorityDatasetRepository.Get(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
        _PcdProtocolConfiguration =
            pcdProtocolConfigurationRepository.Get(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
        _TransactionProfileConfigurations =
            transactionProfileRepository.Get(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
        _DisplayMessages =
            displayMessageRepository.Get(issuerIdentificationNumber, merchantIdentifier, terminalIdentification, languagePreference);
        _KernelConfigurations = kernelConfigurationRepository.Get(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
    }

    #endregion

    #region Instance Members

    public KernelConfiguration GetKernelConfiguration(KernelId kernelId) => _KernelConfigurations[kernelId];
    public PersistentValues GetPersistentKernelValues(KernelId kernelId) => _PersistentKernelValues[kernelId];
    public CertificateAuthorityDataset[] GetCertificateAuthorityDatasets(KernelId kernelId) => _CertificateAuthorityDatasets;
    public PcdProtocolConfiguration GetPcdProtocolConfiguration() => _PcdProtocolConfiguration;
    public TransactionProfile[] GetTransactionProfiles() => _TransactionProfileConfigurations.Values.ToArray();
    public TransactionProfile GetTransactionProfile(CombinationCompositeKey key) => _TransactionProfileConfigurations[key];
    public DisplayMessages GetDisplayMessages(LanguagePreference languagePreference) => _DisplayMessages;

    #endregion
}