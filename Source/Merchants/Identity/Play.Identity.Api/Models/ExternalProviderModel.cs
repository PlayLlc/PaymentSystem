namespace Play.Identity.Api.Models;

public record ExternalProviderModel
{
    #region Instance Values

    public string DisplayName { get; set; } = string.Empty;
    public string AuthenticationScheme { get; set; } = string.Empty;

    #endregion
}