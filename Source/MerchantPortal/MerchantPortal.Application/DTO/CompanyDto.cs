namespace MerchantPortal.Application.DTO;

public record CompanyDto
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
