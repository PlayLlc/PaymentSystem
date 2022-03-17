using System;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services;

public class CardholderVerificationMethodService
{
    #region Instance Values

    private readonly IVerifyCardholderPinOffline _OfflinePinAuthentication;
    private readonly IVerifyCardholderPinOnline _OnlinePinAuthentication;
    private readonly IVerifyCardholderSignature _CardholderSignatureVerification;

    #endregion

    #region Constructor

    public CardholderVerificationMethodService(
        IVerifyCardholderPinOffline offlinePinAuthentication,
        IVerifyCardholderPinOnline onlinePinAuthentication,
        IVerifyCardholderSignature cardholderSignatureVerification)
    {
        _OfflinePinAuthentication = offlinePinAuthentication;
        _OnlinePinAuthentication = onlinePinAuthentication;
        _CardholderSignatureVerification = cardholderSignatureVerification;
    }

    #endregion

    #region Instance Members

    public void Process(CardholderVerificationMethod cardholderVerificationMethod, IQueryTlvDatabase database)
    {
        if (cardholderVerificationMethod == CardholderVerificationMethod.Fail)

            throw new NotImplementedException();
    }

    #endregion

    // TODO:  set the ‘Cardholder verification was performed’ bit in the TSI to 1.
}