using Play.Emv.Identifiers;

namespace Play.Emv.Kernel.State;

public interface IGetKernelState
{
    #region Instance Members

    public KernelState GetKernelState(StateId stateId);

    #endregion
}