using Play.Telecom.SendGrid;

namespace Play.Identity.Application.Services;

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

    protected override string GetTemplate()
    {
        return _Template;
    }

    public string CreateEmail(string callbackUri)
    {
        return CreateMessage(new Dictionary<string, string> {{nameof(callbackUri), callbackUri}});
    }

    #endregion
}