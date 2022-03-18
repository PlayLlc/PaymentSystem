using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services.Verification;

public interface IVerifyCardholder
{
    public CvmResults Process();
}