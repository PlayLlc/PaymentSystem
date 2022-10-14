using System.Net;
using System.Net.Mail;

using Play.Core;

namespace Play.Identity.Api.Identity.Services._Email_Sms_Clientz;

public interface ISendEmail
{
    #region Instance Members

    public Task<EmailDeliveryResult> SendEmail(MailMessage message);

    #endregion
}

public class EmailDeliveryResult : Result
{
    #region Instance Values

    public readonly HttpStatusCode? StatusCode;

    #endregion

    #region Constructor

    public EmailDeliveryResult() : base()
    { }

    public EmailDeliveryResult(HttpStatusCode statusCode, params string[] errors) : base(errors)
    {
        StatusCode = statusCode;
    }

    #endregion
}