using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements.CertificateAuthority;

namespace ___TEMP.Play.Emv.Security.Certificates.Chip;

/// <summary>
///     Remaining digits of the ICC Public Key Modulus
/// </summary>
public record IccPublicKeyRemainder : PrimitiveValue, IEqualityComparer<IccPublicKeyRemainder>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F48;

    #endregion

    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public IccPublicKeyRemainder(ReadOnlySpan<byte> value)
    {
        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    public PublicKeyRemainder AsPublicKeyRemainder()
    {
        return new PublicKeyRemainder(_Value);
    }

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
    }

    public ushort GetByteCount()
    {
        return (ushort) _Value.Length;
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return GetByteCount();
    }

    #endregion

    #region Serialization

    public static IccPublicKeyRemainder Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IccPublicKeyRemainder Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<byte[]> result = codec.Decode(BerEncodingId, value) as DecodedResult<byte[]>
            ?? throw new
                InvalidOperationException($"The {nameof(IccPublicKeyRemainder)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<byte[]>)}");

        return new IccPublicKeyRemainder(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec)
    {
        return codec.EncodeValue(BerEncodingId, _Value);
    }

    public override byte[] EncodeValue(BerCodec codec, int length)
    {
        return codec.EncodeValue(BerEncodingId, _Value, length);
    }

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

    public int GetHashCode(IccPublicKeyRemainder obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}