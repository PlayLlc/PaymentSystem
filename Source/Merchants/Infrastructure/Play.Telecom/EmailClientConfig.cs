using System.ComponentModel.DataAnnotations;

namespace Play.Telecom.SendGrid;

public class EmailClientConfig
{
    #region Instance Values

    public string ApiKey { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Server { get; set; } = string.Empty;

    [EmailAddress]
    public string FromEmail { get; set; } = string.Empty;

    public int Ports { get; set; } // Hack: I don't know where to specify the HTTPS port

    #endregion
}