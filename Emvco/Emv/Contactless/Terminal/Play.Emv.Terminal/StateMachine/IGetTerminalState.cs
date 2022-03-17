using Play.Emv.Identifiers;

namespace Play.Emv.Terminal.StateMachine;

public interface IGetTerminalState
{
    public TerminalState GetKernelState(StateId stateId);
}