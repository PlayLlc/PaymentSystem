using Play.Emv.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse1
{
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        // CHECK: The S9 algorithm only specified to recursively set the state on a STOP signal. Check the specs to make sure that's valid with all STOP signal rules
        return _KernelStateResolver.GetKernelState(StateId);
    }
}