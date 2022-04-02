using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;
using Play.Encryption.Certificates;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     ICC Public Key Exponent used for the verification of the Signed Dynamic Application Data
/// </summary>
public record IccPublicKeyExponent : DataElement<uint>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F47;
    private const byte _MinByteLength = 1;
    private const byte _MaxByteLength = 3;

    #endregion

    #region Constructor

    public IccPublicKeyExponent(uint value) : base(value)
    {
        _Value = value;
    }

    public IccPublicKeyExponent(PublicKeyExponent value) : base((uint) value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static IccPublicKeyExponent Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override IccPublicKeyExponent Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static IccPublicKeyExponent Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new IccPublicKeyExponent(result);
    }

    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Instance Members

    public PublicKeyExponent AsPublicKeyExponent() => PublicKeyExponent.Create(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(EncodingId, _Value);

    #endregion
}