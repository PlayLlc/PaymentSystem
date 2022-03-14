using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services;

public interface ICardholderVerification
{
    public CvmResults Process();
}