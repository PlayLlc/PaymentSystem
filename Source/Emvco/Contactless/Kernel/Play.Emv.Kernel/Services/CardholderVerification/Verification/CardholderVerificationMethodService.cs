using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Kernel.Services.Verification;

public class CardholderVerificationService : IVerifyCardholder
{
    #region Instance Values

    private readonly IVerifyCardholderPinOffline _OfflinePinAuthentication;
    private readonly IVerifyCardholderPinOnline _OnlinePinAuthentication;
    private readonly IVerifyCardholderSignature _CardholderSignatureVerification;

    #endregion

    #region Constructor

    public CardholderVerificationService(
        IVerifyCardholderPinOffline offlinePinAuthentication, IVerifyCardholderPinOnline onlinePinAuthentication,
        IVerifyCardholderSignature cardholderSignatureVerification)
    {
        _OfflinePinAuthentication = offlinePinAuthentication;
        _OnlinePinAuthentication = onlinePinAuthentication;
        _CardholderSignatureVerification = cardholderSignatureVerification;
    }

    #endregion

    #region Instance Members

    public CvmCode Process(ITlvReaderAndWriter database, params CardholderVerificationMethods[] cardholderVerificationMethods)
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
                _CardholderSignatureVerification.Process(database);
        }

        return result;
    }

    public CvmResults Process() => throw new System.NotImplementedException();

    #endregion
}