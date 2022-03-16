using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.ProcessingRestrictions;

public interface IValidateCombinationCompatibility
{
    public void Process(KernelDatabase database);
}