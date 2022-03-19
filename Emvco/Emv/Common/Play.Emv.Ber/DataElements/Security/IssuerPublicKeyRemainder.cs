using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Encryption.Certificates;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Remaining digits of the Issuer Public Key Modulus
/// </summary>
public record IssuerPublicKeyRemainder : DataElement<BigInteger>, IEqualityComparer<IssuerPublicKeyRemainder>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x92;

    #endregion

    #region Constructor

    public IssuerPublicKeyRemainder(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public PublicKeyRemainder AsPublicKeyRemainder() => new(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public static IssuerPublicKeyRemainder Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override IssuerPublicKeyRemainder Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerPublicKeyRemainder Decode(ReadOnlySpan<byte> value)
    {
        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new IssuerPublicKeyRemainder(result);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerPublicKeyRemainder? x, IssuerPublicKeyRemainder? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerPublicKeyRemainder obj) => obj.GetHashCode();

    #endregion
}