using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     When concatenated with the Acquirer Identifier, uniquely identifies a given merchant
/// </summary>
public record MerchantIdentifier : DataElement<char[]>, IEqualityComparer<MerchantIdentifier>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = AlphaNumericSpecialDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x9F16;

    #endregion

    #region Constructor

    public MerchantIdentifier(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static MerchantIdentifier Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static MerchantIdentifier Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 15;
        const ushort charLength = 15;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(MerchantIdentifier)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<char[]> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<char[]>
            ?? throw new InvalidOperationException(
                $"The {nameof(MerchantIdentifier)} could not be initialized because the {nameof(AlphaNumericSpecialDataElementCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(MerchantIdentifier)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new MerchantIdentifier(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(MerchantIdentifier? x, MerchantIdentifier? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(MerchantIdentifier obj) => obj.GetHashCode();

    #endregion
}