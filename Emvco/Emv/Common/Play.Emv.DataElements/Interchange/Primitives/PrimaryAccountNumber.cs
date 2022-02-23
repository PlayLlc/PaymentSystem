using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.DataElements.Emv;
using Play.Emv.DataElements.Exceptions;

using PlayEncodingId = Play.Ber.Codecs.PlayEncodingId;

namespace Play.Emv.DataElements.Interchange;

/// <summary>
///     The Account Number associated to Issuer Card
/// </summary>
public record PrimaryAccountNumber : InterchangeDataElement<char[]>
{
    #region Static Metadata

    /// <value>Hex: C2; Decimal: 194; Interchange: 2</value>
    public static readonly Tag Tag = 0xC2;

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const int _MaxByteLength = 10;

    #endregion

    #region Constructor

    public PrimaryAccountNumber(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <summary>
    ///     Checks whether the Issuer EncodingId provided in the argument matches the leftmost 3 - 8 PAN digits (allowing for
    ///     the possible padding of the Issuer EncodingId with hexadecimal 'F's)
    /// </summary>
    /// <param name="issuerIdentifier"></param>
    /// <returns></returns>
    public bool IsIssuerIdentifierMatching(IssuerIdentificationNumber issuerIdentifier)
    {
        // We're 
        uint thisPan = PlayCodec.CompressedNumericCodec.DecodeToUInt16(PlayCodec.CompressedNumericCodec.Encode(_Value));

        return thisPan == (uint) issuerIdentifier;
    }

    #endregion

    #region Serialization

    public static PrimaryAccountNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static PrimaryAccountNumber Decode(ReadOnlySpan<byte> value)
    {
        const byte maxCharLength = 19;

        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        // The reason we're decoding to a char array is because some Primary Account Numbers start with
        // a '0' value. If we encoded that to a numeric value we would truncate the leading zero values
        // and wouldn't be able to encode the value back or do comparisons with the issuer identification
        // number
        char[] result = PlayCodec.CompressedNumericCodec.DecodeToChars(value);

        Check.Primitive.ForMaxCharLength(result.Length, maxCharLength, Tag);

        return new PrimaryAccountNumber(result);
    }

    public new byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value);
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ReadOnlySpan<byte> value)
    {
        byte[] encoded = PlayCodec.CompressedNumericCodec.Encode(_Value);

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != encoded[i])
                return false;
        }

        return true;
    }

    #endregion
}