using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.DataElements;

/// <summary>
///     Identifies the AEF referenced in commands related to a given ADF or DDF. It is a binary data object having a value
///     in the range 1 to 30 and with the three high order bits set to zero.
/// </summary>
public record ShortFileIdentifier : DataElement<byte>, IEqualityComparer<ShortFileIdentifier>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public static readonly Tag Tag = 0x88;
    private const byte _MinValue = 1;
    private const byte _MaxValue = 30;

    #endregion

    #region Constructor

    public ShortFileIdentifier(byte value) : base(value)
    {
        if (value > _MaxValue)
        {
            throw new ArgumentOutOfRangeException(
                $"The argument {nameof(value)} was out of range. {nameof(ShortFileIdentifier)} objects must have a decimal value between {_MinValue} and {_MaxValue}");
        }
    }

    public ShortFileIdentifier(ShortFileId value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
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

    #region Serialization

    public static ShortFileIdentifier Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ShortFileIdentifier Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 1;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ShortFileIdentifier)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = codec.Decode(BerEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(ShortFileIdentifier)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new ShortFileIdentifier(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

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
}