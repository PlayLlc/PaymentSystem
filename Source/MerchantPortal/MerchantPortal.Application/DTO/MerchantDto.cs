namespace MerchantPortal.Application.DTO;

public record MerchantDto
{
    public long Id { get; set; }

    public string AcquirerId { get; set; } = string.Empty;

    public string MerchantIdentifier { get; set; } = string.Empty;

    public short MerchantCategoryCode { get; set; }

    public string Name { get; set; } = string.Empty;

    public string StreetAddress { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string ZipCode { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public long CompanyId { get; set; }
}
