using System.ComponentModel.DataAnnotations;

namespace Play.Telecom.Twilio.Email;

public interface ISendEmail
{
    #region Instance Members

    public Task<EmailDeliveryResult> SendEmail(
        [EmailAddress] string recipientEmail, string subject, string messageBody, string? recipientNickname = null, bool isBodyHtml = false);

    #endregion
}