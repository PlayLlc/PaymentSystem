namespace MockPos.Configuration;

public class KernelConfigurationDto
{
    #region Instance Values

    public int KernelId { get; set; }
    public List<TagLengthValueDto> TagLengthValues { get; set; } = new();

    #endregion
}