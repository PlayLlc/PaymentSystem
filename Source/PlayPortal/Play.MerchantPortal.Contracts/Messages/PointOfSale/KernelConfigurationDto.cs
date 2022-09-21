namespace Play.MerchantPortal.Contracts.Messages.PointOfSale;

public record KernelConfigurationDto
{
    public byte KernelId { get; set; }

    public IEnumerable<TagLengthValueDto> TagLengthValues { get; set; } = Enumerable.Empty<TagLengthValueDto>();
}
