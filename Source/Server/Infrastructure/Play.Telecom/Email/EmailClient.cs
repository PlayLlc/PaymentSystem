using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions.Logging;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace Play.Telecom.Twilio.Email;

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
        _Config = sendGridConfiguration;
        SendGridClient client = new(sendGridConfiguration.ApiKey);
        client.UrlPath = sendGridConfiguration.Server;
        _Client = client;
    }

    #endregion

    #region Instance Members

    // TODO: Make this more resilient 
    public async Task<EmailDeliveryResult> SendEmail(
        [EmailAddress] string recipientEmail, string subject, string messageBody, string? recipientNickname = null, bool isBodyHtml = false)
    {
        EmailAddress to = recipientNickname is null ? new EmailAddress(recipientEmail) : new EmailAddress(recipientEmail, recipientNickname);
        Response? response = null;

        try
        {
            SendGridMessage emailMessage = new()
            {
                Subject = subject,
                From = new EmailAddress(_Config.FromEmail)
            };
            emailMessage.AddTo(to);

            if (isBodyHtml)
                emailMessage.HtmlContent = messageBody;
            else
                emailMessage.PlainTextContent = messageBody;

            response = await _Client.SendEmailAsync(emailMessage).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new EmailDeliveryResult(response.StatusCode, $"The {nameof(EmailClient)} encountered an error while attempting to send an email");

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