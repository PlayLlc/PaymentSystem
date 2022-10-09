namespace Play.Identity.Api.Models;

public record ExternalProvider
{
    #region Instance Values

    public string DisplayName { get; set; } = string.Empty;
    public string AuthenticationScheme { get; set; } = string.Empty;

    #endregion
}