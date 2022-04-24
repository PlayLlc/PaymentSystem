using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.Verification;

public class CardholderVerificationMethodService
{
    #region Instance Values

    private readonly IVerifyCardholderPinOffline _OfflinePinAuthentication;
    private readonly IVerifyCardholderPinOnline _OnlinePinAuthentication;
    private readonly IVerifyCardholderSignature _CardholderSignatureVerification;

    #endregion

    #region Constructor

    public CardholderVerificationMethodService(
        IVerifyCardholderPinOffline offlinePinAuthentication, IVerifyCardholderPinOnline onlinePinAuthentication,
        IVerifyCardholderSignature cardholderSignatureVerification)
    {
        _OfflinePinAuthentication = offlinePinAuthentication;
        _OnlinePinAuthentication = onlinePinAuthentication;
        _CardholderSignatureVerification = cardholderSignatureVerification;
    }

    #endregion

    #region Instance Members

    public CvmCode Process(KernelDatabase database, params CardholderVerificationMethods[] cardholderVerificationMethods)
    {
        CvmCode result = new(0);

        for (int i = 0; i < cardholderVerificationMethods.Length; i++)
        {
            if (result == CvmCodes.Fail)
                return result;

            if (cardholderVerificationMethods[i] == CardholderVerificationMethods.OfflinePlaintextPin)
                _OfflinePinAuthentication.Process(database);
            if (cardholderVerificationMethods[i] == CardholderVerificationMethods.OfflineEncipheredPin)
                _OfflinePinAuthentication.Process(database);
            if (cardholderVerificationMethods[i] == CardholderVerificationMethods.OnlineEncipheredPin)
                _OnlinePinAuthentication.Process(database);
            if (cardholderVerificationMethods[i] == CardholderVerificationMethods.SignaturePaper)
                _CardholderSignatureVerification.Process();
        }

        return result;
    }

    #endregion
}