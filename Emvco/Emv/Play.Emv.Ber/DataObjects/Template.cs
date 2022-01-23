using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

namespace Play.Ber.Emv.DataObjects;

public abstract class Template : ConstructedValue
{
    #region Static Metadata

    protected static readonly EmvCodec _Codec = new();

    #endregion

    #region Instance Members

    public uint GetTagLengthValueByteCount()
    {
        return GetTagLengthValueByteCount(_Codec);
    }

    public TagLengthValue AsTagLengthValue()
    {
        return AsTagLengthValue(_Codec);
    }

    public ushort GetValueByteCount()
    {
        return _Codec.GetValueByteCount(GetChildren());
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return GetValueByteCount();
    }

    public abstract Tag[] GetChildTags();
    protected abstract IEncodeBerDataObjects?[] GetChildren();

    #endregion

    #region Serialization

    public byte[] EncodeValue()
    {
        return _Codec.EncodeValue(this, GetChildTags(), GetChildren());
    }

    public override byte[] EncodeValue(BerCodec berCodec)
    {
        return EncodeValue();
    }

    public byte[] EncodeTagLengthValue()
    {
        return _Codec.EncodeTagLengthValue(this, GetChildTags(), GetChildren());
    }

    public override byte[] EncodeTagLengthValue(BerCodec codec)
    {
        return EncodeTagLengthValue();
    }

    #endregion
}