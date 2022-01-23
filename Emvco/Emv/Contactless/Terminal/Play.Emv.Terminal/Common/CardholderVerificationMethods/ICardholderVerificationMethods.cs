using Play.Emv.DataElements;

namespace Play.Emv.Terminal.Common.Services.CardholderVerificationMethods.Pin;

public interface ICardholderVerification
{
    public CvmResults Process();
}