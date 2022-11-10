﻿using System.ComponentModel.DataAnnotations;
using System.Net;

using Microsoft.Extensions.Logging;

using Play.Accounts.Domain.Services;
using Play.Core;
using Play.Core.Extensions.IEnumerable;
using Play.Telecom.Twilio.Email;

namespace Play.Accounts.Application.Services;

/// <summary>
///     This class sends a link containing a verification code to an email account to verify that the account is real
/// </summary>
/// and the user has control of the account
public class EmailAccountVerifier : IVerifyEmailAccounts
{
    #region Instance Values

    private readonly ICreateEmailVerificationReturnUrl _EmailVerificationReturnUrlBuilder;
    private readonly ISendEmail _EmailClient;
    private readonly ILogger<EmailAccountVerifier> _Logger;
    private readonly EmailVerificationTemplateBuilder _TemplateBuilder;

    #endregion

    #region Constructor

    public EmailAccountVerifier(
        ICreateEmailVerificationReturnUrl emailVerificationReturnUrlBuilder, ISendEmail emailClient, ILogger<EmailAccountVerifier> logger)
    {
        _EmailVerificationReturnUrlBuilder = emailVerificationReturnUrlBuilder;
        _EmailClient = emailClient;
        _Logger = logger;

        _TemplateBuilder = new EmailVerificationTemplateBuilder();
    }

    #endregion

    #region Instance Members

    public async Task<Result> SendVerificationCode(uint verificationCode, [EmailAddress] string email, string? fullName = null)
    {
        EmailDeliveryResult result = await _EmailClient.SendEmail(email, _TemplateBuilder.Subject,
                _TemplateBuilder.CreateEmail(_EmailVerificationReturnUrlBuilder.CreateReturnUrl(email, verificationCode)), fullName, true)
            .ConfigureAwait(false);

        // TODO: We need to make this resilient. We need an Exponential Retry strategy using Polly or something
        if (!result.Succeeded)
            _Logger.Log(LogLevel.Error,
                $"An attempt was made to send an email verification code to {email} but failed. {nameof(HttpStatusCode)}: [{result.StatusCode}]. Errors: [{result.Errors.ToStringAsConcatenatedValues()}]");

        return result;
    }

    #endregion
}