using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains proprietary application data for transmission to the issuer in an online transaction. Note: For
///     CCD-compliant applications, Annex C, section C7 defines the specific coding of the Issuer Application Data
///     (IAD). To avoid potential conflicts with CCD-compliant applications, it is strongly recommended that the
///     IAD data element in an application that is not CCD-compliant should not use the coding for a CCD-compliant
///     application
/// </summary>
public record IssuerApplicationData : DataElement<BigInteger>, IEqualityComparer<IssuerApplicationData>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F10;
    private const byte _MaxByteLength = 32;

    #endregion

    #region Constructor

    public IssuerApplicationData(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public override ushort GetValueByteCount() => PlayCodec.BinaryCodec.GetByteCount(_Value);

    public static bool EqualsStatic(IssuerApplicationData? x, IssuerApplicationData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static IssuerApplicationData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override IssuerApplicationData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static IssuerApplicationData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new IssuerApplicationData(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);

    #endregion

    #region Equality

    public bool Equals(IssuerApplicationData? x, IssuerApplicationData? y) => EqualsStatic(x, y);
    public int GetHashCode(IssuerApplicationData obj) => obj.GetHashCode();

    #endregion
}