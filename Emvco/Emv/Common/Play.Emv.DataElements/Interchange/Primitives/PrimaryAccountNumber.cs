using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
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

    public static readonly BerEncodingId BerEncodingId = NumericDataElementCodec.Identifier;
    private const int _MaxByteLength = 10;

    #endregion

    #region Constructor

    public PrimaryAccountNumber(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static PrimaryAccountNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static PrimaryAccountNumber Decode(ReadOnlySpan<byte> value)
    {
        const byte maxCharLength = 19;

        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        char[] result = PlayEncoding.Numeric.GetChars(value);

        Check.Primitive.ForMaxCharLength(result.Length, maxCharLength, Tag);

        return new PrimaryAccountNumber(result);
    }

    public new byte[] EncodeValue() => PlayEncoding.Numeric.GetBytes(_Value);
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

    #endregion
}