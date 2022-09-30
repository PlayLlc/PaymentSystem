namespace Play.Merchants.Contracts.DTO.PointOfSale;

public record PointOfSaleConfigurationDto
{
    #region Instance Values

    public Guid Id { get; set; }

    public long TerminalId { get; set; }

    public long StoreId { get; set; }

    public long MerchantId { get; set; }

    public long CompanyId { get; set; }

    public TerminalConfigurationDto TerminalConfiguration { get; set; } = default!;

    public IEnumerable<CombinationConfigurationDto> Combinations { get; set; } = Enumerable.Empty<CombinationConfigurationDto>();

    public KernelConfigurationDto KernelConfiguration { get; set; } = default!;

    public DisplayConfigurationDto DisplayConfiguration { get; set; } = default!;

    public ProximityCouplingDeviceConfigurationDto ProximityCouplingDeviceConfiguration { get; set; } = default!;

    public CertificateAuthorityConfigurationDto CertificateAuthorityConfiguration { get; set; } = default!;

    #endregion
}