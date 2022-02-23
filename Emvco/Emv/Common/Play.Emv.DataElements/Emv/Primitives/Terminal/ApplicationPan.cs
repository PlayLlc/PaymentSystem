using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Valid cardholder account number
/// </summary>
public record ApplicationPan : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x5A;
    private const byte _MaxByteLength = 10;

    #endregion

    #region Constructor

    public ApplicationPan(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ApplicationPan Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationPan Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value).ToBigInteger()
            ?? throw new DataElementNullException(PlayEncodingId);

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