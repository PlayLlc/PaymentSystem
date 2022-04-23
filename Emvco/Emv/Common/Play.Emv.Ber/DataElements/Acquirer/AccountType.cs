using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Uniquely identifies the acquirer within each payment system
/// </summary>
public record AccountType : DataElement<byte>, IEqualityComparer<AccountType>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x5F57;
    public static readonly AccountType Default = new(0);
    public static readonly AccountType Savings = new(10);
    public static readonly AccountType Checking = new(20);
    public static readonly AccountType Credit = new(30);
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public AccountType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static AccountType Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static AccountType Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        PlayCodec.NumericCodec.DecodeToByte(value);

        DecodedResult<byte> result = Codec.Decode(EncodingId, value).ToByteResult() ?? throw new DataElementParsingException(EncodingId);

        return new AccountType(result.Value);
    }

    public new byte[] EncodeValue() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(AccountType? x, AccountType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AccountType obj) => obj.GetHashCode();

    #endregion
}