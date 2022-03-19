using Play.Emv.Ber;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholderSignature
{
    public CvmCode Process();
}