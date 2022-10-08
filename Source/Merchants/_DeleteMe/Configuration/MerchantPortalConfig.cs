namespace _DeleteMe.Configuration;

public class MerchantPortalConfig
{
    #region Instance Values

    public string ClientId { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public IEnumerable<string> RedirectUris { get; set; } = new List<string>();
    public string ClientSecret { get; set; } = string.Empty;

    #endregion
}