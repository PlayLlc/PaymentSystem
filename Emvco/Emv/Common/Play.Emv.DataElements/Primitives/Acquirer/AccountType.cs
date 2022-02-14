using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Uniquely identifies the acquirer within each payment system
/// </summary>
public record AccountType : DataElement<byte>, IEqualityComparer<AccountType>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = Numeric.Identifier;
    public static readonly Tag Tag = 0x9F01;
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

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static AccountType Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static AccountType Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(BerEncodingId, value).ToByteResult()
            ?? throw new DataElementNullException(BerEncodingId);

        return new AccountType(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

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