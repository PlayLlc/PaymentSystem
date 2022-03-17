using System;
using System.Numerics;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
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
    private const byte _MaxCharLength = 19;

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

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationPan Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationPan Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.NumericCodec.DecodeToBigInteger(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _MaxCharLength, Tag);

        return new ApplicationPan(result);
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