using System;

using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    public override KernelState Handle(KernelSession session, QueryPcdResponse signal) => throw new NotImplementedException();
}