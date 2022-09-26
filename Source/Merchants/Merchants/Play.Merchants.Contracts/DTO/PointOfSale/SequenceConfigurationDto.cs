namespace Play.Merchants.Contracts.Messages.PointOfSale;

public record SequenceConfigurationDto
{
    #region Instance Values

    public uint Threshold { get; set; }
    public uint InitializationValue { get; set; }

    #endregion
}