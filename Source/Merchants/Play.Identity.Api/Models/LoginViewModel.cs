namespace Play.Identity.Api.Models;

public class LoginViewModel : LoginInputModel
{
    #region Instance Values

    public IEnumerable<ExternalProviderModel> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

    public bool IsExternalLoginOnly => (EnableLocalLogin == false) && (ExternalProviders?.Count() == 1);
    public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme ?? string.Empty : string.Empty;
    public bool AllowRememberLogin { get; set; } = true;
    public bool EnableLocalLogin { get; set; } = true;

    public IEnumerable<ExternalProviderModel> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProviderModel>();

    #endregion
}