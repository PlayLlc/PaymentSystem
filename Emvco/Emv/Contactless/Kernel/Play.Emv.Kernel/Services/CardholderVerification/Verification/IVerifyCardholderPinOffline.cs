using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IVerifyCardholderPinOffline
{
    public CvmCode Process();
}