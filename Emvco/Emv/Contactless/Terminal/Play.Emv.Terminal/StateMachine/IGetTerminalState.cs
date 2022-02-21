using Play.Emv.Sessions;

namespace Play.Emv.Terminal.StateMachine;

public interface IGetTerminalState
{
    public TerminalState GetKernelState(StateId stateId);
}