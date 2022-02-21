using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates cardholder name according
/// </summary>
public record CardholderName : DataElement<char[]>, IEqualityComparer<CardholderName>
{
    #region Static Metadata

    /// <value>Hex: 5F20 Decimal: 95-32</value>
    public static readonly Tag Tag = 0x5F20;

    public static readonly BerEncodingId BerEncodingId = AlphaNumericSpecialCodec.Identifier;

    #endregion

    #region Constructor

    public CardholderName(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static CardholderName Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static CardholderName Decode(ReadOnlySpan<byte> value)
    {
        const ushort minByteLength = 2;
        const ushort maxByteLength = 26;

        if (value.Length is not >= minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(CardholderName)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<char[]> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<char[]>
            ?? throw new InvalidOperationException(
                $"The {nameof(CardholderName)} could not be initialized because the {nameof(AlphaNumericSpecialCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        return new CardholderName(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(CardholderName? x, CardholderName? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CardholderName obj) => obj.GetHashCode();

    #endregion
}