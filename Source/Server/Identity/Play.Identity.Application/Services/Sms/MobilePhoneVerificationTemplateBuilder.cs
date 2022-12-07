using Play.Telecom.Twilio.Sms;

namespace Play.Identity.Application.Services.Sms;

public class MobilePhoneVerificationTemplateBuilder : SmsTemplateBuilder
{
    #region Static Metadata

    // HACK: we're going to load this from a text file when the builder is created
    protected const string _Template = "Your Play security code is <!--*confirmationCode*-->";

    #endregion

    #region Constructor

    #endregion

    #region Instance Members

    protected override string GetTemplate() => _Template;

    public string CreateSmsMessage(string confirmationCode) => CreateMessage(new() {{nameof(confirmationCode), confirmationCode}});

    #endregion
}