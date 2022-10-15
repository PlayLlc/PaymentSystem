using System.Net.Mail;

namespace Play.Identity.Api.Identity.Services._Email_Sms_Clientz;

public interface ISendEmail
{
    #region Instance Members

    public Task<EmailDeliveryResult> SendEmail(MailMessage message);

    #endregion
}