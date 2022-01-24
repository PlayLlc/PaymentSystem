using Play.Emv.DataElements;

namespace Play.Emv.Terminal.CardholderVerificationMethods;

public interface ICardholderVerification
{
    public CvmResults Process();
}