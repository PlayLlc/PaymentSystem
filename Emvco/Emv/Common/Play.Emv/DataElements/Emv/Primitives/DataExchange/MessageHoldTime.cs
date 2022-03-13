using System;
using System.Collections.Generic;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: Indicates the default delay for the processing of the next MSG DataExchangeSignal. The Message Hold
///     Time is an integer in units of 100ms.
/// </summary>
public record MessageHoldTime : DataElement<Milliseconds>, IEqualityComparer<MessageHoldTime>
{
    #region Static Metadata

    private static readonly Milliseconds _MinimumValue = new(100);
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
    public MessageHoldTime(Milliseconds value) : base(value)
    {
        if (value < _MinimumValue)
        {
            throw new DataElementParsingException(
                $"The argument {nameof(value)} must be at least 100 ms to initialize a {nameof(MessageHoldTime)}");
        }
    }

    #endregion

    #region Instance Members

    public Milliseconds AsMilliseconds() => _Value;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    /// <summary>
    ///     The hold time in units of 100 ms
    /// </summary>
    public ulong GetHoldTime() => (ulong) _Value / 100;

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

        return new MessageHoldTime(new Milliseconds(result * 100));
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
}