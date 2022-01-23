﻿using System;

using Play.Ber.DataObjects;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Security.Certificates;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Transactions;
using Play.Icc.Emv;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Databases;

public abstract class KernelDatabase : IActivateKernelDatabase, IDeactivateKernelDatabase, IQueryKernelDatabase, IQueryTlvDatabase
{
    #region Instance Values

    protected readonly ICertificateAuthorityDatabase _CertificateAuthorityDatabase;
    protected readonly ITlvDatabase _TlvDatabase;
    protected IHandleTerminalRequests _TerminalEndpoint;
    protected KernelSession? _KernelSession;

    #endregion

    #region Constructor

    protected KernelDatabase(
        IHandleTerminalRequests terminalEndpoint,
        ITlvDatabase tlvDatabase,
        ICertificateAuthorityDatabase certificateAuthorityDatabase)
    {
        _TerminalEndpoint = terminalEndpoint;
        _TlvDatabase = tlvDatabase;
        _CertificateAuthorityDatabase = certificateAuthorityDatabase;
    }

    #endregion

    #region Instance Members

    public DataExchangeKernelService GetDataExchanger()
    {
        if (IsActive())
        {
            throw new
                InvalidOperationException($"Could not retrieve the {nameof(DataExchangeKernelService)} because the {nameof(KernelDatabase)} is not active");
        }

        return _KernelSession!.GetDataExchangeKernelService();
    }

    public abstract KernelConfiguration GetKernelConfiguration();

    /// <summary>
    ///     Activate
    /// </summary>
    /// <param name="kernelSessionId"></param>
    /// <param name="terminalEndpoint"></param>
    /// <param name="transaction"></param>
    /// <exception cref="BerInternalException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual void Activate(
        KernelSessionId kernelSessionId,
        IHandleTerminalRequests terminalEndpoint,
        ISendTerminalQueryResponse kernelEndpoint,
        Transaction transaction)
    {
        if (IsActive())
        {
            throw new
                InvalidOperationException($"A command to initialize the Kernel Database was invoked but the {nameof(KernelDatabase)} is already active");
        }

        _KernelSession = new KernelSession(kernelSessionId, _TerminalEndpoint, this, kernelEndpoint);
        UpdateRange(transaction.AsTagLengthValueArray());
    }

    /// <summary>
    ///     Resets the transient values in the database to their default values. The persistent values
    ///     will remain unchanged during the database lifetime
    /// </summary>
    public virtual void Deactivate()
    {
        _TlvDatabase.Clear();
        _CertificateAuthorityDatabase.PurgeRevokedCertificates();
        _KernelSession = null;
    }

    /// <summary>
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual TagLengthValue Get(Tag tag)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(Get)} cannot be accessed because {nameof(KernelDatabase)} is not active");
        }

        return _TlvDatabase.Get(tag);
    }

    /// <summary>
    ///     Returns TRUE if tag T is defined in the data dictionary of the Kernel applicable for the Implementation Option
    /// </summary>
    /// <param name="tag"></param>
    public abstract bool IsKnown(Tag tag);

    /// <summary>
    ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
    ///     zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual bool IsPresent(Tag tag)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(IsPresent)} cannot be accessed because {nameof(KernelDatabase)} is not active");
        }

        return _TlvDatabase.IsPresent(tag);
    }

    /// <summary>
    ///     Returns TRUE if:
    ///     • The Database includes a data object with the provided <see cref="Tag" />
    ///     • The length of the data object is non-zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual bool IsPresentAndNotEmpty(Tag tag)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(IsPresentAndNotEmpty)} cannot be accessed because {nameof(KernelDatabase)} is not active");
        }

        return _TlvDatabase.IsPresentAndNotEmpty(tag);
    }

    /// <summary>
    ///     Indicates if the <see cref="CaPublicKeyCertificate" /> is currently valid. If the current date and time
    ///     is before the certificate's active date or after the certificate's expiry date, then the certificate is
    ///     revoked. Certificates can also be revoked by the issuer
    /// </summary>
    public virtual bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(IsRevoked)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        return _CertificateAuthorityDatabase!.IsRevoked(rid, caPublicKeyIndex);
    }

    /// <summary>
    ///     Updates the <see cref="ICertificateAuthorityDatabase" /> by removing any <see cref="CaPublicKeyCertificate" />
    ///     that has expired since the last time they were checked
    /// </summary>
    public virtual void PurgeRevokedCertificates()
    {
        if (!IsActive())
            return;

        _CertificateAuthorityDatabase.PurgeRevokedCertificates();
    }

    /// <summary>
    ///     Attempts to get the <see cref="CaPublicKeyCertificate" /> associated with the
    ///     <param name="rid" />
    ///     and
    ///     <param name="index"></param>
    ///     provided. If the <see cref="CaPublicKeyCertificate" /> is revoked or none
    ///     can be found then the return value will be false
    /// </summary>
    public virtual bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        return _CertificateAuthorityDatabase.TryGet(rid, index, out result);
    }

    /// <summary>
    ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
    ///     <see cref="DatabaseValue" /> in an out parameter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="result"></param>
    public virtual bool TryGet(Tag tag, out TagLengthValue? result)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        return _TlvDatabase.TryGet(tag, out result);
    }

    public virtual KernelSession GetKernelSession()
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(GetKernelSession)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        return _KernelSession!;
    }

    /// <summary>
    ///     If the Kernel Database is active, true is returned and the KernelSessionId is set as the out parameter.
    ///     Otherwise false is returned
    /// </summary>
    public virtual bool TryGetKernelSessionId(out KernelSessionId? result)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(TryGetKernelSessionId)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        result = _KernelSession!.GetKernelSessionId();

        return true;
    }

    /// <summary>
    ///     Updates the database with the
    ///     <param name="value"></param>
    ///     if it is a recognized object and discards the value if
    ///     it is not recognized
    /// </summary>
    /// <param name="value"></param>
    public virtual void Update(TagLengthValue value)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(Update)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        _TlvDatabase.Update(value);
    }

    /// <summary>
    ///     Updates the the database with all recognized
    ///     <param name="values" />
    ///     provided it is not recognized
    /// </summary>
    /// <param name="values"></param>
    public virtual void UpdateRange(TagLengthValue[] values)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(UpdateRange)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        _TlvDatabase.UpdateRange(values);
    }

    public virtual void Initialize(Tag tag)
    {
        if (!IsActive())
        {
            throw new
                InvalidOperationException($"The method {nameof(Initialize)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        _TlvDatabase.Update(new DatabaseValue(tag));
    }

    public void Update(Level1Error value)
    {
        _KernelSession!.Update(value);
    }

    public void Update(Level2Error value)
    {
        _KernelSession!.Update(value);
    }

    public void Update(Level3Error value)
    {
        _KernelSession!.Update(value);
    }

    public void Update(OutcomeParameterSet.Builder value)
    {
        _KernelSession!.Update(value);
    }

    public void Update(UserInterfaceRequestData.Builder value)
    {
        _KernelSession!.Update(value);
    }

    public void Reset(OutcomeParameterSet value)
    {
        _KernelSession!.Reset(value);
    }

    public void Reset(UserInterfaceRequestData value)
    {
        _KernelSession!.Reset(value);
    }

    public void Reset(ErrorIndication value)
    {
        _KernelSession!.Reset(value);
    }

    protected bool IsActive()
    {
        return _KernelSession != null;
    }

    #endregion
}