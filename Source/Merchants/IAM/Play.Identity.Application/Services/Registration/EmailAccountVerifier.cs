using System.Net;
using System.Net.Mail;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Play.Core.Exceptions;
using Play.Identity.Application.Entities;
using Play.Identity.Domain;
using Play.Randoms;
using Play.Telecom.SendGrid.Email;

namespace Play.Identity.Application.Services.Registration;

public class EmailAccountVerifier : IVerifyEmailAccount
{
    #region Instance Values

    // store the value

    private readonly UserManager<UserIdentity> _UserManager;
    private readonly ISendEmail _EmailClient;
    private readonly ILogger<EmailAccountVerifier> _Logger;
    private readonly EmailVerificationTemplateBuilder _TemplateBuilder;

    #endregion

    #region Constructor

    public EmailAccountVerifier(
        UserManager<UserIdentity> userManager, ISendEmail emailClient, ILogger<EmailAccountVerifier> logger, EmailVerificationTemplateBuilder templateBuilder)
    {
        _UserManager = userManager;
        _EmailClient = emailClient;
        _Logger = logger;
        _TemplateBuilder = templateBuilder;
    }

    #endregion

    #region Instance Members

    public async Task<EmailDeliveryResult> SendVerificationCode(string emailAddress, Func<int, string> returnUrl)
    {
        UserIdentity? user = null;

        try
        {
            user = await _UserManager.FindByEmailAsync(emailAddress).ConfigureAwait(false);

            if (user is null)
                throw new PlayInternalException(
                    $"An internal error caused the {nameof(EmailAccountVerifier)} to not recognize a user with the SubjectId: [{user!.Id}].");

            user.EmailConfirmationCode = Randomize.Integers.Int(100000, 999999);
            _ = await _UserManager.UpdateAsync(user).ConfigureAwait(false);

            MailMessage email = new MailMessage(string.Empty, user.Email, _TemplateBuilder.Subject,
                _TemplateBuilder.CreateEmail(returnUrl.Invoke(user.EmailConfirmationCode!.Value))) {IsBodyHtml = true};

            return await _EmailClient.SendEmail(email).ConfigureAwait(false);
        }
        catch (PlayInternalException e)
        {
            string message = $"The {nameof(UserIdentity)} with the SubjectId: [{user!.Id}] could not be found";
            _Logger.Log(LogLevel.Error, message, e);

            return new EmailDeliveryResult(HttpStatusCode.NotFound, message);
        }
        catch (Exception e)
        {
            string message =
                $"The {nameof(EmailAccountVerifier)} experienced an unexpected error while invoking {nameof(SendVerificationCode)} with the {nameof(emailAddress)}: [{emailAddress}]";
            _Logger.Log(LogLevel.Error, message, e);

            return new EmailDeliveryResult(HttpStatusCode.NotFound, message);
        }
    }

    public async Task<bool> VerifyConfirmationCode(string email, int code)
    {
        UserIdentity? user = await _UserManager.FindByEmailAsync(email).ConfigureAwait(false);

        if (user is null)
            return false;

        user.EmailConfirmationCode = null;
        await _UserManager.UpdateAsync(user).ConfigureAwait(false);

        if (user.EmailConfirmed)
            return true;

        if (user.EmailConfirmationCode is null)
            return false;

        if (user.EmailConfirmationCode != code)
            return false;

        user.EmailConfirmed = true;
        await _UserManager.UpdateAsync(user).ConfigureAwait(false);

        return true;
    }

    #endregion
}