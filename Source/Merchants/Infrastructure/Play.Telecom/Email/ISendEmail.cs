﻿using System.ComponentModel.DataAnnotations;

namespace Play.Telecom.SendGrid.Email;

public interface ISendEmail
{
    #region Instance Members

    public Task<EmailDeliveryResult> SendEmail(
        [EmailAddress] string recipient, string subject, string messageBody, string? recipientNickname = null, bool isBodyHtml = false);

    #endregion
}