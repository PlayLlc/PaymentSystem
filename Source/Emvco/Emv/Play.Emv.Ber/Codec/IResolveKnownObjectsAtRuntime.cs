using Play.Ber.DataObjects;
using Play.Ber.Tags;

namespace Play.Emv.Ber;

public interface IResolveKnownObjectsAtRuntime
{
    #region Instance Members

    public bool TryDecodingAtRuntime(Tag tag, ReadOnlyMemory<byte> value, out PrimitiveValue? primitiveValue);

    #endregion
}