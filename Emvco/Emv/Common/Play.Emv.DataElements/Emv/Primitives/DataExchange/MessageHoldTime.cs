using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Description: Indicates the default delay for the processing of the next MSG DataExchangeSignal. The Message Hold
///     Time is an integer in units of 100ms.
/// </summary>
public record MessageHoldTime : DataElement<Milliseconds>, IEqualityComparer<MessageHoldTime>
{
    #region Static Metadata

    private static readonly Milliseconds _MinimumValue = new(100);
    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;
    public static readonly MessageHoldTime MinimumValue = new(_MinimumValue);
    public static readonly Tag Tag = 0xDF812D;

    #endregion

    #region Constructor

    /// <param name="value">
    ///     Minimum Value: 100 ms
    /// </param>
    public MessageHoldTime(Milliseconds value) : base(value)
    {
        if (value < _MinimumValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} must be at least 100 ms to initialize a {nameof(MessageHoldTime)}");
        }
    }

    #endregion

    #region Instance Members

    public Milliseconds AsMilliseconds() => _Value;
    public override PlayEncodingId GetEncodingId() => PlayEncodingId;

    /// <summary>
    ///     The hold time in units of 100 ms
    /// </summary>
    public ulong GetHoldTime() => (ulong) _Value / 100;

    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static MessageHoldTime Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    private static MessageHoldTime Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 3;
        const ushort charLength = 6;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(MessageHoldTime)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(PlayEncodingId, value) as DecodedResult<uint>
            ?? throw new DataElementNullException(PlayEncodingId);

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(MessageHoldTime)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new MessageHoldTime(new Milliseconds(result.Value * 100));
    }

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
}