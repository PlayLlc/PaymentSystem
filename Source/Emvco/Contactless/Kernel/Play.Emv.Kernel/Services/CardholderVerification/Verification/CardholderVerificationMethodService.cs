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
    //not implemented
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

    public CvmResults Process(ITlvReaderAndWriter database)
    {
        CvmCode result = new(0);

        CvmList cvmList = database.Get<CvmList>(CvmList.Tag);

        if (!cvmList.AreCardholderVerificationRulesPresent())
        {
            database.Update(TerminalVerificationResultCodes.IccDataMissing);

            return new(CvmCodes.Empty, new CvmConditionCode(0), CvmResultCodes.Unknown);
        }

        if (!cvmList.TryGetCardholderVerificationRules(out CvmRule[]? rules))
        {
            database.Update(TerminalVerificationResultCodes.IccDataMissing);

            return new(CvmCodes.Empty, new CvmConditionCode(0), CvmResultCodes.Unknown);
        }

        TerminalCapabilities terminalCapabilities = database.Get<TerminalCapabilities>(TerminalCapabilities.Tag);
        for (int i = 0; i < rules.Length; i++)
        {
            CvmRule currentRule = rules[i];

            if (currentRule.GetCvmConditionCode().Always() == true)
            {
                if (currentRule.)
                if (!currentRule.GetCvmCode().IsSupported(terminalCapabilities))
                {
                    return new(CvmCodes.Fail, new CvmConditionCode(0), CvmResultCodes.Failed);
                }


            }
            else
            {

            }

            //if (cardholderVerificationMethods[i] == CardholderVerificationMethods.OfflinePlaintextPin)
            //    result = _OfflinePinAuthentication.Process(database);
            //if (cardholderVerificationMethods[i] == CardholderVerificationMethods.OfflineEncipheredPin)
            //    result = _OfflinePinAuthentication.Process(database);
            //if (cardholderVerificationMethods[i] == CardholderVerificationMethods.OnlineEncipheredPin)
            //    result = _OnlinePinAuthentication.Process(database);
            //if (cardholderVerificationMethods[i] == CardholderVerificationMethods.SignaturePaper)
            //    result = _CardholderSignatureVerification.Process(database);
        }

        return new(CvmCodes.Fail, new CvmConditionCode(0), CvmResultCodes.Failed);
    }

    public CvmResults Process() => throw new System.NotImplementedException();

    #endregion
}