using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderPinOnline
{
    public CvmCode Process();
}