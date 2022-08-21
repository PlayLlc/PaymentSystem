namespace MockPos.Dtos;

public class KernelConfigurationDto
{
    #region Instance Values

    public int KernelId { get; set; }
    public List<TagLengthValueDto> TagLengthValues { get; set; } = new();

    #endregion
}