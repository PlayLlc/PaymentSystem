using System.Numerics;

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
public record ProtectedDataEnvelope2 : DataElement<BigInteger>, IEqualityComparer<ProtectedDataEnvelope2>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F71;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _MaxByteLength = 192;

    #endregion

    #region Constructor

    public ProtectedDataEnvelope2(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ProtectedDataEnvelope2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ProtectedDataEnvelope2 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ProtectedDataEnvelope2 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new ProtectedDataEnvelope2(result);
    }

    #endregion

    #region Equality

    public bool Equals(ProtectedDataEnvelope2? x, ProtectedDataEnvelope2? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ProtectedDataEnvelope2 obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}