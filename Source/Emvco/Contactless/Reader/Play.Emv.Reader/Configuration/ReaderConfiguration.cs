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

namespace Play.Emv.Reader;

public partial class ReaderConfiguration
{
    #region Instance Values

    private readonly KernelConfigurations _KernelConfigurations;
    private readonly TransactionProfileConfigurations _TransactionProfileConfigurations;
    private readonly Dictionary<KernelId, CertificateAuthorityDataset[]> _CertificateAuthorityDatasets;
    private readonly PrimitiveValue[] _ReaderConfiguration;

    #endregion

    #region Instance Members

    public PrimitiveValue[] GetKernelValues(CombinationCompositeKey key)
    {
        List<PrimitiveValue> result = new();
        result.AddRange(_ReaderConfiguration);
        result.AddRange(_TransactionDatabase.Values.OfType<PrimitiveValue>());
        result.AddRange(GetKernelConfiguration(key.GetKernelId()));
        result.AddRange(_TransactionProfileConfigurations.GetTransactionProfile(key)!.AsPrimitiveValues());

        return result.ToArray();
    }

    public TransactionType[] GetSupportedTransactionTypes() => _TransactionProfileConfigurations.GetSupportedTransactionTypes();
    public bool IsTransactionSupported(TransactionType transactionType) => _TransactionProfileConfigurations.IsTransactionSupported(transactionType);
    public PrimitiveValue[] GetKernelConfiguration(KernelId kernelId) => _KernelConfigurations.Get(kernelId);
    public CertificateAuthorityDataset[] GetCertificateAuthorityDatasets(KernelId kernelId) => _CertificateAuthorityDatasets[kernelId];
    public TransactionProfile? GetTransactionProfile(CombinationCompositeKey key) => _TransactionProfileConfigurations.GetTransactionProfile(key);

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