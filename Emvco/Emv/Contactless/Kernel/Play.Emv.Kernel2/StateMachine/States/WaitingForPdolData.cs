using System;

using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

internal class WaitingForPdolData : KernelState
{
    #region Static Metadata

    public static readonly KernelStateId KernelStateId = new(nameof(WaitingForPdolData));

    #endregion

    public override KernelStateId GetKernelStateId() => KernelStateId;

    #region ACT

    public override KernelState Handle(ActivateKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion

    #region CLEAN

    public override KernelState Handle(CleanKernelRequest signal) => throw new NotImplementedException();

    #endregion

    #region STOP

    public override KernelState Handle(StopKernelRequest signal) => throw new NotImplementedException();

    #endregion

    #region RAPDU

    public override KernelState Handle(QueryPcdResponse signal) => throw new NotImplementedException();

    #endregion

    #region DET

    public override KernelState Handle(UpdateKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(QueryTerminalResponse signal) => throw new NotImplementedException();

    #endregion

    public override KernelState Handle(QueryKernelRequest signal) => throw new NotImplementedException();
}