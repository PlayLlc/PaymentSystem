using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services._CardholderVerification;

public interface ISelectCardholderVerificationMethod
{
    public void Process(KernelDatabase database);
}