using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine;

public partial class Idle : KernelState
{
    #region Instance Members

    #region CLEAN

    public override KernelState Handle(CleanKernelRequest signal)
    {
        _KernelCleaner.Clean();

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #endregion

    #endregion
}