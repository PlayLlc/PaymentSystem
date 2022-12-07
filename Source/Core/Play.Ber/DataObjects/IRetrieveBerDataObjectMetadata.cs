using Play.Ber.Codecs;
using Play.Ber.Tags;

namespace Play.Ber.DataObjects;

public interface IRetrieveBerDataObjectMetadata
{
    #region Equality

    public int GetHashCode();

    #endregion

    #region Instance Members

    public Tag GetTag();
    public uint GetTagLengthValueByteCount(BerCodec codec);
    public ushort GetValueByteCount(BerCodec codec);

    #endregion
}