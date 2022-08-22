using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Encryption.Certificates;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Issuer public key exponent used for the verification of the Signed Static Application Data and the ICC Public Key
///     Certificate
/// </summary>
public record IssuerPublicKeyExponent : DataElement<uint>, IEqualityComparer<IssuerPublicKeyExponent>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F32;
    private const byte _MinByteLength = 1;
    private const byte _MaxByteLength = 3;

    #endregion

    #region Constructor

    public IssuerPublicKeyExponent(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => PlayCodec.UnsignedIntegerCodec.Encode(_Value);
    public PublicKeyExponents AsPublicKeyExponent() => PublicKeyExponents.Create(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public int GetByteCount() => _Value.GetMostSignificantBit();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public static IssuerPublicKeyExponent Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override IssuerPublicKeyExponent Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerPublicKeyExponent Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMinimumLength(value, _MaxByteLength, Tag);
        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new IssuerPublicKeyExponent(result);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerPublicKeyExponent? x, IssuerPublicKeyExponent? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerPublicKeyExponent obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator PublicKeyExponents(IssuerPublicKeyExponent value) => PublicKeyExponents.Create(value._Value);

    #endregion
}