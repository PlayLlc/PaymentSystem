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

    public uint GetTagLengthValueByteCount() => GetTagLengthValueByteCount(_Codec);
    public TagLengthValue AsTagLengthValue() => AsTagLengthValue(_Codec);
    public ushort GetValueByteCount() => _Codec.GetValueByteCount(GetChildren());
    public override ushort GetValueByteCount(BerCodec codec) => GetValueByteCount();
    public abstract Tag[] GetChildTags();
    protected abstract IEncodeBerDataObjects?[] GetChildren();

    #endregion

    #region Serialization

    public byte[] EncodeValue() => _Codec.EncodeValue(this, GetChildTags(), GetChildren());
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();
    public byte[] EncodeTagLengthValue() => _Codec.EncodeTagLengthValue(this, GetChildTags(), GetChildren());
    public override byte[] EncodeTagLengthValue(BerCodec codec) => EncodeTagLengthValue();

    #endregion

    // TODO ===========================================================================
}