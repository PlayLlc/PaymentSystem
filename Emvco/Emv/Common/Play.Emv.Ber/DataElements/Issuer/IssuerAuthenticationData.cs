using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
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
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerAuthenticationData Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerAuthenticationData Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort minByteLength = 8;
        const ushort maxByteLength = 16;

        if (value.Length is not >= minByteLength and <= maxByteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(IssuerAuthenticationData)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<BigInteger> result = codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new
                InvalidOperationException($"The {nameof(IssuerAuthenticationData)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IssuerAuthenticationData(result.Value);
    }

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IssuerAuthenticationData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IssuerAuthenticationData Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new IssuerAuthenticationData(result);
    }

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
}