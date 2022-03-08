using System;

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

    /// <summary>
    ///     Takes a sequence of content octets and creates a Length object
    /// </summary>
    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    internal Length(ReadOnlySpan<byte> contentOctets)
    {
        Span<byte> encodedContentOctets = Serialize(contentOctets);

        if (ShortLength.IsValid(encodedContentOctets[0]))
        {
            _Value = encodedContentOctets[0];

            return;
        }

        LongLength.Validate(encodedContentOctets[..LongLength.GetByteCount(encodedContentOctets)]);

        _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(encodedContentOctets[..LongLength.GetByteCount(encodedContentOctets)]);
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    internal Length(uint value)
    {
        if (ShortLength.IsValid((byte) value))
        {
            _Value = (byte) value;

            return;
        }

        LongLength.Validate(value);

        _Value = value;
    }

    #endregion

    #region Instance Members

    public int CompareTo(Length other)
    {
        if (_Value > other._Value)
            return 1;

        if (_Value < other._Value)
            return -1;

        return 0;
    }

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    public readonly byte GetByteCount()
    {
        if (ShortLength.IsValid(_Value))
            return 1;

        return LongLength.GetByteCount(_Value);
    }

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
    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    internal static Length Parse(ReadOnlySpan<byte> berLength)
    {
        if (berLength.Length == 0)
        {
            throw new BerFormatException(new ArgumentOutOfRangeException(nameof(berLength),
                $"A {nameof(Length)} object cannot be initialized with an empty {nameof(berLength)} argument "));
        }

        if (ShortLength.IsValid(berLength[0]))
            return new Length(berLength[0]);

        LongLength.Validate(berLength[..LongLength.GetByteCount(berLength)]);

        return new Length(PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(berLength[..LongLength.GetByteCount(berLength)]));
    }

    #endregion

    #region Serialization

    public byte[] Serialize() => BitConverter.GetBytes(_Value)[..GetByteCount()];

    internal static byte[] Serialize(PrimitiveValue value, BerCodec codec)
    {
        ushort contentOctetByteCount = value.GetValueByteCount(codec);

        if (ShortLength.IsValid(contentOctetByteCount))
            return new[] {(byte) contentOctetByteCount};

        return LongLength.Serialize(contentOctetByteCount);
    }

    /// <summary>
    ///     Takes a sequence of content octets and creates an encoded Length sequence
    /// </summary>
    /// <param name="contentOctets"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="BerFormatException"></exception>
    internal static byte[] Serialize(ReadOnlySpan<byte> contentOctets)
    {
        if (contentOctets.Length > LongLength.MaxLengthSupported)
        {
            throw new BerFormatException(new ArgumentOutOfRangeException(nameof(contentOctets),
                $"This code base supports a TLV with a maximum Length field with {LongLength.MaxLengthSupported} bytes"));
        }

        if (ShortLength.IsValid(contentOctets.Length))
            return new[] {(byte) contentOctets.Length};

        return LongLength.Serialize((ushort) contentOctets.Length);
    }

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

    #endregion

    #region Operator Overrides

    public static bool operator ==(Length left, Length right) => left.Equals(right);
    public static bool operator ==(Length left, byte right) => left._Value == right;
    public static implicit operator uint(Length length) => length._Value;
    public static bool operator !=(Length left, Length right) => !left.Equals(right);
    public static bool operator !=(Length left, byte right) => left._Value != right;

    #endregion
}