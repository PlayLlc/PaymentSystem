using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderPinOnline
{
    public CvmCode Process(KernelDatabase database);
}