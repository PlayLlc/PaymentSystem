using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

using BinaryCodec = Play.Emv.Ber.Codecs.BinaryCodec;

namespace Play.Emv.Security.Cryptograms;

/// <summary>
///     Result of a hash function specified in Book 2, Annex B3.1
/// </summary>
public record TransactionCertificateHashValue : PrimitiveValue, IEqualityComparer<TransactionCertificateHashValue>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x98;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public TransactionCertificateHashValue(BigInteger value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TransactionCertificateHashValue Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TransactionCertificateHashValue Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 20;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionCertificateHashValue)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<BigInteger> result = codec.Decode(PlayEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(TransactionCertificateHashValue)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new TransactionCertificateHashValue(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(PlayEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(PlayEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(TransactionCertificateHashValue? x, TransactionCertificateHashValue? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionCertificateHashValue obj) => obj.GetHashCode();

    #endregion
}