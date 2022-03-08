using Play.Emv.DataElements.Emv.Primitives.CVM;

namespace Play.Emv.Terminal.CardholderVerificationMethods;

public interface ICardholderVerification
{
    public CvmResults Process();
}