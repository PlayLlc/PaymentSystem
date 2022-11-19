using System.Net;

using Play.Core;

namespace Play.Telecom.Twilio.Email;

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