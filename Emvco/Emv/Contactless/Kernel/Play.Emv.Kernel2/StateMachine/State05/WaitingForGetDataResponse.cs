using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGetDataResponse : KernelState
{
    #region Instance Values

    private readonly S456 _S456;

    #endregion

    #region Constructor

    public WaitingForGetDataResponse(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint,
        IManageTornTransactions tornTransactionLog, IGetKernelState kernelStateResolver, IHandlePcdRequests pcdEndpoint,
        IHandleDisplayRequests displayEndpoint, S456 s456) : base(database, dataExchangeKernelService, kernelEndpoint, tornTransactionLog,
        kernelStateResolver, pcdEndpoint, displayEndpoint)
    {
        _S456 = s456;
    }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForGetDataResponse));

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

    #region DET

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion
}