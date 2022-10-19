using System.Net.Mail;

namespace Play.Telecom.SendGrid.Email;

public interface ISendEmail
{
    #region Instance Members

    public Task<EmailDeliveryResult> SendEmail(MailMessage message);

    #endregion
}