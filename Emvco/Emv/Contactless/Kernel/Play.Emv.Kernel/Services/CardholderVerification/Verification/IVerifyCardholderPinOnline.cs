using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IVerifyCardholderPinOnline
{
    public CvmCode Process();
}