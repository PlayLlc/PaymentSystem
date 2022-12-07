using Play.Emv.Identifiers;

namespace Play.Emv.Kernel.State;

public interface IGetKernelStateId
{
    #region Instance Members

    public StateId GetStateId();

    #endregion
}