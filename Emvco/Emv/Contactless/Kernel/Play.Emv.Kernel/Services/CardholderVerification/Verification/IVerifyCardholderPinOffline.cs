using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderPinOffline
{
    public CvmCode Process();
}