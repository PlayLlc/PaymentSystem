using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     ICC PIN Encipherment Public Key Exponent used for PIN encipherment
/// </summary>
public record IccPinEnciphermentPublicKeyExponent : DataElement<uint>, IEqualityComparer<IccPinEnciphermentPublicKeyExponent>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F2E;
    private const byte _MinByteLength = 1;
    private const byte _MaxByteLength = 3;

    #endregion

    #region Constructor

    public IccPinEnciphermentPublicKeyExponent(uint value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public static IccPinEnciphermentPublicKeyExponent Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="BerParsingException"></exception>
    public override IccPinEnciphermentPublicKeyExponent Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IccPinEnciphermentPublicKeyExponent Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMinimumLength(value, _MaxByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new IccPinEnciphermentPublicKeyExponent(result);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IccPinEnciphermentPublicKeyExponent? x, IccPinEnciphermentPublicKeyExponent? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccPinEnciphermentPublicKeyExponent obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}