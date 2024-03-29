using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Identifies the AEF referenced in commands related to a given ADF or DDF. It is a binary data object having a value
///     in the range 1 to 30 and with the three high order bits set to zero.
/// </summary>
public record ShortFileIdentifier : DataElement<byte>, IEqualityComparer<ShortFileIdentifier>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x88;
    private const byte _MinValue = 1;
    private const byte _MaxValue = 30;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public ShortFileIdentifier(byte value) : base(value)
    {
        if (value > _MaxValue)
        {
            throw new DataElementParsingException(
                $"The argument {nameof(value)} was out of range. {nameof(ShortFileIdentifier)} objects must have a decimal value between {_MinValue} and {_MaxValue}");
        }
    }

    public ShortFileIdentifier(ShortFileId value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ShortFileIdentifier Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ShortFileIdentifier Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ShortFileIdentifier Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        Check.Primitive.ForMinimumValue(result, _MinValue, Tag);
        Check.Primitive.ForMaximumValue(result, _MaxValue, Tag);

        return new ShortFileIdentifier(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(ShortFileIdentifier? x, ShortFileIdentifier? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ShortFileIdentifier obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(ShortFileIdentifier left, Tag right) => left.Equals(right);
    public static bool operator ==(Tag left, ShortFileIdentifier right) => right.Equals(left);
    public static explicit operator byte(ShortFileIdentifier value) => value._Value;
    public static explicit operator ShortFileIdentifier(byte value) => new(value);
    public static implicit operator ShortFileId(ShortFileIdentifier value) => new(value._Value);
    public static bool operator !=(ShortFileIdentifier left, Tag right) => !left.Equals(right);
    public static bool operator !=(Tag left, ShortFileIdentifier right) => !right.Equals(left);

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public ushort GetByteCount() => 1;
    public override Tag GetTag() => Tag;
    public static bool IsValid(byte value) => value is >= _MinValue and <= _MaxValue;

    public static bool IsValid(Tag value)
    {
        uint valueCopy = value;

        if (valueCopy > byte.MaxValue)
            return false;

        return valueCopy is >= _MinValue and <= _MaxValue;
    }

    #endregion
}