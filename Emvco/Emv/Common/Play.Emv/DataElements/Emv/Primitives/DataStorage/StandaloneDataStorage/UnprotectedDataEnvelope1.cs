using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Description: The Unprotected Data Envelopes contain proprietary information from the issuer, payment system or
///     third
///     party. Unprotected Data Envelopes can be retrieved with the GET DATA command and can be updated with the PUT DATA
///     (CLA='80') command without secure messaging.
/// </summary>
public record UnprotectedDataEnvelope1 : DataElement<BigInteger>, IEqualityComparer<UnprotectedDataEnvelope1>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F75;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;

    #endregion

    #region Constructor

    public UnprotectedDataEnvelope1(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public static UnprotectedDataEnvelope1 Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static UnprotectedDataEnvelope1 Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort maxByteLength = 192;

        if (value.Length > maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(UnprotectedDataEnvelope1)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {maxByteLength} bytes in length");
        }

        DecodedResult<BigInteger> result = codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(UnprotectedDataEnvelope1)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new UnprotectedDataEnvelope1(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(UnprotectedDataEnvelope1? x, UnprotectedDataEnvelope1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(UnprotectedDataEnvelope1 obj) => obj.GetHashCode();

    #endregion
}