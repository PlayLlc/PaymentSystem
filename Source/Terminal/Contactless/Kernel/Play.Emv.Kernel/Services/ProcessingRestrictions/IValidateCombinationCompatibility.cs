using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services;

public interface IValidateCombinationCompatibility
{
    #region Instance Members

    public void Process(KernelDatabase database);

    #endregion
}