using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the code table according to ISO/IEC 8859 for displaying the Application Preferred Name
/// </summary>
public record IssuerCodeTableIndex : DataElement<byte>, IEqualityComparer<IssuerCodeTableIndex>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F11;
    private const byte _ByteLength = 1;
    private const byte _CharLength = 2;

    #endregion

    #region Constructor

    public IssuerCodeTableIndex(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public static bool StaticEquals(IssuerCodeTableIndex? x, IssuerCodeTableIndex? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static IssuerCodeTableIndex Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override IssuerCodeTableIndex Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static IssuerCodeTableIndex Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.NumericCodec.DecodeToByte(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new IssuerCodeTableIndex(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(IssuerCodeTableIndex? x, IssuerCodeTableIndex? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerCodeTableIndex obj) => obj.GetHashCode();

    #endregion
}