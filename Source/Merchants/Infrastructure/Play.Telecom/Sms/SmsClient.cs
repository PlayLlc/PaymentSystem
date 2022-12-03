using Microsoft.Extensions.Logging;

using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Play.Telecom.Twilio.Sms;

public class SmsClient : ISendSmsMessages
{
    #region Instance Values

    private readonly TwilioSmsConfiguration _Configuration;
    private readonly ITwilioRestClient _Client;
    private readonly ILogger<SmsClient> _Logger;

    #endregion

    #region Constructor

    public SmsClient(TwilioSmsConfiguration configuration, ILogger<SmsClient> logger)
    {
        _Configuration = configuration;
        TwilioClient.Init(configuration.AccountSid, configuration.AuthToken);
        _Client = TwilioClient.GetRestClient();
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    public async Task<SmsDeliveryResult> Send(string phone, string text)
    {
        CreateMessageOptions messageOptions = new(new PhoneNumber(phone));
        messageOptions.MessagingServiceSid = _Configuration.AccountSid;
        messageOptions.Body = text;
        MessageResource messageResource = await MessageResource.CreateAsync(messageOptions, _Client).ConfigureAwait(false);

        if (messageResource.ErrorCode is not null)
        {
            _Logger.Log(LogLevel.Error,
                $"The {nameof(SmsClient)} failed to send the SMS message to the {nameof(PhoneNumber)} with the phone number: [XXX-XXX-{phone.Substring(phone.Length - 4)}]. Error Message: [{messageResource.ErrorMessage}]");

            return new SmsDeliveryResult(messageResource.ErrorMessage);
        }

        return new SmsDeliveryResult();
    }

    #endregion
}