namespace Play.Telecom.SendGrid.Sms;

public record SmsClientConfiguration
{
    #region Instance Values

    public readonly string AccountSid;
    public readonly string AuthToken;

    #endregion

    #region Constructor

    public SmsClientConfiguration(string accountSid, string authToken)
    {
        AccountSid = accountSid;
        AuthToken = authToken;
    }

    #endregion
}