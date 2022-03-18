using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Selection;

public interface ISelectCardholderVerificationMethod
{
    public void Process(KernelDatabase database);
}