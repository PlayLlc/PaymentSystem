using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForCccResponse2 : KernelState
{
    #region Constructor

    public WaitingForCccResponse2(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IEndpointClient endpointClient,
        IManageTornTransactions tornTransactionLog, IGetKernelState kernelStateResolver) : base(database, dataExchangeKernelService, tornTransactionLog,
        kernelStateResolver, endpointClient)
    { }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForCccResponse2));

    #endregion

    public override StateId GetStateId() => StateId;

    #region ACT

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    #endregion

    #region CLEAN

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(CleanKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    #endregion

    #region DET

    // BUG: Need to make sure you're properly implementing each DEK handler for each state

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    #endregion
}