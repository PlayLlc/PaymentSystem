﻿using Play.Emv.Display.Contracts;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

/// <remarks>
///     The first state entered after wakeup
/// </remarks>
public partial class Idle : KernelState
{
    #region Constructor

    public Idle(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint,
        IManageTornTransactions tornTransactionManager, IGetKernelState kernelStateResolver, IHandlePcdRequests pcdEndpoint,
        IHandleDisplayRequests displayEndpoint, IGenerateUnpredictableNumber unpredictableNumberGenerator) : base(database,
        dataExchangeKernelService, kernelEndpoint, tornTransactionManager, kernelStateResolver, pcdEndpoint, displayEndpoint)
    {
        _UnpredictableNumberGenerator = unpredictableNumberGenerator;
    }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(Idle));

    #endregion

    #region Instance Values

    private readonly IGenerateUnpredictableNumber _UnpredictableNumberGenerator;

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    #region RAPDU

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #endregion

    #region DET

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal) =>
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

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion
}