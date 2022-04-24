using Play.Emv.Identifiers;

namespace Play.Emv.Terminal.StateMachine;

public interface IGetTerminalState
{
    #region Instance Members

    public TerminalState GetKernelState(StateId stateId);

    #endregion
}