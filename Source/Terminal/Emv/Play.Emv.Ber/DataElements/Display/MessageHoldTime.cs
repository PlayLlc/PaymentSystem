using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Ber.DataElements.Display;

/// <summary>
///     Description: Indicates the default delay for the processing of the next MSG DataExchangeSignal. The Message Hold
///     Time is an integer in units of 100ms.
/// </summary>
public record MessageHoldTime : DataElement<Deciseconds>, IEqualityComparer<MessageHoldTime>
{
    #region Static Metadata

    private static readonly Deciseconds _MinimumValue = new(0);
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly MessageHoldTime MinimumValue = new(_MinimumValue);
    public static readonly Tag Tag = 0xDF812D;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    /// <param name="value">
    ///     Minimum Value: 100 ms
    /// </param>
    /// <exception cref="DataElementParsingException"></exception>
    public MessageHoldTime(Deciseconds value) : base(value)
    {
        if (_Value < _MinimumValue)
            throw new DataElementParsingException($"The argument {nameof(value)} must be at least 100 ms to initialize a {nameof(MessageHoldTime)}");
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public MessageHoldTime(Milliseconds value) : base(new Deciseconds(value))
    {
        if (_Value < _MinimumValue)
            throw new DataElementParsingException($"The argument {nameof(value)} must be at least 100 ms to initialize a {nameof(MessageHoldTime)}");
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MessageHoldTime Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MessageHoldTime Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MessageHoldTime Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new MessageHoldTime(new Deciseconds(result));
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(MessageHoldTime? x, MessageHoldTime? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(MessageHoldTime obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator Deciseconds(MessageHoldTime value) => value._Value;

    #endregion

    #region Instance Members

    public Milliseconds AsMilliseconds() => _Value;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    /// <summary>
    ///     The hold time in units of 100 ms
    /// </summary>
    public Deciseconds GetHoldTime() => _Value;

    public override Tag GetTag() => Tag;

    #endregion
}