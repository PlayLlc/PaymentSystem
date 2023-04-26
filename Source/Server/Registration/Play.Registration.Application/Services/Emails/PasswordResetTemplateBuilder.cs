using Play.Telecom.Twilio.Email;

namespace Play.Registration.Application.Services.Emails;

public class PasswordResetTemplateBuilder : EmailTemplateBuilder
{
    #region Static Metadata

    // HACK: we're going to load this from a text file when the builder is created
    protected const string _Template = "";

    #endregion

    #region Constructor

    public PasswordResetTemplateBuilder() : base("Your Password Has Been Reset!")
    { }

    #endregion

    #region Instance Members

    protected override string GetTemplate() => _Template;

    public string CreateEmail(string callbackUri) => CreateMessage(new Dictionary<string, string> {{nameof(callbackUri), callbackUri}});

    #endregion
}