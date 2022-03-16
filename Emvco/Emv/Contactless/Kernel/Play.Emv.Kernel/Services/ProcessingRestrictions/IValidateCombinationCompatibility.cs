using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services._TempNew;

public interface IValidateCombinationCompatibility
{
    public void Process(KernelDatabase database);
}