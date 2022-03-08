using System;
using System.Numerics;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

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

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions._Temp.BerFormatException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="DataObjectParsingException"></exception>
    public static ApplicationPan Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value).ToBigInteger()
            ?? throw new DataObjectParsingException(EncodingId);

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