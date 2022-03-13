using System;
using System.Collections.Generic;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;
using Play.Globalization.Time;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the time that the field is to be turned off after the transaction is completed if requested to do so by
///     the  cardholder device
/// </summary>
public record HoldTimeValue : DataElement<Deciseconds>, IEqualityComparer<HoldTimeValue>
{
    #region Static Metadata

    private static readonly Deciseconds _MinimumValue = new(0);
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8130;
    private const byte _ByteLength = 3;
    private const byte _CharLength = 6;

    #endregion

    #region Constructor

    /// <param name="value">
    ///     Minimum Value: 100 ms
    /// </param>
    /// <exception cref="DataElementParsingException"></exception>
    public HoldTimeValue(Deciseconds value) : base(value)
    {
        if (value < _MinimumValue)
        {
            throw new DataElementParsingException(new ArgumentOutOfRangeException(nameof(value),
                                                                                  $"The argument {nameof(value)} must be at least 100 ms to initialize a {nameof(HoldTimeValue)}"));
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
    public static HoldTimeValue Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static HoldTimeValue Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new HoldTimeValue(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(HoldTimeValue? x, HoldTimeValue? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(HoldTimeValue obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator Deciseconds(HoldTimeValue value) => value._Value;

    #endregion
}