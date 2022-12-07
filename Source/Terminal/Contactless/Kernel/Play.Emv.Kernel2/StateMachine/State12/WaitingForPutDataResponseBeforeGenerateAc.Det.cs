using Play.Emv.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPutDataResponseBeforeGenerateAc
{
    /// <exception cref="RequestOutOfSyncException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal) => _KernelStateResolver.GetKernelState(StateId);
}