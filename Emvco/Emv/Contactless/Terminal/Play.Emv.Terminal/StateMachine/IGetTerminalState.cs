using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Kernel.State;

namespace Play.Emv.Terminal.StateMachine;

public interface IGetTerminalState
{
    public TerminalState GetKernelState(StateId stateId);
}