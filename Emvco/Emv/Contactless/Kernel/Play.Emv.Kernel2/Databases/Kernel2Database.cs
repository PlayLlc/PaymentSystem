using System;
using System.Linq;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel2.Configuration;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;

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
        IKernelCertificateDatabase kernelCertificateDatabase) : base(terminalEndpoint, tlvDatabase, kernelCertificateDatabase)
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
    /// <exception cref="Play.Ber.Exceptions._Temp.BerFormatException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public override void Activate(KernelSessionId kernelSessionId, Transaction transaction)
    {
        if (IsActive())
        {
            throw new InvalidOperationException(
                $"A command to initialize the Kernel Database was invoked but the {nameof(KernelDatabase)} is already active");
        }

        _KernelSessionId = kernelSessionId;
        UpdateRange(transaction.AsTagLengthValueArray().Select(a => new DatabaseValue(a)).ToArray());
    }

    public override Kernel2Configuration GetKernelConfiguration() => _Kernel2Configuration;

    /// <summary>
    ///     Returns TRUE if tag T is defined in the data dictionary of the Kernel applicable for the Implementation Option
    /// </summary>
    /// <param name="tag"></param>
    public override bool IsKnown(Tag tag) => KnownObjects.Exists(tag);

    #endregion
}