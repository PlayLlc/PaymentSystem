using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;
using Play.Encryption.Certificates;

namespace Play.Emv.DataElements;

/// <summary>
///     Remaining digits of the ICC Public Key Modulus
/// </summary>
public record IccPublicKeyRemainder : DataElement<BigInteger>, IEqualityComparer<IccPublicKeyRemainder>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F48;

    #endregion

    #region Constructor

    public IccPublicKeyRemainder(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public PublicKeyRemainder AsPublicKeyRemainder() => new(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public ushort GetByteCount() => (ushort) _Value.GetByteCount();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => GetByteCount();

    #endregion

    #region Serialization

    public static IccPublicKeyRemainder Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IccPublicKeyRemainder Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<BigInteger> result = codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new DataElementParsingException(
                $"The {nameof(IccPublicKeyRemainder)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<byte[]>)}");

        return new IccPublicKeyRemainder(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IccPublicKeyRemainder? x, IccPublicKeyRemainder? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccPublicKeyRemainder obj) => obj.GetHashCode();

    #endregion
}