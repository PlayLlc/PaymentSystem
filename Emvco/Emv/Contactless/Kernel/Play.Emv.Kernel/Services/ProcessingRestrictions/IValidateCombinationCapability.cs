using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.ProcessingRestrictions;

public interface IValidateCombinationCapability
{
    public void Process(KernelDatabase database);
}