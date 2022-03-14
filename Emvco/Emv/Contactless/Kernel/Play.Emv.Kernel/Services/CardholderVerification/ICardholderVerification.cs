using Play.Emv.DataElements;

namespace Play.Emv.Terminal.Contracts;

public interface ICardholderVerification
{
    public CvmResults Process();
}