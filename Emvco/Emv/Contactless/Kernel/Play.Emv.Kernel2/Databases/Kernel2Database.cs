using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Databases.Certificates;
using Play.Emv.Kernel.Databases.Tlv;
using Play.Emv.Kernel2.Configuration;
using Play.Emv.Terminal.Contracts;

using ITlvDatabase = Play.Emv.Kernel2.Databases._Temp.ITlvDatabase;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2Database : KernelDatabase
{
    #region Instance Values

    private readonly Kernel2Configuration _Kernel2Configuration;

    #endregion

    #region Constructor

    public Kernel2Database(
        Kernel2Configuration kernel2Configuration,
        IHandleTerminalRequests terminalEndpoint,
        ITlvDatabase tlvDatabase,
        ICertificateDatabase certificateDatabase) : base(terminalEndpoint, tlvDatabase, certificateDatabase)
    {
        _Kernel2Configuration = kernel2Configuration;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Activate
    /// </summary>
    /// <param name="kernelSessionId"></param>
    /// <param name="transaction"></param>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exceptions.TerminalDataException"></exception>
    public override void Activate(KernelSessionId kernelSessionId)
    {
        if (IsActive())
        {
            throw new
                InvalidOperationException($"A command to initialize the Kernel Database was invoked but the {nameof(KernelDatabase)} is already active");
        }

        _KernelSessionId = kernelSessionId; 
    }

    public override Kernel2Configuration GetKernelConfiguration() => _Kernel2Configuration;

    /// <summary>
    ///     Returns TRUE if tag T is defined in the data dictionary of the Kernel applicable for the Implementation Option
    /// </summary>
    /// <param name="tag"></param>
    public override bool IsKnown(Tag tag) => KnownObjects.Exists(tag);

    #endregion
}