using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The number that identifies the major industry and the card issuer and that forms the first part of the Primary
///     Account Number (PAN)
/// </summary>
public record IssuerIdentificationNumber : DataElement<uint>, IEqualityComparer<IssuerIdentificationNumber>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x42;
    private const byte _ByteLength = 3;
    private const byte _CharLength = 6;

    #endregion

    #region Constructor

    public IssuerIdentificationNumber(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => _ByteLength;

    public override ushort GetValueByteCount() => _ByteLength;

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static IssuerIdentificationNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override IssuerIdentificationNumber Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static IssuerIdentificationNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.NumericCodec.DecodeToUInt32(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new IssuerIdentificationNumber(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(IssuerIdentificationNumber? x, IssuerIdentificationNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerIdentificationNumber obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator uint(IssuerIdentificationNumber value) => value._Value;

    #endregion
}