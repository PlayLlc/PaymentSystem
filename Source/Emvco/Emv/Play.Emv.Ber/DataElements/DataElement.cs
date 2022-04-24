using Play.Ber.Codecs;
using Play.Ber.DataObjects;

namespace Play.Emv.Ber.DataElements;

public abstract record DataElement<T>(T _Value) : PrimitiveValue, IEncodeDataElement
{
    #region Static Metadata

    protected static readonly EmvCodec _Codec = new();

    #endregion

    #region Instance Values

    protected readonly T _Value = _Value;

    #endregion

    #region Instance Members

    public TagLengthValue AsTagLengthValue() => AsTagLengthValue(_Codec);

    // TODO: The below should not be casting to ushort. We need to change PrimitiveValue.GetTagLengthValueByteCount to
    // TODO: return a ushort instead
    public ushort GetTagLengthValueByteCount() => (ushort) GetTagLengthValueByteCount(_Codec);
    public ushort GetValueByteCount() => _Codec.GetByteCount(GetEncodingId(), _Value);
    public override ushort GetValueByteCount(BerCodec codec) => GetValueByteCount();

    #endregion

    #region Serialization

    /// <summary>
    ///     Encodes this objects content as the Value field of a Tag-Length-Value encoding
    /// </summary>
    /// <returns></returns>
    public virtual byte[] EncodeValue() => _Codec.EncodeValue(GetEncodingId(), _Value);

    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

    /// <summary>
    ///     Encodes this objects content as the Value field of a Tag-Length-Value encoding
    /// </summary>
    /// <param name="length">This parameter determines the length of the TLV Value field</param>
    /// <returns></returns>
    public virtual byte[] EncodeValue(int length) => _Codec.EncodeValue(GetEncodingId(), _Value, length);

    public override byte[] EncodeValue(BerCodec codec, int length) => EncodeValue(length);

    /// <summary>
    ///     Encodes this object as a Tag-Length-Value
    /// </summary>
    /// <returns></returns>
    public byte[] EncodeTagLengthValue() => EncodeTagLengthValue(_Codec);

    /// <summary>
    ///     Encodes this object as a Tag-Length-Value
    /// </summary>
    /// <param name="length">This parameter determines the length of the TLV Value field</param>
    /// <returns></returns>
    public byte[] EncodeTagLengthValue(int length) => EncodeTagLengthValue(_Codec, length);

    #endregion

    #region Operator Overrides

    public static implicit operator TagLengthValue(DataElement<T> value) => value.AsTagLengthValue();

    #endregion
}