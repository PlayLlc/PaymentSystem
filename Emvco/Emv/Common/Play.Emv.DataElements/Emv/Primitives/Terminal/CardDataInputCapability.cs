using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the card data input capability of the Terminal and Reader.
/// </summary>
public record CardDataInputCapability : DataElement<byte>, IEqualityComparer<CardDataInputCapability>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8117;

    #endregion

    #region Constructor

    public CardDataInputCapability(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static CardDataInputCapability Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static CardDataInputCapability Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<byte> result = _Codec.Decode(PlayEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(CardDataInputCapability)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new CardDataInputCapability(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(CardDataInputCapability? x, CardDataInputCapability? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CardDataInputCapability obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(CardDataInputCapability value) => value._Value;

    #endregion
}