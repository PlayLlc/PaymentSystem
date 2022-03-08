using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Emv.Security.Certificates.Icc;

/// <summary>
///     ICC Public Key certified by the issuer
/// </summary>
public record IccPublicKeyCertificate : PrimitiveValue, IEqualityComparer<IccPublicKeyCertificate>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F46;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public IccPublicKeyCertificate(BigInteger value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public ushort GetByteCount() => (ushort) _Value.GetByteCount();
    public ReadOnlySpan<byte> GetEncipherment() => _Value.ToByteArray().AsSpan();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => GetByteCount();

    #endregion

    #region Serialization

    public static IccPublicKeyCertificate Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IccPublicKeyCertificate Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<BigInteger> result = codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(IccPublicKeyCertificate)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IccPublicKeyCertificate(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IccPublicKeyCertificate? x, IccPublicKeyCertificate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccPublicKeyCertificate obj) => obj.GetHashCode();

    #endregion
}