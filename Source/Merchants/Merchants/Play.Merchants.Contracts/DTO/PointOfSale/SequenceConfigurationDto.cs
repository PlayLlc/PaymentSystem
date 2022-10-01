namespace Play.Merchants.Contracts.DTO;

public record SequenceConfigurationDto
{
    #region Instance Values

    public uint Threshold { get; set; }
    public uint InitializationValue { get; set; }

    #endregion
}