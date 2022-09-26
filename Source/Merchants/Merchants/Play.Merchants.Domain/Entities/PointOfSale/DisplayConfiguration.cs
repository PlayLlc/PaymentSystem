namespace Play.Merchants.Domain.Entities.PointOfSale;

public class DisplayConfiguration
{
    #region Instance Values

    public string MessageHoldTime { get; set; } = string.Empty;

    public IEnumerable<DisplayMessageSet> DisplayMessages { get; set; } = Enumerable.Empty<DisplayMessageSet>();

    #endregion
}