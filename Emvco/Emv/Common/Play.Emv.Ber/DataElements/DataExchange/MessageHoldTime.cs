using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Exceptions;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Ber.DataElements;

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
        {
            throw new
                DataElementParsingException($"The argument {nameof(value)} must be at least 100 ms to initialize a {nameof(MessageHoldTime)}");
        }
    }

    public MessageHoldTime(Milliseconds value) : base(new Deciseconds(value))
    {
        if (_Value < _MinimumValue)
        {
            throw new
                DataElementParsingException($"The argument {nameof(value)} must be at least 100 ms to initialize a {nameof(MessageHoldTime)}");
        }
    }

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

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MessageHoldTime Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MessageHoldTime Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.BinaryCodec.DecodeToUInt32(value);

        return new MessageHoldTime(new Deciseconds(result));
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

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
}