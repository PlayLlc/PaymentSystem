using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Exceptions;
using Play.Emv.Database;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Exceptions;
using Play.Emv.Outcomes;
using Play.Emv.Security.Certificates;
using Play.Emv.Terminal.Contracts;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel.Databases;

public abstract partial class KernelDatabase : IManageKernelDatabaseLifetime, IQueryKernelDatabase, IQueryTlvDatabase
{
    #region Instance Values

    protected readonly ICertificateDatabase _CertificateDatabase;
    protected readonly ITlvDatabase _TlvDatabase;
    protected KernelSessionId? _KernelSessionId;
    protected IHandleTerminalRequests _TerminalEndpoint;
    protected OutcomeParameterSet.Builder _OutcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
    protected UserInterfaceRequestData.Builder _UserInterfaceRequestDataBuilder = UserInterfaceRequestData.GetBuilder();
    protected ErrorIndication.Builder _ErrorIndicationBuilder = ErrorIndication.GetBuilder();
    protected TerminalVerificationResults.Builder _TerminalVerificationResultBuilder = TerminalVerificationResults.GetBuilder();

    #endregion

    #region Constructor

    protected KernelDatabase(
        IHandleTerminalRequests terminalEndpoint,
        ITlvDatabase tlvDatabase,
        ICertificateDatabase certificateDatabase)
    {
        _TerminalEndpoint = terminalEndpoint;
        _TlvDatabase = tlvDatabase;
        _CertificateDatabase = certificateDatabase;
    }

    #endregion

    #region Instance Members

    public abstract KernelConfiguration GetKernelConfiguration(); 

    /// <summary>
    ///     Activate
    /// </summary>
    /// <param name="kernelSessionId"></param>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public virtual void Activate(KernelSessionId kernelSessionId)
    {
        if (IsActive())
        {
            throw new
                InvalidOperationException($"A command to initialize the Kernel Database was invoked but the {nameof(KernelDatabase)} is already active");
        }

        _KernelSessionId = kernelSessionId;
    }

    /// <summary>
    ///     Resets the transient values in the database to their default values. The persistent values
    ///     will remain unchanged during the database lifetime
    /// </summary>
    public virtual void Deactivate()
    {
        _TlvDatabase.Clear();
        _CertificateDatabase.PurgeRevokedCertificates();
    }  

    public bool TryGetKernelSessionId(out KernelSessionId? result)
    {
        if (_KernelSessionId == null)
        {
            result = null;

            return false;
        }

        result = _KernelSessionId;

        return true;
    } 

    protected bool IsActive() => _KernelSessionId != null;

    #endregion


    

   

}