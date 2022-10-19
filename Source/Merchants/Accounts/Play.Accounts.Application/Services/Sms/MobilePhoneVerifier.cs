using System.Net;
using System.Net.Mail;

using Play.Accounts.Domain.Services;
using Play.Telecom.SendGrid.Sms;

using Microsoft.Extensions.Logging;

using Play.Accounts.Application.Services.Sms;
using Play.Accounts.Domain.ValueObjects;
using Play.Core;
using Play.Telecom.Twilio.Sms;

namespace Play.Accounts.Application.Services
{
    public class MobilePhoneVerifier : IVerifyMobilePhones
    {
        #region Instance Values

        private readonly ISendSmsMessages _SmsClient;
        private readonly MobilePhoneVerificationTemplateBuilder _TemplateBuilder;
        private readonly ILogger<MobilePhoneVerifier> _Logger;

        #endregion

        #region Constructor

        public MobilePhoneVerifier(ISendSmsMessages smsClient, ILogger<MobilePhoneVerifier> logger)
        {
            _SmsClient = smsClient;
            _TemplateBuilder = new MobilePhoneVerificationTemplateBuilder();
            _Logger = logger;
        }

        #endregion

        #region Instance Members

        public async Task<Result> SendVerificationCode(uint code, Phone mobile)
        {
            var message = _TemplateBuilder.CreateSmsMessage($"{code}");

            var result = await _SmsClient.Send(mobile, message).ConfigureAwait(false);

            if (result.Succeeded)
                return result;

            // BUG: We can't log the user's full phone number. We'll need to log the Aggregate's ID and last 4 of phone or something
            _Logger.Log(LogLevel.Error, $"An attempt was made to send an SMS verification code to {mobile.Value}");

            return result;
        }

        #endregion
    }
}