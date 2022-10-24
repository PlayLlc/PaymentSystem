using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

using Microsoft.Extensions.Logging;

using SendGrid;
using SendGrid.Helpers.Mail;

using Twilio.TwiML.Messaging;

namespace Play.Telecom.SendGrid.Email;

public sealed class EmailClient : ISendEmail
{
    #region Instance Values

    private readonly SendGridClient _Client;
    private readonly SendGridConfiguration _Config;
    private readonly ILogger<EmailClient> _Logger;

    #endregion

    #region Constructor

    public EmailClient(SendGridConfiguration sendGridConfiguration, ILogger<EmailClient> logger)
    {
        _Logger = logger;
        SendGridClient client = new SendGridClient(sendGridConfiguration.ApiKey);
        client.UrlPath = sendGridConfiguration.Server;
    }

    #endregion

    #region Instance Members

    // TODO: Make this more resilient 
    public async Task<EmailDeliveryResult> SendEmail(
        [EmailAddress] string recipient, string subject, string messageBody, string? recipientNickname = null, bool isBodyHtml = false)
    {
        EmailAddress to = recipientNickname is null ? new EmailAddress(recipient) : new EmailAddress(recipient, recipientNickname);
        Response? response = null;

        try
        {
            SendGridMessage emailMessage = new SendGridMessage
            {
                Subject = subject,
                From = string.IsNullOrEmpty(_Config.Nickname) ? new EmailAddress(_Config.FromEmail) : new EmailAddress(_Config.FromEmail, _Config.Nickname),
                ReplyTo = to
            };

            if (isBodyHtml)
                emailMessage.HtmlContent = messageBody;
            else
                emailMessage.PlainTextContent = messageBody;

            response = await _Client.SendEmailAsync(emailMessage).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new EmailDeliveryResult(response.StatusCode,
                    $"The {nameof(EmailClient)} encountered an error while attempting to send an email to the user with the SubjectId:");

            return new EmailDeliveryResult();
        }
        catch (Exception e)
        {
            string error = $"The {nameof(EmailClient)} encountered an error while communicating with the email server";
            _Logger.Log(LogLevel.Error, error, e);

            return new EmailDeliveryResult(response!.StatusCode, error);
        }
    }

    #endregion
}