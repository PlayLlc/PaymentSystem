using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv.Primitives.DataStorage.StandaloneDataStorage;

/// <summary>
///     Description: The Unprotected Data Envelopes contain proprietary information from the issuer, payment system or
///     third
///     party. Unprotected Data Envelopes can be retrieved with the GET DATA command and can be updated with the PUT DATA
///     (CLA='80') command without secure messaging.
/// </summary>
public record UnprotectedDataEnvelope2 : DataElement<BigInteger>, IEqualityComparer<UnprotectedDataEnvelope2>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F76;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;

    #endregion

    #region Constructor

    public UnprotectedDataEnvelope2(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public static UnprotectedDataEnvelope2 Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static UnprotectedDataEnvelope2 Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort maxByteLength = 192;

        if (value.Length > maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(UnprotectedDataEnvelope2)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {maxByteLength} bytes in length");
        }

        DecodedResult<BigInteger> result = codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(UnprotectedDataEnvelope2)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new UnprotectedDataEnvelope2(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(UnprotectedDataEnvelope2? x, UnprotectedDataEnvelope2? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(UnprotectedDataEnvelope2 obj) => obj.GetHashCode();

    #endregion
}