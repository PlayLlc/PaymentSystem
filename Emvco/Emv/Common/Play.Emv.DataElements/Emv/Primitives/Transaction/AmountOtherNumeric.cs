using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Secondary amount associated with the transaction representing a cashback amount
/// </summary>
public record AmountOtherNumeric : DataElement<ulong>, IEqualityComparer<AmountOtherNumeric>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F03;

    #endregion

    #region Constructor

    public AmountOtherNumeric(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public Money AsMoney(CultureProfile cultureProfile) => new(_Value, cultureProfile);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static AmountOtherNumeric Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static AmountOtherNumeric Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 6;
        const ushort charLength = 12;

        Check.Primitive.ForExactLength(value, byteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(AmountOtherNumeric)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(AmountOtherNumeric)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new AmountOtherNumeric(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(AmountOtherNumeric? x, AmountOtherNumeric? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AmountOtherNumeric obj) => obj.GetHashCode();

    #endregion
}