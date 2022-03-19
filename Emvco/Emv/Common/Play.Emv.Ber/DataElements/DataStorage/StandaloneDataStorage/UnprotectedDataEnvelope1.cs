using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: The Unprotected Data Envelopes contain proprietary information from the issuer, payment system or
///     third
///     party. Unprotected Data Envelopes can be retrieved with the GET DATA command and can be updated with the PUT DATA
///     (CLA='80') command without secure messaging.
/// </summary>
public record UnprotectedDataEnvelope1 : DataElement<BigInteger>, IEqualityComparer<UnprotectedDataEnvelope1>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F75;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _MaxByteLength = 192;

    #endregion

    #region Constructor

    public UnprotectedDataEnvelope1(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static bool TryDecoding(TagLengthValue value, out UnprotectedDataEnvelope1? result)
    {
        if (value.GetTag() != Tag)
        {
            result = null;

            return false;
        }

        result = Decode(value.EncodeValue().AsSpan());

        return true;
    }

    #endregion

    #region Serialization

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static UnprotectedDataEnvelope1 Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort maxByteLength = 192;

        if (value.Length > maxByteLength)
        {
            throw new
                DataElementParsingException($"The Primitive Value {nameof(UnprotectedDataEnvelope1)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {maxByteLength} bytes in length");
        }

        DecodedResult<BigInteger> result = codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new
                DataElementParsingException($"The {nameof(UnprotectedDataEnvelope1)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new UnprotectedDataEnvelope1(result.Value);
    }

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static UnprotectedDataEnvelope1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static UnprotectedDataEnvelope1 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.NumericCodec.DecodeToBigInteger(value);

        return new UnprotectedDataEnvelope1(result);
    }

    #endregion

    #region Equality

    public bool Equals(UnprotectedDataEnvelope1? x, UnprotectedDataEnvelope1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(UnprotectedDataEnvelope1 obj) => obj.GetHashCode();

    #endregion
}