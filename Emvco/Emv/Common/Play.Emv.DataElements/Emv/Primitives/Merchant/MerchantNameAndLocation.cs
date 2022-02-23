using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the name and location of the merchant
/// </summary>
public record MerchantNameAndLocation : DataElement<char[]>, IEqualityComparer<MerchantNameAndLocation>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    public static readonly Tag Tag = 0x9F4E;

    #endregion

    #region Constructor

    public MerchantNameAndLocation(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static MerchantNameAndLocation Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static MerchantNameAndLocation Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<char[]> result = _Codec.Decode(PlayEncodingId, value) as DecodedResult<char[]>
            ?? throw new InvalidOperationException(
                $"The {nameof(MerchantNameAndLocation)} could not be initialized because the {nameof(AlphaNumericSpecialCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        return new MerchantNameAndLocation(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(MerchantNameAndLocation? x, MerchantNameAndLocation? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(MerchantNameAndLocation obj) => obj.GetHashCode();

    #endregion
}