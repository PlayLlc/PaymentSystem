using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Valid cardholder account number
/// </summary>
public record ApplicationPan : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = NumericDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x5A;
    private const byte _MaxByteLength = 10;

    #endregion

    #region Constructor

    public ApplicationPan(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ApplicationPan Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationPan Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<BigInteger> result = _Codec.Decode(BerEncodingId, value).ToBigInteger()
            ?? throw new DataElementNullException(BerEncodingId);

        return new ApplicationPan(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(ApplicationPan? x, ApplicationPan? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationPan obj) => obj.GetHashCode();

    #endregion
}