namespace Play.Merchants.Contracts.Messages.PointOfSale;

public record CreatePosConfigurationDto
{
    #region Instance Values

    public long CompanyId { get; set; }

    public long MerchantId { get; set; }

    public long StoreId { get; set; }

    public long TerminalId { get; set; }

    #endregion
}