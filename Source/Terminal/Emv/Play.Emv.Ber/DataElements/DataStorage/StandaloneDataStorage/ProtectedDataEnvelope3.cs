using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: The Protected Data Envelopes contain proprietary information from the issuer, payment system or third
///     party. The Protected Data Envelope can be retrieved with the GET DATA command. Updating the Protected Data Envelope
///     with the PUT DATA command requires secure messaging and is outside the scope of this specification.
/// </summary>
public record ProtectedDataEnvelope3 : DataElement<BigInteger>, IEqualityComparer<ProtectedDataEnvelope3>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F72;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _MaxByteLength = 192;

    #endregion

    #region Constructor

    public ProtectedDataEnvelope3(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ProtectedDataEnvelope3 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ProtectedDataEnvelope3 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ProtectedDataEnvelope3 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new ProtectedDataEnvelope3(result);
    }

    #endregion

    #region Equality

    public bool Equals(ProtectedDataEnvelope3? x, ProtectedDataEnvelope3? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ProtectedDataEnvelope3 obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}