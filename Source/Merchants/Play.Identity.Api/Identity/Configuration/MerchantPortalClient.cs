namespace Play.Identity.Api.Identity.Configuration;

public class MerchantPortalClient
{
    #region Static Metadata

    private const string _ClientName = nameof(MerchantPortalClient);

    private const string _Description = $"The client object that representing the {_ClientName}";

    #endregion

    #region Instance Values

    public string Description => _Description;

    public string ClientName => _ClientName;
    public string ClientId { get; set; } = string.Empty;
    public IEnumerable<string> RedirectUris { get; set; } = new List<string>();
    public string ClientSecret { get; set; } = string.Empty;

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