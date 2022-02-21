using Play.Emv.DataElements.Emv;

namespace Play.Emv.Terminal.CardholderVerificationMethods;

public interface ICardholderVerification
{
    public CvmResults Process();
}