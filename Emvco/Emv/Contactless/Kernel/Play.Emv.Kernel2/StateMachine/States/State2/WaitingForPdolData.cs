using System;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv;
using Play.Emv.DataExchange;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Templates.FileControlInformation;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPdolData : KernelState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForPdolData));

    #endregion

    #region Instance Values

    private readonly IKernelEndpoint _KernelEndpoint;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly ICleanTornTransactions _KernelCleaner;

    #endregion

    #region Constructor

    public WaitingForPdolData(
        IKernelEndpoint kernelEndpoint,
        IHandleTerminalRequests terminalEndpoint,
        IHandlePcdRequests pcdEndpoint,
        IGetKernelState kernelStateResolver,
        ICleanTornTransactions kernelCleaner,
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchangeKernelService) : base(kernelDatabase, dataExchangeKernelService)
    {
        _KernelEndpoint = kernelEndpoint;
        _TerminalEndpoint = terminalEndpoint;
        _PcdEndpoint = pcdEndpoint;
        _KernelStateResolver = kernelStateResolver;
        _KernelCleaner = kernelCleaner;
    }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    #endregion
}