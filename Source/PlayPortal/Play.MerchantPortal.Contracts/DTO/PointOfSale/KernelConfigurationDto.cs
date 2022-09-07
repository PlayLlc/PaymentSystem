namespace Play.MerchantPortal.Contracts.DTO.PointOfSale;

public class KernelConfigurationDto
{
    public byte KernelId { get; set; }

    public IEnumerable<TagLengthValueDto> TagLengthValues { get; set; } = Enumerable.Empty<TagLengthValueDto>();
}
