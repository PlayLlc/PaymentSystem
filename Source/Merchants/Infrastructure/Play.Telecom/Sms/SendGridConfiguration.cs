namespace Play.Telecom.SendGrid.Sms;

public record TwilioSmsConfiguration
{
    #region Instance Values

    public readonly string AccountSid;
    public readonly string AuthToken;

    #endregion

    #region Constructor

    public TwilioSmsConfiguration(string accountSid, string authToken)
    {
        AccountSid = accountSid;
        AuthToken = authToken;
    }

    #endregion
}