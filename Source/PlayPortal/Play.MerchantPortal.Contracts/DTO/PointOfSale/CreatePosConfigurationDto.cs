namespace Play.MerchantPortal.Contracts.DTO.PointOfSale;

public class CreatePosConfigurationDto
{
    public long CompanyId { get; set; }

    public long MerchantId { get; set; }

    public long StoreId { get; set; }

    public long TerminalId { get; set; }
}
