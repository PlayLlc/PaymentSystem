using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains a binary number that indicates which of the application's certification authority public keys and its
///     associated algorithm is to be used
/// </summary>
public record CaPublicKeyIndex : DataElement<byte>, IEqualityComparer<CaPublicKeyIndex>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly CaPublicKeyIndex Five;
    public static readonly CaPublicKeyIndex Four;
    public static readonly CaPublicKeyIndex One;
    public static readonly CaPublicKeyIndex Six;
    public static readonly CaPublicKeyIndex Three;
    public static readonly CaPublicKeyIndex Two;
    public static readonly Tag Tag = 0x8F;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    static CaPublicKeyIndex()
    {
        const byte one = 1;
        const byte two = 2;
        const byte three = 3;
        const byte four = 4;
        const byte five = 5;
        const byte six = 6;

        One = new CaPublicKeyIndex(one);
        Two = new CaPublicKeyIndex(two);
        Three = new CaPublicKeyIndex(three);
        Four = new CaPublicKeyIndex(four);
        Five = new CaPublicKeyIndex(five);
        Six = new CaPublicKeyIndex(six);
    }

    public CaPublicKeyIndex(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static CaPublicKeyIndex Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static CaPublicKeyIndex Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new CaPublicKeyIndex(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(CaPublicKeyIndex? x, CaPublicKeyIndex? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CaPublicKeyIndex obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CaPublicKeyIndex value) => value._Value;
    public static explicit operator CaPublicKeyIndex(byte value) => new(value);

    #endregion
}