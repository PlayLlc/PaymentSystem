using Play.Accounts.Domain.Services;
using Play.Telecom.SendGrid.Sms;

using Microsoft.Extensions.Logging;

using Play.Accounts.Application.Services.Sms;
using Play.Accounts.Domain.ValueObjects;
using Play.Core;
using Play.Telecom.Twilio.Sms;

using static Duende.IdentityServer.Models.IdentityResources;

namespace Play.Accounts.Application.Services;

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

    public async Task<Result> SendVerificationCode(uint code, string mobile)
    {
        string message = _TemplateBuilder.CreateSmsMessage($"{code}");

        SmsDeliveryResult result = await _SmsClient.Send(mobile, message).ConfigureAwait(false);

        if (result.Succeeded)
            return result;

        _Logger.Log(LogLevel.Error, $"An attempt was made to send an SMS verification code to [XXX-XXX-{mobile.Substring(mobile.Length - 4)}]");

        return result;
    }

    #endregion
}