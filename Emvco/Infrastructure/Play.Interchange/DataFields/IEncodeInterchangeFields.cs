using Play.Interchange.Codecs;

namespace Play.Interchange.DataFields;

public interface IEncodeInterchangeFields
{
    #region Instance Members

    public DataField AsDataField(InterchangeCodec codec);

    #endregion

    #region Serialization

    public byte[] Encode(InterchangeCodec codec);
    public void Encode(InterchangeCodec codec, Memory<byte> buffer, ref int offset);

    #endregion
}