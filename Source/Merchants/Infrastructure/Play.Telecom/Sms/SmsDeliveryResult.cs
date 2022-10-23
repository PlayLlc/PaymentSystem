using Play.Core;

namespace Play.Telecom.Twilio.Sms;

public class SmsDeliveryResult : Result
{
    #region Constructor

    public SmsDeliveryResult() : base()
    { }

    public SmsDeliveryResult(params string[] errors) : base(errors)
    { }

    #endregion
}