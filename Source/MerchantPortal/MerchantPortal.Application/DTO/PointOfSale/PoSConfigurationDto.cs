namespace MerchantPortal.Application.DTO.PointOfSale;

public class PoSConfigurationDto : PosConfigurationHeaderDto
{
    public TerminalConfigurationDto TerminalConfiguration { get; set; }

    public IEnumerable<CombinationDto> Combinations { get; set; }

    public KernelConfigurationDto KernelConfiguration { get; set; }

    public DisplayConfigurationDto DisplayConfiguration { get; set; }

    public CertificateAuthorityConfigurationDto CertificateAuthorityConfiguration { get; set; }
}
