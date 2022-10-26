using System.ComponentModel.DataAnnotations;

namespace Play.Telecom.Twilio.Email;

public class SendGridConfiguration
{
    #region Instance Values

    [Required]
    public string ApiKey { get; set; } = string.Empty;

    public string Nickname { get; set; } = string.Empty;

    [Required]
    public string Server { get; set; } = string.Empty;

    [EmailAddress]
    [Required]
    public string FromEmail { get; set; } = string.Empty;

    public int Ports { get; set; } // Hack: I don't know where to specify the HTTPS port

    #endregion
}