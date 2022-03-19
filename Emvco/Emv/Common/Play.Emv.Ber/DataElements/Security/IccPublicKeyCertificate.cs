using System.Numerics;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     ICC Public Key certified by the issuer
/// </summary>
public record IccPublicKeyCertificate : DataElement<BigInteger>, IEqualityComparer<IccPublicKeyCertificate>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F46;
    private const byte _MaxByteLength = 248;

    #endregion

    #region Constructor

    public IccPublicKeyCertificate(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IccPublicKeyCertificate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IccPublicKeyCertificate Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new IccPublicKeyCertificate(result);
    }

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