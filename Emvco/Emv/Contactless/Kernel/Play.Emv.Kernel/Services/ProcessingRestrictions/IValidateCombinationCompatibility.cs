using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.ProcessingRestrictions;

public interface IValidateCombinationCompatibility
{
    #region Instance Members

    public void Process(KernelDatabase database);

    #endregion
}