using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: The CVC3 (Track2) is a 2-byte cryptogram returned by the Card in the response to the COMPUTE
///     CRYPTOGRAPHIC CHECKSUM command.
/// </summary>
public record CardholderVerificationCode3Track2 : DataElement<ushort>, IEqualityComparer<CardholderVerificationCode3Track2>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F61;

    #endregion
     

    #region Constructor

    public CardholderVerificationCode3Track2(ushort value) : base(value)
    { 
    }

    #endregion

    #region Instance Members

    public static bool EqualsStatic(CardholderVerificationCode3Track2? x, CardholderVerificationCode3Track2? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static CardholderVerificationCode3Track2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static CardholderVerificationCode3Track2 Decode(ReadOnlySpan<byte> value)
    {

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);
        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);
        return new CardholderVerificationCode3Track2(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    private const byte _ByteLength = 2;
    #endregion

    #region Equality

    public bool Equals(CardholderVerificationCode3Track2? x, CardholderVerificationCode3Track2? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CardholderVerificationCode3Track2 obj) => obj.GetHashCode();

    #endregion
}