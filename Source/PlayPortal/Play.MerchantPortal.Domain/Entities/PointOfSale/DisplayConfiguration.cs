namespace Play.MerchantPortal.Domain.Entities.PointOfSale;

public class DisplayConfiguration
{
    public string MessageHoldTime { get; set; } = string.Empty;

    public IEnumerable<DisplayMessageSet> DisplayMessages { get; set; } = Enumerable.Empty<DisplayMessageSet>();
}
