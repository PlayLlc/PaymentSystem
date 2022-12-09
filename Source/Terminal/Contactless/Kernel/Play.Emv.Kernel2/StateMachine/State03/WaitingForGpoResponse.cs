﻿using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    #region Instance Values

    private readonly S3R1 _S3R1;
    private readonly IGenerateUnpredictableNumber _UnpredictableNumberGenerator;

    #endregion

    #region Constructor

    public WaitingForGpoResponse(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IEndpointClient endpointClient,
        IManageTornTransactions tornTransactionLog, IGetKernelState kernelStateResolver, IGenerateUnpredictableNumber unpredictableNumberGenerator, S3R1 s3R1) :
        base(database, dataExchangeKernelService, tornTransactionLog, kernelStateResolver, endpointClient)
    {
        _S3R1 = s3R1;
        _UnpredictableNumberGenerator = unpredictableNumberGenerator;
    }

    #endregion

    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForGpoResponse));

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    #region ACT

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

    #endregion

    #region CLEAN

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(CleanKernelRequest signal) => throw new RequestOutOfSyncException(signal, KernelChannel.Id);

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