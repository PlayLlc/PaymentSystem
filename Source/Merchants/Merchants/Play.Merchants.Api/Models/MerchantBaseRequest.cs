namespace Play.Merchants.Api.Models;

public class MerchantBaseRequest
{
    #region Instance Values

    public string? AcquirerId { get; set; }

    public string? MerchantIdentifier { get; set; }

    public short MerchantCategoryCode { get; set; }

    public string? Name { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    #endregion
}