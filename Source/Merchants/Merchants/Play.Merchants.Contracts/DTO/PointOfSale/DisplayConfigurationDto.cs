namespace Play.Merchants.Contracts.DTO.PointOfSale;

public record DisplayConfigurationDto
{
    #region Instance Values

    public string MessageHoldTime { get; set; } = string.Empty;

    public IEnumerable<DisplayMessageSetDto> DisplayMessages { get; set; } = Enumerable.Empty<DisplayMessageSetDto>();

    #endregion
}