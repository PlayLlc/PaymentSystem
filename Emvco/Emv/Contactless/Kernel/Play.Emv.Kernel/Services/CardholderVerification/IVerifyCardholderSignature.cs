using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IVerifyCardholderSignature
{
    public CvmCode Process();
}