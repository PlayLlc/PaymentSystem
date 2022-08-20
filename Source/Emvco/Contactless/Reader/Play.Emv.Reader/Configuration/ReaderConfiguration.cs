using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Display.Configuration;
using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Reader;

public partial class ReaderConfiguration
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

    public ReaderConfiguration(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification,
        LanguagePreference languagePreference, IReaderRepository readerRepository, ICertificateAuthorityDatasetRepository certificateAuthorityDatasetRepository,
        IDisplayMessageRepository displayMessageRepository, IPcdProtocolRepository pcdProtocolRepository, IKernelRepository kernelRepository,
        ITransactionProfileRepository transactionProfileRepository)
    {
        _TransactionDatabase = new Dictionary<Tag, PrimitiveValue?>();
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

    public PrimitiveValue[] GetKernelValues(CombinationCompositeKey key)
    {
        List<PrimitiveValue> result = new();
        result.AddRange(_TransactionDatabase.Values.OfType<PrimitiveValue>());
        result.AddRange(_KernelConfigurations[key.GetKernelId()]);
        result.AddRange(_TransactionProfiles[key]);

        return result.ToArray();
    }

    public TransactionType[] GetSupportedTransactionTypes() =>
        _SupportedTransactionTypes; // this should be on the terminal so the POS can list the transaction types

    public PrimitiveValue[] GetKernelConfiguration(KernelId kernelId) => _KernelConfigurations[kernelId];
    public CertificateAuthorityDataset[] GetCertificateAuthorityDatasets(KernelId kernelId) => _CertificateAuthorityDatasets[kernelId];
    public PcdProtocolConfiguration GetPcdProtocolConfiguration() => _PcdProtocolConfiguration;
    public PrimitiveValue[] GetTransactionProfile(CombinationCompositeKey key) => _TransactionProfiles[key];
    public DisplayMessages GetDisplayMessages(LanguagePreference languagePreference) => _DisplayMessages[languagePreference];

    #endregion

    #region Lifetime Management

    /// <summary>
    ///     Resets the transient values in the database to their default values. The persistent values
    ///     will remain unchanged during the database lifetime
    /// </summary>
    public virtual void Deactivate()
    {
        _TransactionDatabase.Clear();
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="TerminalException"></exception>
    public virtual void Activate(TransactionSessionId kernelSessionId)
    {
        if (IsActive())
        {
            throw new TerminalException(
                new InvalidOperationException(
                    $"A command to initialize the Kernel Database was invoked but the {nameof(ReaderConfiguration)} is already active"));
        }

        _TransactionSessionId = kernelSessionId;
        Seed();
    }

    private void Seed()
    {
        foreach (PrimitiveValue value in _ReaderConfiguration)
            _TransactionDatabase.Add(value.GetTag(), value);
    }

    public bool IsActive() => _TransactionSessionId != null;

    public bool IsActive(out TransactionSessionId? result)
    {
        if (!IsActive())
        {
            result = null;

            return false;
        }

        result = _TransactionSessionId;

        return true;
    }

    #endregion
}