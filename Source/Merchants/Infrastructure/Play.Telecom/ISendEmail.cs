using System.Net.Mail;

namespace Play.Telecom.SendGrid;

public interface ISendEmail
{
    #region Instance Members

    public Task<EmailDeliveryResult> SendEmail(MailMessage message);

    #endregion
}