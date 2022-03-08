﻿using System;

using Play.Emv.Exceptions;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

internal class WaitingForExchangeRelayResistanceDataResponse : KernelState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForExchangeRelayResistanceDataResponse));

    #endregion

    #region Constructor

    public WaitingForExchangeRelayResistanceDataResponse(
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchange,
        IKernelEndpoint kernelEndpoint) : base(kernelDatabase, dataExchange, kernelEndpoint)
    { }

    #endregion

    public override StateId GetStateId() => StateId;

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    public override KernelState Handle(CleanKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, StopKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal) => throw new NotImplementedException();
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal) => throw new NotImplementedException();
}