using System.Net.Mail;

using Microsoft.Extensions.Logging;

using Play.Accounts.Domain.Services;
using Play.Randoms;
using Play.Telecom.SendGrid;

namespace Play.Accounts.Application.Services
{
    public interface ICreateEmailVerificationReturnUrl
    {
        #region Instance Members

        public string CreateReturnUrl(string email, uint confirmationCode);

        #endregion
    }

    internal class EmailAccountVerifier : IVerifyEmailAccounts
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

        public async Task<uint> SendVerificationCode(string email)
        {
            // TODO: Logging

            uint verificationCode = Randomize.Integers.UInt(100000, 999999);

            MailMessage message = new MailMessage(string.Empty, email, _TemplateBuilder.Subject,
                _TemplateBuilder.CreateEmail(_EmailVerificationReturnUrlBuilder.CreateReturnUrl(email, verificationCode))) {IsBodyHtml = true};

            // TODO: Exponential callback or something if this fails?
            await _EmailClient.SendEmail(message).ConfigureAwait(false);

            return verificationCode;
        }

        #endregion
    }
}