namespace Play.Telecom.Twilio.Sms;

public interface ISendSmsMessages
{
    #region Instance Members

    public Task<SmsDeliveryResult> Send(string phone, string text);

    #endregion
}