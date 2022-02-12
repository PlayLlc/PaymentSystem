namespace Play.Emv.Terminal.Common.Services.CardholderVerificationMethods;

public class CardholderVerificationMethodService
{
    private readonly IVerifyCardholderPinOffline _OfflinePinAuthentication;
    private readonly IVerifyCardholderPinOnline _OnlinePinAuthentication;
    private readonly IVerifyCardholderSignature _CardholderSignatureVerification;

    public CardholderVerificationMethodService(IVerifyCardholderPinOffline offlinePinAuthentication, IVerifyCardholderPinOnline onlinePinAuthentication, IVerifyCardholderSignature cardholderSignatureVerification)
    {
        _OfflinePinAuthentication = offlinePinAuthentication;
        _OnlinePinAuthentication = onlinePinAuthentication;
        _CardholderSignatureVerification = cardholderSignatureVerification;
    }
}