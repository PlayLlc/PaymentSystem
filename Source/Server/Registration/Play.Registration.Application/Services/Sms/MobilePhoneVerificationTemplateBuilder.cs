using Play.Telecom.Twilio.Sms;

namespace Play.Registration.Application.Services.Sms;

public class MobilePhoneVerificationTemplateBuilder : SmsTemplateBuilder
{
    #region Static Metadata

    // HACK: we're going to load this from a text file when the builder is created
    protected const string _Template = "Your Play security code is <!--*confirmationCode*-->";

    #endregion

    #region Instance Members

    protected override string GetTemplate() => _Template;

    public string CreateSmsMessage(string confirmationCode) => CreateMessage(new Dictionary<string, string> {{nameof(confirmationCode), confirmationCode}});

    #endregion
}