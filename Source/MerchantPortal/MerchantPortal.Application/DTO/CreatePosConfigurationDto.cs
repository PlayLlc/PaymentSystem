namespace MerchantPortal.Application.DTO;

public class CreatePosConfigurationDto
{
    public long CompanyId { get; set; }

    public long MerchatId { get; set; }

    public long StoreId { get; set; }

    public long TerminalId { get; set; }
}
