namespace Play.Merchants.Contracts.DTO.PointOfSale;

public record SequenceConfigurationDto
{
    #region Instance Values

    public uint Threshold { get; set; }
    public uint InitializationValue { get; set; }

    #endregion
}