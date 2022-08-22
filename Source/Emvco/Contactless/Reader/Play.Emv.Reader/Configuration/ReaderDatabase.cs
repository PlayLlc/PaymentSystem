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
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Configuration;
using Play.Emv.Selection.Contracts;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Reader;

public partial class ReaderDatabase
{
    #region Instance Values

    private readonly KernelPersistentConfigurations _KernelPersistentConfigurations;
    private readonly TransactionProfiles _TransactionProfiles;
    private readonly CertificateAuthorityDatasets _CertificateAuthorityDatasets;
    private readonly ReaderPersistentConfiguration _ReaderConfiguration;

    #endregion

    #region Constructor

    public ReaderDatabase(
        KernelPersistentConfigurations kernelPersistentConfigurations, TransactionProfiles transactionProfiles,
        CertificateAuthorityDatasets certificateAuthorityDatasets, ReaderPersistentConfiguration readerConfiguration)
    {
        _TransactionDatabase = new Dictionary<Tag, PrimitiveValue?>();
        _KernelPersistentConfigurations = kernelPersistentConfigurations;
        _TransactionProfiles = transactionProfiles;
        _CertificateAuthorityDatasets = certificateAuthorityDatasets;
        _ReaderConfiguration = readerConfiguration;
    }

    #endregion

    #region Instance Members

    public PrimitiveValue[] GetKernelValues(CombinationCompositeKey key)
    {
        List<PrimitiveValue> result = new();
        result.AddRange(GetKernelConfiguration(key.GetKernelId()));
        result.AddRange(_ReaderConfiguration.GetPersistentConfigurations());
        result.AddRange(_TransactionDatabase.Values.OfType<PrimitiveValue>());
        result.AddRange(_TransactionProfiles.GetTransactionProfile(key)!.AsPrimitiveValues());

        return result.ToArray();
    }

    public TransactionType[] GetSupportedTransactionTypes() => _TransactionProfiles.GetSupportedTransactionTypes();
    public bool IsTransactionSupported(TransactionType transactionType) => _TransactionProfiles.IsTransactionSupported(transactionType);
    public PrimitiveValue[] GetKernelConfiguration(KernelId kernelId) => _KernelPersistentConfigurations.Get(kernelId).ToArray();
    public CertificateAuthorityDataset[] GetCertificateAuthorityDatasets() => _CertificateAuthorityDatasets.GetCertificateAuthorityDatasets();
    public void PurgeRevokedCertificates() => _CertificateAuthorityDatasets.PurgeRevokedCertificates();
    public void PurgeRevokedCertificates(RegisteredApplicationProviderIndicator rid) => _CertificateAuthorityDatasets.PurgeRevokedCertificates();
    public TransactionProfile? GetTransactionProfile(CombinationCompositeKey key) => _TransactionProfiles.GetTransactionProfile(key);

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
                new InvalidOperationException($"A command to initialize the Kernel Database was invoked but the {nameof(ReaderDatabase)} is already active"));
        }

        _TransactionSessionId = kernelSessionId;
        Seed();
    }

    private void Seed()
    {
        foreach (PrimitiveValue value in _ReaderConfiguration.GetPersistentConfigurations())
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