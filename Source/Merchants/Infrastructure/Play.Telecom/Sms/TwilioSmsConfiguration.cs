namespace Play.Telecom.SendGrid.Sms;

public record TwilioSmsConfiguration
{
    #region Instance Values

    public string AccountSid { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;

    #endregion

    #region Constructor

    public TwilioSmsConfiguration()
    { }

    public TwilioSmsConfiguration(string accountSid, string authToken)
    {
        AccountSid = accountSid;
        AuthToken = authToken;
    }

    #endregion
}