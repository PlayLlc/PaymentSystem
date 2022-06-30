﻿using System;
using System.Linq;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Lengths.Long;
using Play.Ber.Lengths.Short;
using Play.Codecs;
using Play.Core.Extensions;

namespace Play.Ber.Lengths;

public readonly struct Length
{
    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    /// <summary>The argument represents the number of bytes in the Content Octets</summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    internal Length(uint value)
    {
        if (value <= ShortLength.MaxValue)
        {
            _Value = value;

            return;
        }

        Span<byte> encodedContentOctets = LongLength.Serialize((ushort) value);
        LongLength.Validate(encodedContentOctets[..LongLength.GetByteCount(encodedContentOctets)]);
        _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(encodedContentOctets[..LongLength.GetByteCount(encodedContentOctets)]);
    }

    #endregion

    #region Instance Members

    /// <summary>Gets the number of bytes that this Length object is composed of</summary>
    /// <exception cref="InvalidOperationException">Ignore.</exception>
    public readonly byte GetByteCount()
    {
        if (ShortLength.IsValid(_Value))
            return 1;

        return LongLength.GetByteCount(_Value);
    }

    /// <summary>Gets the number of bytes that this Length object is composed of</summary>
    internal static byte GetByteCount(PrimitiveValue value, BerCodec codec)
    {
        ushort contentOctetByteCount = value.GetValueByteCount(codec);

        return (byte) (ShortLength.IsValid(contentOctetByteCount) ? 1 : (byte) (contentOctetByteCount.GetMostSignificantByte() + 1));
    }

    /// <summary>
    ///     The byte count of the TLV object's Contents in the Value field
    /// </summary>
    public readonly ushort GetContentLength()
    {
        if (ShortLength.TryGetContentLength(_Value, out ushort result))
            return result;

        return LongLength.GetContentLength(_Value);
    }

    public override string ToString() =>
        $"Hex: {PlayCodec.HexadecimalCodec.DecodeToString(Serialize())}; Binary: {PlayCodec.BinaryCodec.DecodeToString(_Value)}";

    /// <summary>
    ///     Parses a raw BER encoded Length sequence into a Length object
    /// </summary>
    /// <param name="berLength"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="BerParsingException"></exception>
    internal static Length Parse(ReadOnlySpan<byte> berLength)
    {
        if (berLength.IsEmpty)
            return new Length(0);

        if (ShortLength.IsValid(berLength[0]))
            return new Length(berLength[0]);

        LongLength.Validate(berLength[..LongLength.GetByteCount(berLength)]);

        var byteCount = LongLength.GetByteCount(berLength);
        var hello = PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(berLength[1..byteCount]);

        return new Length(hello);
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     Serializes this Length object into the appropriate BER encoding for a Length
    /// </summary>
    /// <returns></returns>
    public byte[] Serialize() => BitConverter.GetBytes(_Value)[..GetByteCount()].Reverse().ToArray();

    #endregion

    #region Equality

    public override bool Equals(object? other) => other is Length length && Equals(length);
    public bool Equals(Length other) => _Value == other._Value;
    public int GetHashCode(Length obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
        const int largePrime = 486187739;

        unchecked
        {
            return largePrime + (3 * (int) _Value);
        }
    }

    public int CompareTo(Length other)
    {
        if (_Value > other._Value)
            return 1;

        if (_Value < other._Value)
            return -1;

        return 0;
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Length left, Length right) => left.Equals(right);
    public static bool operator ==(Length left, byte right) => left._Value == right;
    public static implicit operator uint(Length length) => length._Value;
    public static bool operator !=(Length left, Length right) => !left.Equals(right);
    public static bool operator !=(Length left, byte right) => left._Value != right;

    #endregion
}