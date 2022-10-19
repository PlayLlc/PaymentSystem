using Play.Telecom.Twilio.Sms;

namespace Play.Telecom.SendGrid.Sms;

public interface ISendSmsMessages
{
    #region Instance Members

    public Task<SmsDeliveryResult> Send(string phone, string message);

    #endregion
}