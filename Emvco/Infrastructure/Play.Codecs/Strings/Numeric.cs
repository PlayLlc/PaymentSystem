﻿using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs.Strings;

/// <summary>
///     NumericCodec data elements consist of two numeric digits (having values in the range Hex '0' – '9') per byte.
///     These digits are right justified and padded with leading hexadecimal zeroes. Other specifications sometimes
///     refer to this data format as BinaryCodec Coded Decimal (“BCD”) or unsigned packed.
///     Example: Amount, Authorized(NumericCodec) is defined as “n 12” with a length of six bytes.
///     A value of 12345 is stored in Amount, Authorized (NumericCodec) as Hex '00 00 00 01 23 45'.
/// </summary>
public class NumericCodec : PlayEncoding
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = new(typeof(NumericCodec));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public bool IsValid(byte value) => IsNibbleValid((byte) (value >> 4)) && IsNibbleValid(value.GetMaskedValue(0xF0));

    public bool IsValid(char value)
    {
        const char zeroDigit = '0';
        const char nineDigit = '9';

        return value is >= zeroDigit and <= nineDigit;
    }

    private static bool IsNibbleValid(byte value) => value is >= 0 and <= 9;

    public new byte[] Encode(string value)
    {
        if ((value.Length % 2) != 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        int length = value.Length / 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[length];

            for (int i = 0, j = 0; i < length; i++, j += 2)
                buffer[i] = GetByte(value[j], value[j + 1]);

            return buffer.ToArray();
        }
        else
        {
            SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < length; i++, j += 2)
                buffer[i] = GetByte(value[j], value[j + 1]);

            return buffer.ToArray();
        }
    }

    public byte[] Encode<T>(T value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value));
    }

    public byte[] Encode<T>(T value, int length)
    {
        if (length == Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (length == Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (length == 3)
            return Encode(Unsafe.As<T, uint>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (length < Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value), length);
    }

    public byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value));

        throw new NotImplementedException();
    }

    public byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value), length);

        throw new NotImplementedException();
    }

    public override int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        ReadOnlySpan<byte> buffer = Encode(chars[charIndex..(charIndex + charCount)]);

        for (int i = 0, j = byteIndex; i < (buffer.Length + byteIndex); i++, j++)
            bytes[j] = buffer[i];

        return buffer.Length;
    }

    public override byte[] Encode(ReadOnlySpan<char> value) => Encode(value, value.Length);

    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        int byteSize = (value.Length / 2) + (value.Length % 2);

        if (byteSize == length)
            return HexadecimalCodec.Encode(value);

        if (byteSize > length)
            return HexadecimalCodec.Encode(value)[^length..];

        byte[] result = new byte[length];

        Array.ConstrainedCopy(HexadecimalCodec.Encode(value), 0, result, length - byteSize, byteSize);

        return result;
    }

    public byte[] Encode(byte value)
    {
        return new[] {value};
    }

    public byte[] Encode(ushort value)
    {
        const byte byteSize = Specs.Integer.UInt16.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = (numberOfDigits / 2) + (numberOfDigits % 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = byteSize - numberOfBytes, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(uint value)
    {
        const byte byteSize = Specs.Integer.UInt32.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = (numberOfDigits / 2) + (numberOfDigits % 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = byteSize - numberOfBytes, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(uint value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = (numberOfDigits / 2) + (numberOfDigits % 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
        Span<byte> buffer = spanOwner.Span;

        for (int i = length - numberOfBytes, j = (numberOfBytes * 2) - 1; i < length; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(ulong value)
    {
        const byte byteSize = Specs.Integer.UInt64.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = (numberOfDigits / 2) + (numberOfDigits % 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = byteSize - numberOfBytes, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] Encode(ulong value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = (numberOfDigits / 2) + (numberOfDigits % 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
        Span<byte> buffer = spanOwner.Span;

        for (int i = length - numberOfBytes, j = (numberOfBytes * 2) - 1; i < length; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    // TODO: This is likely wrong
    public byte[] Encode(BigInteger value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((numberOfDigits % 2) + (numberOfDigits / 2));
        Span<byte> buffer = spanOwner.Span;

        for (int i = (buffer.Length * 2) - numberOfDigits, j = numberOfDigits - 1; i < (buffer.Length * 2); i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        return buffer.ToArray();
    }

    // TODO: This is likely wrong
    public byte[] Encode(BigInteger value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((numberOfDigits % 2) + (numberOfDigits / 2));
        Span<byte> buffer = spanOwner.Span;
        int numberOfBytes = (numberOfDigits / 2) + (numberOfDigits % 2);

        if (length == numberOfBytes)
            return Encode(value);

        if (length > numberOfBytes)
        {
            for (int i = length - numberOfBytes, j = numberOfDigits - 1; i < length; i++, j--)
            {
                if ((i % 2) == 0)
                    buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
                else
                    buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
            }

            return buffer.ToArray();
        }

        for (int i = length - numberOfBytes, j = numberOfDigits - (numberOfDigits - (length * 2)) - 1; i < length; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        return buffer.ToArray();
    }

    private static byte GetByte(char leftChar, char rightChar)
    {
        byte result = _ByteMap[leftChar];
        result *= 10;
        result += _ByteMap[rightChar];

        return result;
    }

    public byte GetByte(byte value)
    {
        int leftNibble = value >> 4;
        byte rightNibble = (byte) (value & ~0xF0);

        return (byte) ((leftNibble * 10) + rightNibble);
    }

    public override bool TryEncoding(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = HexadecimalCodec.Encode(value);

        return true;
    }

    public override int GetByteCount(char[] chars, int index, int count) => GetMaxByteCount(count);
    public override int GetMaxByteCount(int charCount) => charCount / 2;

    public override int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                DecodeToChars(value[i], buffer, j);

            string? a = buffer.ToArray().ToString();
            string? b = new(buffer);
            string? c = new(buffer.ToArray());

            return buffer.ToArray();
        }
        else
        {
            SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                DecodeToChars(value[i], buffer, j);

            return buffer.ToArray();
        }
    }

    private void DecodeToChars(byte value, Span<char> buffer, int offset)
    {
        int leftNibble = value / 10;
        int rightNibble = value % 10;

        buffer[offset++] = _CharMap[(byte) (value / 10)];
        buffer[offset] = _CharMap[(byte) (value % 10)];
    }

    public override int GetCharCount(byte[] bytes, int index, int count) => GetMaxCharCount(count);
    public override int GetMaxCharCount(int byteCount) => byteCount * 2;
    public override string DecodeToString(ReadOnlySpan<byte> value) => new(DecodeToChars(value));

    public override bool TryDecodingToString(ReadOnlySpan<byte> value, out string result)
    {
        if (!IsValid(value))
        {
            result = string.Empty;

            return false;
        }

        result = DecodeToString(value);

        return true;
    }

    public BigInteger DecodeToBigInteger(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        BigInteger result = 0;

        return BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the NumericCodec encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ushort DecodeToUInt16(ReadOnlySpan<byte> value)
    {
        ushort result = 0;

        return (ushort) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the NumericCodec encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint DecodeToUInt32(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new ArgumentOutOfRangeException(nameof(value));

        uint result = 0;

        return (uint) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the NumericCodec encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ulong DecodeToUInt64(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        ulong result = 0;

        return (ulong) BuildInteger(result, value);
    }

    private dynamic BuildInteger(dynamic resultBuffer, ReadOnlySpan<byte> value)
    {
        if (resultBuffer != byte.MinValue)
            resultBuffer = 0;

        for (int i = 0, j = (value.Length * 2) - 2; i < value.Length; i++, j -= 2)
            resultBuffer += GetByte(value[i]) * Math.Pow(10, j);

        return resultBuffer;
    }

    #endregion
}