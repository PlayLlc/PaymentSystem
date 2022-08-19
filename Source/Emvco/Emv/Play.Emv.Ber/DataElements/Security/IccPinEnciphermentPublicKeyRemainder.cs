using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Remaining digits of the ICC PIN Encipherment Public Key Modulus
/// </summary>
public record IccPinEnciphermentPublicKeyRemainder : DataElement<BigInteger>, IEqualityComparer<IccPinEnciphermentPublicKeyRemainder>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F2F;

    #endregion

    #region Constructor

    public IccPinEnciphermentPublicKeyRemainder(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public override IccPinEnciphermentPublicKeyRemainder Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    public static IccPinEnciphermentPublicKeyRemainder Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IccPinEnciphermentPublicKeyRemainder Decode(ReadOnlySpan<byte> value)
    {
        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new IccPinEnciphermentPublicKeyRemainder(result);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

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

    public int GetHashCode(IccPinEnciphermentPublicKeyRemainder obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}