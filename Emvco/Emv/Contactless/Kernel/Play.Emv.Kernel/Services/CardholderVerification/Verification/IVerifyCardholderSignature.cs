using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderSignature
{
    public CvmCode Process();
}