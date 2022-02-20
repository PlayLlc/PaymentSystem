using Play.Interchange.Codecs;

namespace Play.Interchange.DataFields;

public interface IEncodeInterchangeFields
{
    #region Instance Members

    internal DataField AsDataField();

    #endregion

    #region Serialization

    public byte[] Encode();
    public void Encode(Memory<byte> buffer, ref int offset);

    #endregion
}