namespace Play.Merchants.Domain.Entities;

public class KernelConfiguration
{
    #region Instance Values

    public byte KernelId { get; set; }

    public IEnumerable<TagLengthValue> TagLengthValues { get; set; } = Enumerable.Empty<TagLengthValue>();

    #endregion
}