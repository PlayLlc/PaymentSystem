using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine.States;

internal class WaitingForPdolData : KernelState
{
    #region Static Metadata

    public static readonly KernelStateId KernelStateId = new(2);

    #endregion

    public override KernelStateId GetKernelStateId() => KernelStateId;
    public override KernelState Handle(ActivateKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(CleanKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(QueryKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(StopKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(UpdateKernelRequest signal) => throw new NotImplementedException();
    public override KernelState Handle(QueryPcdResponse signal) => throw new NotImplementedException();
    public override KernelState Handle(QueryTerminalResponse signal) => throw new NotImplementedException();
}