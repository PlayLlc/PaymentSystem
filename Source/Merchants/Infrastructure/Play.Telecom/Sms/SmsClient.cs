using Play.Telecom.Twilio.Sms;

namespace Play.Telecom.SendGrid.Sms
{
    public class SmsClient : ISendSmsMessages
    {
        #region Instance Values

        private readonly SmsClientConfiguration _Configuration;

        #endregion

        #region Constructor

        public SmsClient(SmsClientConfiguration configuration)
        {
            _Configuration = new SmsClientConfiguration("ACe687560431e5aea720ee30c443b767c3", "df1ed3dc580ef0585d82504a8d62ba38");

            //_Client = TwilioClient.Init(_Configuration.AccountSid, _Configuration.AuthToken);
        }

        #endregion

        #region Instance Members

        public async Task<SmsDeliveryResult> Send(string phone, string message)
        {
            throw new NotImplementedException();

            //var messageOptions = new CreateMessageOptions(
            //    new PhoneNumber("+14693461987"));

            //var message = MessageResource.Create(
            //    body: "Hi there",
            //    from: new Twilio.Types.PhoneNumber("+15017122661"),
            //    to: new Twilio.Types.PhoneNumber("+15558675310")
            //);
            //message.Status
        }

        #endregion
    }
}