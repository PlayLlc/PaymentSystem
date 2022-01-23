using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the name and location of the merchant
/// </summary>
public record MerchantNameAndLocation : DataElement<char[]>, IEqualityComparer<MerchantNameAndLocation>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = AlphaNumericSpecialCodec.Identifier;
    public static readonly Tag Tag = 0x9F4E;

    #endregion

    #region Constructor

    public MerchantNameAndLocation(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static MerchantNameAndLocation Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static MerchantNameAndLocation Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<char[]> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<char[]>
            ?? throw new
                InvalidOperationException($"The {nameof(MerchantNameAndLocation)} could not be initialized because the {nameof(AlphaNumericSpecialCodec)} returned a null {nameof(DecodedResult<char[]>)}");

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