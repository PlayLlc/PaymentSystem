using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Data sent to the ICC for online issuer authentication
/// </summary>
public record IssuerAuthenticationData : DataElement<BigInteger>, IEqualityComparer<IssuerAuthenticationData>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x91;
    private const byte _MinByteLength = 8;
    private const byte _MaxByteLength = 16;

    #endregion

    #region Constructor

    public IssuerAuthenticationData(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    public static IssuerAuthenticationData Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerAuthenticationData Decode(ReadOnlySpan<byte> value, BerCodec codec) => Decode(value);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static IssuerAuthenticationData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override IssuerAuthenticationData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static IssuerAuthenticationData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new IssuerAuthenticationData(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);

    #endregion

    #region Equality

    public bool Equals(IssuerAuthenticationData? x, IssuerAuthenticationData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerAuthenticationData obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => PlayCodec.BinaryCodec.GetByteCount(_Value);

    public override ushort GetValueByteCount() => PlayCodec.BinaryCodec.GetByteCount(_Value);

    #endregion
}