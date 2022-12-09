﻿using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

/// <remarks>
///     The first state entered after wakeup
/// </remarks>
public partial class Idle : KernelState
{
    #region Instance Values

    private readonly IGenerateUnpredictableNumber _UnpredictableNumberGenerator;

    #endregion

    #region Constructor

    public Idle(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IEndpointClient endpointClient,
        IManageTornTransactions tornTransactionLog, IGetKernelState kernelStateResolver, IGenerateUnpredictableNumber unpredictableNumberGenerator) : base(
        database, dataExchangeKernelService, tornTransactionLog, kernelStateResolver, endpointClient)
    {
        _UnpredictableNumberGenerator = unpredictableNumberGenerator;
    }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(Idle));

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
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

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
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    #endregion
}