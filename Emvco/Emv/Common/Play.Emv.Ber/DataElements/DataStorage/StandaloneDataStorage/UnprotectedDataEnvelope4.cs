using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
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
public record UnprotectedDataEnvelope4 : DataElement<BigInteger>, IEqualityComparer<UnprotectedDataEnvelope4>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F78;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _MaxByteLength = 192;

    #endregion

    #region Constructor

    public UnprotectedDataEnvelope4(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static bool TryDecoding(TagLengthValue value, out UnprotectedDataEnvelope4? result)
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

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static UnprotectedDataEnvelope4 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override UnprotectedDataEnvelope4 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static UnprotectedDataEnvelope4 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new UnprotectedDataEnvelope4(result);
    }

    #endregion

    #region Equality

    public bool Equals(UnprotectedDataEnvelope4? x, UnprotectedDataEnvelope4? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(UnprotectedDataEnvelope4 obj) => obj.GetHashCode();

    #endregion
}