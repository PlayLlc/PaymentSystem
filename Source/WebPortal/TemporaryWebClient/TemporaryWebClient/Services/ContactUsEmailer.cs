using Play.Core.Extensions.IEnumerable;
using Play.Core;
using Play.Telecom.Twilio.Email;

using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

using TemporaryWebClient.Models;

namespace TemporaryWebClient.Services;

public class ContactUsEmailer : IEmailContactUsMessages
{
    #region Instance Values

    private readonly ISendEmail _EmailClient;

    #endregion

    #region Constructor

    public ContactUsEmailer(ISendEmail emailClient)
    {
        _EmailClient = emailClient;
    }

    #endregion

    #region Instance Members

    private static string CreateMessageBody(string email, string name, string message)
    {
        StringBuilder builder = new StringBuilder();

        //builder.Append()
        builder.Append("----------------------------------------------------");
        builder.Append("SENDER DETAILS");
        builder.Append($"[Name: {name}]");
        builder.Append($"[Email: {email}]");
        builder.Append("----------------------------------------------------");
        builder.Append("[Message:]");
        builder.Append("");
        builder.Append(message);
        builder.Append("");
        builder.Append("[EndMessage]");

        return builder.ToString();
    }

    public async Task<Result> Send(ContactUsModel message)
    {
        EmailDeliveryResult result = await _EmailClient.SendEmail("contactus@paywithplay.com", $"[www.paywithplay.com][Contact Us][{message.Email}]",
                CreateMessageBody(message.Email, message.Name, message.EmailBody), message.Name)
            .ConfigureAwait(false);

        return result;
    }

    #endregion
}