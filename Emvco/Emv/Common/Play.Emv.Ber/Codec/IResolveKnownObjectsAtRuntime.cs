using Play.Ber.DataObjects;

namespace Play.Emv.Ber;

public interface IResolveKnownObjectsAtRuntime
{
    public IEnumerable<PrimitiveValue> DecodePrimitiveSiblingsAtRuntime(ReadOnlyMemory<byte> value);
    public bool TryDecodingPrimitiveValueAtRuntime(ReadOnlyMemory<byte> value, out PrimitiveValue? result);
}