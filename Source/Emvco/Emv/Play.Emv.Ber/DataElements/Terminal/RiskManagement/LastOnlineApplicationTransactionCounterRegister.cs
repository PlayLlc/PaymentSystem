using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements.Terminal.RiskManagement;

/// <summary>
///     ATC value of the last transaction that went online
/// </summary>
public record LastOnlineApplicationTransactionCounterRegister : DataElement<ushort>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F13;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public LastOnlineApplicationTransactionCounterRegister(ushort value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static LastOnlineApplicationTransactionCounterRegister Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override LastOnlineApplicationTransactionCounterRegister Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static LastOnlineApplicationTransactionCounterRegister Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new LastOnlineApplicationTransactionCounterRegister(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(LastOnlineApplicationTransactionCounterRegister? x, LastOnlineApplicationTransactionCounterRegister? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(LastOnlineApplicationTransactionCounterRegister obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(LastOnlineApplicationTransactionCounterRegister value) => value._Value;

    #endregion

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
}