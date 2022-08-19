using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     ICC PIN Encipherment Public Key certified by the issuer
/// </summary>
public record IccPinEnciphermentPublicKeyCertificate : DataElement<BigInteger>, IEqualityComparer<IccPinEnciphermentPublicKeyCertificate>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F2D;

    #endregion

    #region Constructor

    public IccPinEnciphermentPublicKeyCertificate(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public override IccPinEnciphermentPublicKeyCertificate Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    public static IccPinEnciphermentPublicKeyCertificate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IccPinEnciphermentPublicKeyCertificate Decode(ReadOnlySpan<byte> value) => new(PlayCodec.BinaryCodec.DecodeToBigInteger(value));

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IccPinEnciphermentPublicKeyCertificate? x, IccPinEnciphermentPublicKeyCertificate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccPinEnciphermentPublicKeyCertificate obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value.ToByteArray();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public int GetByteCount() => _Value.GetByteCount();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}