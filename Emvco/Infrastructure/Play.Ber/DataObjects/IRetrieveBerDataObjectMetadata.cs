using Play.Ber.Codecs;
using Play.Ber.Identifiers;

namespace Play.Ber.DataObjects;

public interface IRetrieveBerDataObjectMetadata
{
    #region Instance Members

    public Tag GetTag();
    public uint GetTagLengthValueByteCount(BerCodec codec);
    public ushort GetValueByteCount(BerCodec codec);

    #endregion

    #region Equality

    public int GetHashCode();

    #endregion
}