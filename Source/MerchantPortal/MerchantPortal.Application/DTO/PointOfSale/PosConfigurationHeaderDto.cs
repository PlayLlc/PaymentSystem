namespace MerchantPortal.Application.DTO.PointOfSale;

public class PosConfigurationHeaderDto
{
    public Guid Id { get; set; }

    public long TerminalId { get; set; }

    public long StoreId { get; set; }

    public long MerchantId { get; set; }

    public long CompanyId { get; set; }
}
