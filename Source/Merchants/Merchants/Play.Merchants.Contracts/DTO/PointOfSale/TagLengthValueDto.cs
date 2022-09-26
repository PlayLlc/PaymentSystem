namespace Play.Merchants.Contracts.DTO.PointOfSale;

public record TagLengthValueDto
{
    #region Instance Values

    public string Name { get; set; } = string.Empty;

    public string Tag { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    #endregion
}