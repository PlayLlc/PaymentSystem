using Play.Telecom.SendGrid.Sms;

namespace Play.Accounts.Application.Services.Sms;

public class MobilePhoneVerificationTemplateBuilder : SmsTemplateBuilder
{
    #region Static Metadata

    // HACK: we're going to load this from a text file when the builder is created
    protected const string _Template = "Your Play security code is <!--*confirmationCode*-->";

    #endregion

    #region Constructor

    public MobilePhoneVerificationTemplateBuilder() : base()
    { }

    #endregion

    #region Instance Members

    protected override string GetTemplate()
    {
        return _Template;
    }

    public string CreateSmsMessage(string confirmationCode)
    {
        return CreateMessage(new Dictionary<string, string> {{nameof(confirmationCode), confirmationCode}});
    }

    #endregion
}