namespace MerchantPortal.Application.DTO;

public record MerchantDto
{
    public long Id { get; set; }

    public string? AcquirerId { get; set; }

    public string? MerchantIdentifier { get; set; }

    public short MerchantCategoryCode { get; set; }

    public string? Name { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public long CompanyId { get; set; }
}
