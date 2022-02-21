using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.Codecs;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Interchange;

/// <summary>
///     The Account Number associated to Issuer Card
/// </summary>
public record PrimaryAccountNumber : InterchangeDataElement<char[]>
{
    #region Static Metadata

    /// <value>Hex: C2; Decimal: 194; Interchange: 2</value>
    public static readonly Tag Tag = 0xC2;

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    private const int _MaxByteLength = 10;

    #endregion

    #region Constructor

    public PrimaryAccountNumber(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    /// <summary>
    ///     Checks whether the Issuer Identifier provided in the argument matches the leftmost 3 - 8 PAN digits (allowing for
    ///     the possible padding of the Issuer Identifier with hexadecimal 'F's)
    /// </summary>
    /// <param name="issuerIdentifier"></param>
    /// <returns></returns>
    public bool IsIssuerIdentifierMatching(ReadOnlySpan<byte> issuerIdentifier)
    {
        byte[] encoded = PlayEncoding.CompressedNumeric.GetBytes(_Value);

        for (int i = 0; i < CompressedNumeric.GetPadCount(issuerIdentifier); i++)
        {
            if (issuerIdentifier[i] != encoded[i])
                return false;
        }

        return true;
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

        char[] result = PlayEncoding.CompressedNumeric.GetChars(value);

        Check.Primitive.ForMaxCharLength(result.Length, maxCharLength, Tag);

        return new PrimaryAccountNumber(result);
    }

    public new byte[] EncodeValue() => PlayEncoding.Numeric.GetBytes(_Value);
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ReadOnlySpan<byte> value)
    {
        byte[] encoded = PlayEncoding.CompressedNumeric.GetBytes(_Value);

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != encoded[i])
                return false;
        }

        return true;
    }

    #endregion
}