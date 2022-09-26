namespace Play.MerchantPortal.Api.Models;

public class StoreDetailsRequest
{
    #region Instance Values

    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public long MerchantId { get; set; }

    #endregion
}