namespace Play.Merchants.Contracts.DTO;

public record KernelConfigurationDto
{
    #region Instance Values

    public byte KernelId { get; set; }

    public IEnumerable<TagLengthValueDto> TagLengthValues { get; set; } = Enumerable.Empty<TagLengthValueDto>();

    #endregion
}