namespace MerchantPortal.Core.Entities.PointOfSale;

public class PoSConfiguration : PosConfigurationHeader
{
    public TerminalConfiguration TerminalConfiguration { get; set; }

    public IEnumerable<Combination> Combinations { get; set; }

    public KernelConfiguration KernelConfiguration { get; set; }

    public DisplayConfiguration DisplayConfiguration { get; set; }

    public ProximityCouplingDeviceConfiguration ProximityCouplingDeviceConfiguration { get; set; }

    public CertificateAuthorityConfiguration CertificateAuthorityConfiguration { get; set; }
}
