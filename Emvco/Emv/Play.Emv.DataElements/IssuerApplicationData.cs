using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.DataElements;

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

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F10;

    #endregion

    #region Constructor

    public IssuerApplicationData(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

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

    public static IssuerApplicationData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerApplicationData Decode(ReadOnlySpan<byte> value)
    {
        const ushort maxByteLength = 32;

        if (value.Length > maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IssuerApplicationData)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {maxByteLength} bytes in length");
        }

        DecodedResult<BigInteger> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerApplicationData)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IssuerApplicationData(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerApplicationData? x, IssuerApplicationData? y) => EqualsStatic(x, y);
    public int GetHashCode(IssuerApplicationData obj) => obj.GetHashCode();

    #endregion
}