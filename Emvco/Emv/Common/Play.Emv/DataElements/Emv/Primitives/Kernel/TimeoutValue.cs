using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.DataElements;

/// <summary>
///     Defines the time in ms before the timer generates a TIMEOUT Signal.
/// </summary>
public record TimeoutValue : DataElement<ushort>, IEqualityComparer<TimeoutValue>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8127;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public TimeoutValue(ushort value) : base(value)
    { }

    public TimeoutValue(Milliseconds value) : base((ushort) value)
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
    public static TimeoutValue Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TimeoutValue Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new TimeoutValue(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

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
}