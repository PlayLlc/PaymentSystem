using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholder
{
    public CvmResults Process();
}