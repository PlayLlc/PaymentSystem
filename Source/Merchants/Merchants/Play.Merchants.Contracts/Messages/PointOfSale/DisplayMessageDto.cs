namespace Play.Merchants.Contracts.Messages.PointOfSale;

public record DisplayMessageDto
{
    #region Instance Values

    public string MessageIdentifier { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    #endregion
}