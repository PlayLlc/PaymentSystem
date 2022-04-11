using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Services.CommonStateLogic;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForEmvReadRecordResponse : KernelState
{
    #region Constructor

    public WaitingForEmvReadRecordResponse(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint,
        IManageTornTransactions tornTransactionManager, IGetKernelState kernelStateResolver, IHandlePcdRequests pcdEndpoint,
        IHandleDisplayRequests displayEndpoint, IHandleTerminalRequests terminalEndpoint, ICleanTornTransactions kernelCleaner, S456 s456) :
        base(database, dataExchangeKernelService, kernelEndpoint, tornTransactionManager, kernelStateResolver, pcdEndpoint, displayEndpoint)
    {
        _TerminalEndpoint = terminalEndpoint;
        _KernelCleaner = kernelCleaner;
        _S456 = s456;
    }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForEmvReadRecordResponse));

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    #region ACT

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region CLEAN

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(CleanKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #endregion

    #region Instance Values

    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly ICleanTornTransactions _KernelCleaner;

    #endregion

    #region DET

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion
}