namespace Play.Identity.Api.Identity.Configuration;

public class BusinessPayClient
{
    #region Static Metadata

    private const string _ClientName = nameof(BusinessPayClient);
    private const string _Description = $"The client object that representing the {_ClientName}";

    #endregion

    #region Instance Values

    public string Description => _Description;
    public string ClientName => _ClientName;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string RedirectUris { get; set; } = string.Empty;
    public string PostLogoutRedirectUris { get; set; } = string.Empty;

    #endregion

    #region Instance Members

    public static string GetDescription()
    {
        return _Description;
    }

    public static string GetClientName()
    {
        return _ClientName;
    }

    #endregion
}