using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Services.PrepareGenerateAc;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPutDataResponseBeforeGenerateAc : KernelState
{
    #region Instance Values

    private readonly PrepareGenerateAcService _PrepareGenAcService;

    #endregion

    #region Constructor

    public WaitingForPutDataResponseBeforeGenerateAc(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint,
        IManageTornTransactions tornTransactionLog, IGetKernelState kernelStateResolver, IHandlePcdRequests pcdEndpoint, IHandleDisplayRequests displayEndpoint,
        PrepareGenerateAcService prepareGenAcService) : base(database, dataExchangeKernelService, kernelEndpoint, tornTransactionLog, kernelStateResolver,
        pcdEndpoint, displayEndpoint)
    {
        _PrepareGenAcService = prepareGenAcService;
    }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForPutDataResponseBeforeGenerateAc));

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