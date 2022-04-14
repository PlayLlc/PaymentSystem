using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse2 : KernelState
{
    #region Instance Values

    private readonly IReadOfflineBalance _BalanceReader;
    private readonly IAuthenticateTransactionSession _AuthenticationService;
    private readonly ResponseHandler _ResponseHandler;
    private readonly AuthHandler _AuthHandler;

    #endregion

    #region Constructor

    public WaitingForGenerateAcResponse2(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint,
        IManageTornTransactions tornTransactionLog, IGetKernelState kernelStateResolver, IHandlePcdRequests pcdEndpoint,
        IHandleDisplayRequests displayEndpoint, IAuthenticateTransactionSession authenticationService,
        IReadOfflineBalance offlineBalanceReader) : base(database, dataExchangeKernelService, kernelEndpoint, tornTransactionLog,
        kernelStateResolver, pcdEndpoint, displayEndpoint)
    {
        _AuthenticationService = authenticationService;
        _ResponseHandler = new ResponseHandler(database, _DataExchangeKernelService, kernelEndpoint, pcdEndpoint);
        _AuthHandler = new AuthHandler(database, _ResponseHandler, authenticationService);
        _BalanceReader = offlineBalanceReader;
    }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForGenerateAcResponse2));

    #endregion

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

    #region DET

    // BUG: Need to make sure you're properly implementing each DEK handler for each state
    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion
}