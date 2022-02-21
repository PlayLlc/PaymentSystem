using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs.Strings;

/// <summary>
///     Numeric data elements consist of two numeric digits (having values in the range Hex '0' – '9') per byte and a
///     leading byte that consists of either a 'C' for for a positive credit value or a 'D' for a negative debit value.
///     These digits are right justified and padded with leading hexadecimal zeroes
/// </summary>
public class SignedNumeric : PlayEncoding
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = new(nameof(SignedNumeric));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap = Enumerable.Range(0, 10)
        .Concat(new[] {67, 68}).ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(0, 10).Concat(new[] {67, 68}).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    private const char Positive = 'C';
    private const char Negative = 'D';

    #endregion

    #region Instance Members

    public PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

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
        if (_ByteMap.Keys.Contains(value))
            return true;

        return false;
    }

    private static bool IsNibbleValid(byte value)
    {
        if (_CharMap.Keys.Contains(value))
            return true;

        return false;
    }

    public new byte[] GetBytes(string value)
    {
        if (value.Length == 0)
            return Array.Empty<byte>();

        if ((value[0] != Positive) && (value[0] != Negative))
            throw new ArgumentOutOfRangeException(nameof(value));

        if (value.Length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[value.Length];

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
    }

    public byte[] GetBytes<T>(T value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.Int8.ByteCount)
            return GetBytes(Unsafe.As<T, sbyte>(ref value));
        if (byteSize == Specs.Integer.Int16.ByteCount)
            return GetBytes(Unsafe.As<T, short>(ref value));
        if (byteSize <= Specs.Integer.Int32.ByteCount)
            return GetBytes(Unsafe.As<T, int>(ref value));
        if (byteSize <= Specs.Integer.Int64.ByteCount)
            return GetBytes(Unsafe.As<T, long>(ref value));

        return GetBytes(Unsafe.As<T, BigInteger>(ref value));
    }

    public byte[] GetBytes<T>(T value, int length)
    {
        if (length == Specs.Integer.Int8.ByteCount)
            return GetBytes(Unsafe.As<T, sbyte>(ref value));
        if (length == Specs.Integer.Int16.ByteCount)
            return GetBytes(Unsafe.As<T, short>(ref value));
        if (length == 3)
            return GetBytes(Unsafe.As<T, int>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteCount)
            return GetBytes(Unsafe.As<T, int>(ref value));
        if (length < Specs.Integer.UInt64.ByteCount)
            return GetBytes(Unsafe.As<T, long>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteCount)
            return GetBytes(Unsafe.As<T, long>(ref value));

        return GetBytes(Unsafe.As<T, BigInteger>(ref value), length);
    }

    public byte[] GetBytes<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return GetBytes(Unsafe.As<T[], char[]>(ref value));
        if (typeof(T) == typeof(byte))
            return GetBytes(Unsafe.As<T[], byte[]>(ref value));

        throw new NotImplementedException();
    }

    public byte[] GetBytes<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return GetBytes(Unsafe.As<T[], char[]>(ref value), length);
        if (typeof(T) == typeof(byte))
            return GetBytes(Unsafe.As<T[], byte[]>(ref value));

        throw new NotImplementedException();
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        ReadOnlySpan<byte> buffer = GetBytes(chars[charIndex..(charIndex + charCount)]);

        for (int i = 0, j = byteIndex; i < (buffer.Length + byteIndex); i++, j++)
            bytes[j] = buffer[i];

        return buffer.Length;
    }

    public override byte[] GetBytes(ReadOnlySpan<char> value) => GetBytes(value, value.Length);

    public byte[] GetBytes(ReadOnlySpan<char> value, int length)
    {
        if (value.Length == 0)
            return Array.Empty<byte>();

        if ((value[0] != Positive) && (value[0] != Negative))
            throw new ArgumentOutOfRangeException(nameof(value));

        if (value.Length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[value.Length];

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
    }

    public byte[] GetBytes(byte value)
    {
        return new[] {value};
    }

    public byte[] GetBytes(short value)
    {
        const byte byteSize = Specs.Integer.Int16.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? Negative : Positive);

        for (int i = (byteSize - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(int value)
    {
        const byte byteSize = Specs.Integer.Int32.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? Negative : Positive);

        for (int i = (byteSize - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(int value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? Negative : Positive);

        for (int i = (length - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < length; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(long value)
    {
        const byte byteSize = Specs.Integer.Int64.ByteCount;
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? Negative : Positive);

        for (int i = (byteSize - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < byteSize; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(long value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? Negative : Positive);

        for (int i = (length - numberOfBytes) + 1, j = (numberOfBytes * 2) - 1; i < length; i++, j -= 2)
        {
            buffer[i] = (byte) ((value / Math.Pow(10, j - 1)) % 10);
            buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
        }

        return buffer.ToArray();
    }

    // TODO: This is likely wrong
    public byte[] GetBytes(BigInteger value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((numberOfDigits % 2) + (numberOfDigits / 2) + 1);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? Negative : Positive);

        for (int i = ((buffer.Length * 2) - numberOfDigits) + 1, j = numberOfDigits - 1; i < (buffer.Length * 2); i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        return buffer.ToArray();
    }

    // TODO: This is likely wrong
    public byte[] GetBytes(BigInteger value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        int numberOfBytes = numberOfDigits + 1;

        if (length == numberOfBytes)
            return GetBytes(value);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((numberOfDigits % 2) + (numberOfDigits / 2));
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = (byte) (value < 0 ? Negative : Positive);

        if (length > numberOfBytes)
        {
            for (int i = (length - numberOfBytes) + 1, j = numberOfDigits - 1; i < length; i++, j--)
            {
                if ((i % 2) == 0)
                    buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
                else
                    buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
            }

            return buffer.ToArray();
        }

        for (int i = (length - numberOfBytes) + 1, j = numberOfDigits - (numberOfDigits - (length * 2)) - 1; i < length; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        return buffer.ToArray();
    }

    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = GetBytes(value, value.Length);

        return true;
    }

    public override int GetByteCount(char[] chars, int index, int count) => count;
    public override int GetMaxByteCount(int charCount) => charCount;

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public char[] GetChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _CharMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _CharMap[value[i]];

            return buffer.ToArray();
        }
    }

    public override int GetCharCount(byte[] bytes, int index, int count) => count;
    public override int GetMaxCharCount(int byteCount) => byteCount;
    public override string GetString(ReadOnlySpan<byte> value) => new(GetChars(value));

    public override bool TryGetString(ReadOnlySpan<byte> value, out string result)
    {
        if (!IsValid(value))
        {
            result = string.Empty;

            return false;
        }

        result = GetString(value);

        return true;
    }

    public BigInteger GetBigInteger(ReadOnlySpan<byte> value)
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
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ushort GetInt16(ReadOnlySpan<byte> value)
    {
        ushort result = 0;

        return (ushort) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint GetInt32(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new ArgumentOutOfRangeException(nameof(value));

        uint result = 0;

        return (uint) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ulong GetInt64(ReadOnlySpan<byte> value)
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

        for (int i = 1, j = (value.Length * 2) - 2; i < value.Length; i++, j -= 2)
            resultBuffer += (byte) (value[i] * Math.Pow(10, j));

        resultBuffer *= value[0] == Negative ? -1 : 1;

        return resultBuffer;
    }

    #endregion
}