using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services._TempNew;

public interface IValidateCombinationCapability
{
    public void Process(KernelDatabase database);
}