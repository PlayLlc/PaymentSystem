namespace MerchantPortal.Core.Entities.PointOfSale;

public class KernelConfiguration
{
    public byte KernelId { get; set; }

    public IEnumerable<TagLengthValue> TagLengthValues { get; set; }
}
