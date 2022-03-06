using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Templates.Exceptions;
using Play.Emv.Templates.FileControlInformation;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

/// <remarks>
///     The first state entered after wakeup
/// </remarks>
public partial class Idle : KernelState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(Idle));

    #endregion

    #region Instance Values

    private readonly IKernelEndpoint _KernelEndpoint;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly ICleanTornTransactions _KernelCleaner;

    #endregion

    #region Constructor

    public Idle(
        ICleanTornTransactions kernelCleaner,
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchange,
        IGetKernelState kernelStateResolver,
        IKernelEndpoint kernelEndpoint,
        IHandleTerminalRequests terminalEndpoint,
        IHandlePcdRequests pcdEndpoint) : base(kernelDatabase, dataExchange)
    {
        _KernelCleaner = kernelCleaner;
        _KernelStateResolver = kernelStateResolver;
        _KernelEndpoint = kernelEndpoint;
        _TerminalEndpoint = terminalEndpoint;
        _PcdEndpoint = pcdEndpoint;
    }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    #endregion
}