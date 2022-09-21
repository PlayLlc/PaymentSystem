namespace Play.MerchantPortal.Contracts.Messages.PointOfSale;

public record SequenceConfigurationDto
{
    public uint Threshold { get; set; }
    public uint InitializationValue { get; set; }
}
