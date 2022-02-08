using System;

using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public class WaitingForCccResponse1 : KernelState
{
    #region Static Metadata

    public static readonly KernelStateId KernelStateId = new(nameof(WaitingForCccResponse1));

    #endregion

    public override KernelStateId GetKernelStateId() => KernelStateId;
    public override KernelState Handle(ActivateKernelRequest signal) => throw new RequestOutOfSyncException(signal, ChannelType.Kernel);
    public override KernelState Handle(CleanKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(QueryKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(StopKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(UpdateKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(QueryPcdResponse signal) => throw new NotImplementedException();
    public override KernelState Handle(QueryTerminalResponse signal) => throw new NotImplementedException();
}