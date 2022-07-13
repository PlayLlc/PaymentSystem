using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Reader.Database;

public interface IReaderRepository
{
    #region Instance Members

    public PrimitiveValue[] GetReaderConfiguration(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification);

    #endregion
}

public partial class ReaderDatabase
{
    #region Instance Values

    private readonly Dictionary<KernelId, PrimitiveValue[]> _KernelConfigurations;
    private readonly Dictionary<CombinationCompositeKey, PrimitiveValue[]> _TransactionProfiles;
    private readonly Dictionary<LanguagePreference, DisplayMessages> _DisplayMessages;
    private readonly Dictionary<KernelId, CertificateAuthorityDataset[]> _CertificateAuthorityDatasets;
    private readonly TransactionType[] _SupportedTransactionTypes;
    private readonly PcdProtocolConfiguration _PcdProtocolConfiguration;
    private readonly PrimitiveValue[] _ReaderConfiguration;

    #endregion

    #region Constructor

    public ReaderDatabase(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification,
        LanguagePreference languagePreference, IReaderRepository readerRepository, ICertificateAuthorityDatasetRepository certificateAuthorityDatasetRepository,
        IDisplayMessageRepository displayMessageRepository, IPcdProtocolRepository pcdProtocolRepository, IKernelRepository kernelRepository,
        ITransactionProfileRepository transactionProfileRepository)
    {
        _TransactionValues = new Dictionary<Tag, PrimitiveValue>();
        _ReaderConfiguration = readerRepository.GetReaderConfiguration(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);

        _KernelConfigurations = kernelRepository.GetKernelConfigurations(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
        _CertificateAuthorityDatasets =
            certificateAuthorityDatasetRepository.GetCertificateAuthorityDatasets(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
        _PcdProtocolConfiguration = pcdProtocolRepository.Get(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
        _TransactionProfiles = transactionProfileRepository.GetTransactionProfiles(issuerIdentificationNumber, merchantIdentifier, terminalIdentification);
        _SupportedTransactionTypes = _TransactionProfiles.Keys.Select(a => a.GetTransactionType()).Distinct().ToArray();
        _DisplayMessages =
            displayMessageRepository.GetDisplayMessages(issuerIdentificationNumber, merchantIdentifier, terminalIdentification, languagePreference);
    }

    #endregion

    #region Instance Members

    public TransactionType[] GetSupportedTransactionTypes() => _SupportedTransactionTypes;
    public PrimitiveValue[] GetKernelConfiguration(KernelId kernelId) => _KernelConfigurations[kernelId];
    public CertificateAuthorityDataset[] GetCertificateAuthorityDatasets(KernelId kernelId) => _CertificateAuthorityDatasets[kernelId];
    public PcdProtocolConfiguration GetPcdProtocolConfiguration() => _PcdProtocolConfiguration;
    public PrimitiveValue[] GetTransactionProfile(CombinationCompositeKey key) => _TransactionProfiles[key];
    public DisplayMessages GetDisplayMessages(LanguagePreference languagePreference) => _DisplayMessages[languagePreference];

    #endregion
}