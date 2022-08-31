namespace MerchantPortal.Core.Entities.PointOfSale;

public class PoSConfiguration : PosConfigurationHeader
{

    public TerminalConfiguration TerminalConfiguration { get; set; }

    public IEnumerable<Combination> Combinations { get; set; }


}
