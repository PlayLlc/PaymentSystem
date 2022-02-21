using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Description: The Protected Data Envelopes contain proprietary information from the issuer, payment system or third
///     party. The Protected Data Envelope can be retrieved with the GET DATA command. Updating the Protected Data Envelope
///     with the PUT DATA command requires secure messaging and is outside the scope of this specification.
/// </summary>
public record ProtectedDataEnvelope4 : DataElement<BigInteger>, IEqualityComparer<ProtectedDataEnvelope4>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F73;
    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;

    #endregion

    #region Constructor

    public ProtectedDataEnvelope4(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;

    #endregion

    #region Serialization

    public static ProtectedDataEnvelope4 Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ProtectedDataEnvelope4 Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort maxByteLength = 192;

        if (value.Length > maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ProtectedDataEnvelope4)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {maxByteLength} bytes in length");
        }

        DecodedResult<BigInteger> result = codec.Decode(BerEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(ProtectedDataEnvelope4)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new ProtectedDataEnvelope4(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(ProtectedDataEnvelope4? x, ProtectedDataEnvelope4? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ProtectedDataEnvelope4 obj) => obj.GetHashCode();

    #endregion
}