using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     When concatenated with the Acquirer EncodingId, uniquely identifies a given merchant
/// </summary>
public record MerchantIdentifier : DataElement<char[]>, IEqualityComparer<MerchantIdentifier>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    public static readonly Tag Tag = 0x9F16;

    #endregion

    #region Constructor

    public MerchantIdentifier(ReadOnlySpan<char> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static MerchantIdentifier Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override MerchantIdentifier Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static MerchantIdentifier Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 15;
        const ushort charLength = 15;

        if (value.Length != byteLength)
        {
            throw new
                DataElementParsingException($"The Primitive Value {nameof(MerchantIdentifier)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value) as DecodedResult<char[]>
            ?? throw new
                DataElementParsingException($"The {nameof(MerchantIdentifier)} could not be initialized because the {nameof(AlphaNumericSpecialCodec)} returned a null {nameof(DecodedResult<char[]>)}");

        if (result.CharCount != charLength)
        {
            throw new
                DataElementParsingException($"The Primitive Value {nameof(MerchantIdentifier)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
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