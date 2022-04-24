﻿using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Services.BalanceReading;
using Play.Emv.Kernel2.Services.PrepareGenerateAc;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForRecoverAcResponse : KernelState
{
    #region Instance Values

    private readonly S910 _S910;
    private readonly PrepareGenerateAcService _PrepareApplicationCryptogramService;
    private readonly OfflineBalanceReader _OfflineBalanceReader;

    #endregion

    #region Constructor

    public WaitingForRecoverAcResponse(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint,
        IManageTornTransactions tornTransactionLog, IGetKernelState kernelStateResolver, IHandlePcdRequests pcdEndpoint,
        IHandleDisplayRequests displayEndpoint, IAuthenticateTransactionSession transactionAuthenticator) : base(database,
        dataExchangeKernelService, kernelEndpoint, tornTransactionLog, kernelStateResolver, pcdEndpoint, displayEndpoint)
    {
        _S910 = new S910(database, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint, transactionAuthenticator,
            displayEndpoint);
        _OfflineBalanceReader =
            new OfflineBalanceReader(database, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint);

        _PrepareApplicationCryptogramService =
            new PrepareGenerateAcService(database, dataExchangeKernelService, kernelStateResolver, pcdEndpoint, kernelEndpoint);
    }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForRecoverAcResponse));

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    #region ACT

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, KernelChannel.Id);

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
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    #endregion
}