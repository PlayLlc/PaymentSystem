﻿using System.Net.Mail;

using Microsoft.Extensions.Logging;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace Play.Telecom.SendGrid;

public sealed class EmailClient : ISendEmail
{
    #region Instance Values

    private readonly SendGridClient _Client;
    private readonly EmailClientConfig _Config;
    private readonly ILogger<EmailClient> _Logger;

    #endregion

    #region Constructor

    public EmailClient(EmailClientConfig emailClientConfig, ILogger<EmailClient> logger)
    {
        _Logger = logger;
        SendGridClient client = new SendGridClient(emailClientConfig.ApiKey);
        client.UrlPath = emailClientConfig.Server;
    }

    #endregion

    #region Instance Members

    public async Task<EmailDeliveryResult> SendEmail(MailMessage message)
    {
        Response? response = null;

        try
        {
            SendGridMessage emailMessage = new SendGridMessage
            {
                Subject = message.Subject,
                From = new EmailAddress(message.From!.Address, message.From.DisplayName),
                ReplyTo = new EmailAddress(_Config.FromEmail, _Config.Nickname)
            };

            if (message.IsBodyHtml)
                emailMessage.HtmlContent = message.Body;
            else
                emailMessage.PlainTextContent = message.Body;

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