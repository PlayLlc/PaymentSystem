using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace ___TEMP.Play.Emv.Security.Certificates.Chip.Pin;

/// <summary>
///     Remaining digits of the ICC PIN Encipherment Public Key Modulus
/// </summary>
public record IccPinEnciphermentPublicKeyRemainder : PrimitiveValue, IEqualityComparer<IccPinEnciphermentPublicKeyRemainder>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F2F;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public IccPinEnciphermentPublicKeyRemainder(BigInteger value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return codec.GetByteCount(GetBerEncodingId(), _Value);
    }

    #endregion

    #region Serialization

    public static IccPinEnciphermentPublicKeyRemainder Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IccPinEnciphermentPublicKeyRemainder Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<BigInteger> result = codec.Decode(BerEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new
                InvalidOperationException($"The {nameof(IccPinEnciphermentPublicKeyRemainder)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IccPinEnciphermentPublicKeyRemainder(result.Value);
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

    public bool Equals(IccPinEnciphermentPublicKeyRemainder? x, IccPinEnciphermentPublicKeyRemainder? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccPinEnciphermentPublicKeyRemainder obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}