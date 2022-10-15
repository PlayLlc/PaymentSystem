using System.Net;

using Play.Core;

namespace Play.Identity.Api.Identity.Services._Email_Sms_Clientz;

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