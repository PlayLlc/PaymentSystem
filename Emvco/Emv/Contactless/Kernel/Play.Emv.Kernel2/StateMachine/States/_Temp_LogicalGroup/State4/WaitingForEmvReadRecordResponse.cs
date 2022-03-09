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

namespace Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup.State4;

public partial class WaitingForEmvReadRecordResponse : KernelState
{
    #region Static Metadata

    public static readonly StateId StateId = new(nameof(WaitingForEmvReadRecordResponse));

    #endregion

    #region Constructor

    public WaitingForEmvReadRecordResponse(
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchange,
        IKernelEndpoint kernelEndpoint) : base(kernelDatabase, dataExchange, kernelEndpoint)
    { }

    #endregion

    #region Instance Members

    public override StateId GetStateId() => StateId;

    #region ACT

    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, ActivateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region CLEAN

    public override KernelState Handle(CleanKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #endregion
}