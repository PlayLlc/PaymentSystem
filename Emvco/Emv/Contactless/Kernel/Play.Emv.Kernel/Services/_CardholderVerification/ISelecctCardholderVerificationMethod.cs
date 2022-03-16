using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services._TempNew;

public interface ISelecctCardholderVerificationMethod
{
    public void Process(KernelDatabase database);
}