using Play.Ber.DataObjects;

namespace Play.Emv.Ber;

public interface IResolveKnownObjectsAtRuntime
{
    #region Instance Members

    public PrimitiveValue DecodePrimitiveValueAtRuntime(ReadOnlySpan<byte> value);
    public PrimitiveValue[] DecodePrimitiveValuesAtRuntime(ReadOnlySpan<byte> value);
    public bool TryDecodingPrimitiveValueAtRuntime(ReadOnlySpan<byte> value, out PrimitiveValue? result);

    #endregion
}