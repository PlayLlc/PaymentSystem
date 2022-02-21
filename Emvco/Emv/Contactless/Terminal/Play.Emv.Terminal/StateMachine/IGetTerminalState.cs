using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Sessions;

namespace Play.Emv.Terminal.StateMachine;

public interface IGetTerminalState
{
    public TerminalState GetKernelState(StateId stateId);
}