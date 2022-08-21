using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse1 : KernelState
{
    #region Instance Values

    private readonly S910 _S910;
    private readonly IReadOfflineBalance _BalanceReader;

    #endregion

    #region Constructor

    public WaitingForGenerateAcResponse1(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IEndpointClient endpointClient,
        IManageTornTransactions tornTransactionLog, IGetKernelState kernelStateResolver, IReadOfflineBalance balanceReader, S910 s910) : base(database,
        dataExchangeKernelService, tornTransactionLog, kernelStateResolver, endpointClient)
    {
        _S910 = s910;
        _BalanceReader = balanceReader;
    }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForGenerateAcResponse1));

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    #region ACT

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    #endregion

    #region CLEAN

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(CleanKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    #endregion

    #endregion

    #region DET

    // BUG: Need to make sure you're properly implementing each DEK handler for each state

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    #endregion
}