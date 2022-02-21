using Play.Interchange.Codecs;
using Play.Interchange.Codecs.Dynamic;

namespace Play.Interchange.DataFields;

public interface IEncodeInterchangeFields
{
    #region Instance Members

    internal DataFieldSpan AsDataField();

    #endregion

    #region Serialization

    public byte[] Encode();
    public void Encode(Memory<byte> buffer, ref int offset);

    #endregion
}