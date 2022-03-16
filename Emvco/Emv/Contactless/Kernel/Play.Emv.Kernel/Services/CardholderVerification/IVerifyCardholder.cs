using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IVerifyCardholder
{
    public CvmResults Process();
}