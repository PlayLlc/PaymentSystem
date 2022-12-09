﻿using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Defines the time in ms before the timer generates a TIMEOUT Signal.
/// </summary>
public record TimeoutValue : DataElement<Milliseconds>, IEqualityComparer<TimeoutValue>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8127;
    public static readonly TimeoutValue Default = new(0x01F4); //C-2-4.5.1

    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public TimeoutValue(Milliseconds value) : base((ushort) value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TimeoutValue Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TimeoutValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TimeoutValue Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new TimeoutValue(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode((ushort)_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode((ushort)_Value, length);

    #endregion

    #region Equality

    public bool Equals(TimeoutValue? x, TimeoutValue? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TimeoutValue obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator Milliseconds(TimeoutValue value) => new(value._Value);

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => PlayCodec.BinaryCodec.GetByteCount((ushort)_Value);

    public override ushort GetValueByteCount() => PlayCodec.BinaryCodec.GetByteCount((ushort)_Value);

    #endregion
}