using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Encryption.Certificates;

namespace Play.Emv.Ber.DataElements;

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

    #region Serialization

    public static IccPublicKeyRemainder Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override IccPublicKeyRemainder Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    public static IccPublicKeyRemainder Decode(ReadOnlySpan<byte> value)
    {
        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new IccPublicKeyRemainder(result);
    }
    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);

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

    #region Instance Members

    public PublicKeyRemainder AsPublicKeyRemainder() => new(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public ushort GetByteCount() => (ushort) _Value.GetByteCount();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => GetByteCount();

    #endregion
}