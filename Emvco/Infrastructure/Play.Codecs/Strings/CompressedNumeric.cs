using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs.Strings;

public class CompressedNumeric : PlayEncoding
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = new(nameof(CompressedNumeric));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    private const char _PaddingCharKey = 'F';
    private const byte _PaddedLeftNibble = 0xF0;
    private const byte _PaddedRightNibble = 0xF;
    private const byte _PaddedByte = 0xFF;

    #endregion

    #region Instance Members

    public PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    public override bool IsValid(ReadOnlySpan<char> value)
    {
        int padCount = GetPadCount(value);

        for (int i = padCount; i < value.Length; i++)
        {
            if (!_ByteMap.ContainsKey(value[i]))
                return false;
        }

        return true;
    }

    private static int GetPadCount(ReadOnlySpan<char> value)
    {
        int offset = 0;

        for (; offset < value.Length;)
        {
            if (value[offset] != _PaddingCharKey)
                break;

            offset++;
        }

        return offset;
    }

    private static int GetPadCount(ReadOnlySpan<byte> value)
    {
        int offset = value.Length;

        for (; offset > 0;)
        {
            if (value[offset] != _PaddedByte)
                break;

            offset -= 2;
        }

        if (value[offset].AreBitsSet(_PaddedRightNibble))
            offset++;

        return offset;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        int padCount = GetPadCount(value);

        for (int i = 0, j = 0; j < ((value.Length * 2) - padCount); i += j % 2, j++)
        {
            if ((padCount % 2) != 0)
            {
                if (!_CharMap.ContainsKey((byte) value[i].GetMaskedValue(_PaddedLeftNibble)))
                    return false;
            }
            else
            {
                if (!_CharMap.ContainsKey((byte) value[i].GetMaskedValue(_PaddedRightNibble)))
                    return false;
            }
        }

        return true;
    }

    public bool IsValid(byte value)
    {
        if (value == _PaddedByte)
            return true;

        if (!value.AreBitsSet(_PaddedLeftNibble))
        {
            if (!_CharMap.ContainsKey((byte) (value >> 4)))
                return false;
        }

        if (!_CharMap.ContainsKey(value.GetMaskedValue(_PaddedLeftNibble)))
            return false;

        return true;
    }

    public bool IsValid(char value) => _ByteMap.ContainsKey(value);

    public new byte[] GetBytes(string value)
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

    public byte[] GetBytes<T>(T value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteSize)
            return GetBytes(Unsafe.As<T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteSize)
            return GetBytes(Unsafe.As<T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteSize)
            return GetBytes(Unsafe.As<T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return GetBytes(Unsafe.As<T, ulong>(ref value));

        return GetBytes(Unsafe.As<T, BigInteger>(ref value));
    }

    public byte[] GetBytes<T>(T value, int length)
    {
        if (length == Specs.Integer.UInt8.ByteSize)
            return GetBytes(Unsafe.As<T, byte>(ref value));
        if (length == Specs.Integer.UInt16.ByteSize)
            return GetBytes(Unsafe.As<T, ushort>(ref value));
        if (length == 3)
            return GetBytes(Unsafe.As<T, uint>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteSize)
            return GetBytes(Unsafe.As<T, uint>(ref value));
        if (length < Specs.Integer.UInt64.ByteCount)
            return GetBytes(Unsafe.As<T, ulong>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteCount)
            return GetBytes(Unsafe.As<T, ulong>(ref value));

        return GetBytes(Unsafe.As<T, BigInteger>(ref value), length);
    }

    public byte[] GetBytes<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return GetBytes(Unsafe.As<T[], char[]>(ref value));

        throw new NotImplementedException();
    }

    public byte[] GetBytes<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return GetBytes(Unsafe.As<T[], char[]>(ref value), length);

        throw new NotImplementedException();
    }

    // //////////////////////////////START

    public void GetBytes<T>(T value, Span<byte> buffer, ref int offset)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteSize)
            GetBytes(Unsafe.As<T, byte>(ref value), buffer, ref offset);
        else if (byteSize == Specs.Integer.UInt16.ByteSize)
            GetBytes(Unsafe.As<T, ushort>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt32.ByteSize)
            GetBytes(Unsafe.As<T, uint>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt64.ByteCount)
            GetBytes(Unsafe.As<T, ulong>(ref value), buffer, ref offset);
        else
            GetBytes(Unsafe.As<T, BigInteger>(ref value), buffer, ref offset);
    }

    public void GetBytes<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        if (length == Specs.Integer.UInt8.ByteSize)
            GetBytes(Unsafe.As<T, byte>(ref value));
        else if (length == Specs.Integer.UInt16.ByteSize)
            GetBytes(Unsafe.As<T, ushort>(ref value));
        else if (length == 3)
            GetBytes(Unsafe.As<T, uint>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt32.ByteSize)
            GetBytes(Unsafe.As<T, uint>(ref value), buffer, ref offset);
        else if (length < Specs.Integer.UInt64.ByteCount)
            GetBytes(Unsafe.As<T, ulong>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt64.ByteCount)
            GetBytes(Unsafe.As<T, ulong>(ref value), buffer, ref offset);
        else
            GetBytes(Unsafe.As<T, BigInteger>(ref value), length, buffer, ref offset);
    }

    public byte[] GetBytes<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            return GetBytes(Unsafe.As<T[], char[]>(ref value));

        throw new NotImplementedException();
    }

    public byte[] GetBytes<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            return GetBytes(Unsafe.As<T[], char[]>(ref value), length);

        throw new NotImplementedException();
    }

    // //////////////////////////////END

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        ReadOnlySpan<byte> buffer = GetBytes(chars[charIndex..(charIndex + charCount)]);

        for (int i = 0, j = byteIndex; i < (buffer.Length + byteIndex); i++, j++)
            bytes[j] = buffer[i];

        return buffer.Length;
    }

    public override byte[] GetBytes(ReadOnlySpan<char> value) => GetBytes(value, value.Length);

    private int Pad(Span<byte> buffer)
    {
        int padCount = GetPadCount(buffer);
        buffer[^(padCount / 2)..].Fill(_PaddedByte);
        if ((padCount % 2) != 0)
            buffer[^((padCount / 2) + 1)] |= _PaddedRightNibble;

        return padCount;
    }

    public byte[] GetBytes(ReadOnlySpan<char> value, int length)
    {
        int byteSize = (value.Length / 2) + (value.Length % 2);

        if (byteSize > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
            Span<byte> result = spanOwner.Span;

            int padCount = Pad(result);

            for (int i = 0, j = 0; j < ((value.Length * 2) - padCount); i += j % 2, j++)
            {
                try
                {
                    if ((padCount % 2) == 0)
                        result[i] |= (byte) (_ByteMap[value[j]] << 4);
                    else
                        result[i] |= _ByteMap[value[j]];
                }
                catch (IndexOutOfRangeException exception)
                {
                    throw new EncodingException(
                        $"The value could not be encoded by {nameof(CompressedNumeric)} because there was an invalid character", exception);
                }
            }

            return result.ToArray();
        }
        else
        {
            Span<byte> result = stackalloc byte[byteSize];

            int padCount = Pad(result);

            for (int i = 0, j = 0; j < ((value.Length * 2) - padCount); i += j % 2, j++)
            {
                try
                {
                    if ((padCount % 2) == 0)
                        result[i] |= (byte) (_ByteMap[value[j]] << 4);
                    else
                        result[i] |= _ByteMap[value[j]];
                }
                catch (IndexOutOfRangeException exception)
                {
                    throw new EncodingException(
                        $"The value could not be encoded by {nameof(CompressedNumeric)} because there was an invalid character", exception);
                }
            }

            return result.ToArray();
        }
    }

    public void GetBytes(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        int byteSize = (length / 2) + (length % 2);

        int padCount = Pad(buffer[offset..]);

        for (int i = 0, j = 0; j < ((length * 2) - padCount); i += j % 2, j++)
        {
            try
            {
                if ((padCount % 2) == 0)
                    buffer[i] |= (byte) (_ByteMap[value[j]] << 4);
                else
                    buffer[i] |= _ByteMap[value[j]];
            }
            catch (IndexOutOfRangeException exception)
            {
                throw new EncodingException(
                    $"The value could not be encoded by {nameof(CompressedNumeric)} because there was an invalid character", exception);
            }
        }

        offset += byteSize;
    }

    public byte[] GetBytes(byte value)
    {
        return new[] {value};
    }

    public byte[] GetBytes(ushort value)
    {
        const byte byteSize = Specs.Integer.UInt16.ByteSize;
        int padCount = value.GetNumberOfDigits() - (byteSize * 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(uint value)
    {
        const byte byteSize = Specs.Integer.UInt32.ByteSize;
        int padCount = value.GetNumberOfDigits() - (byteSize * 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(uint value, int length)
    {
        const byte byteSize = Specs.Integer.UInt32.ByteSize;

        if (length > (byteSize * 2))
            throw new Exception();

        int padCount = (byteSize * 2) - value.GetNumberOfDigits();

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(ulong value)
    {
        const byte byteSize = Specs.Integer.UInt64.ByteCount;
        int padCount = value.GetNumberOfDigits() - (byteSize * 2);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(ulong value, int length)
    {
        const byte byteSize = Specs.Integer.UInt64.ByteCount;

        if (length > (byteSize * 2))
            throw new Exception();

        int padCount = (byteSize * 2) - value.GetNumberOfDigits();

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(BigInteger value)
    {
        int numberOfDigits = value.GetNumberOfDigits();
        int byteSize = (numberOfDigits / 2) + (numberOfDigits % 2);
        int padCount = numberOfDigits % 2;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = (byteSize * 2) - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / (BigInteger) Math.Pow(10, j - 1)) % 10);
        }

        return buffer.ToArray();
    }

    public byte[] GetBytes(BigInteger value, int length)
    {
        int byteSize = (length / 2) + (length % 2);
        int maxNumberOfDigits = byteSize * 2;
        int padCount = maxNumberOfDigits - (maxNumberOfDigits - length);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteSize);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = maxNumberOfDigits - padCount; j > 0; i += j % 2, j--)
        {
            if ((j % 2) == 0)
                buffer[i] |= (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i] |= (byte) ((value / (BigInteger) Math.Pow(10, j - 1)) % 10);
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

    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = Hexadecimal.GetBytes(value);

        return true;
    }

    public override int GetByteCount(char[] chars, int index, int count) => GetMaxByteCount(count);
    public override int GetMaxByteCount(int charCount) => charCount / 2;

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public char[] GetChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                GetChars(value[i], buffer, j);

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
                GetChars(value[i], buffer, j);

            return buffer.ToArray();
        }
    }

    private void GetChars(byte value, Span<char> buffer, int offset)
    {
        int leftNibble = value / 10;
        int rightNibble = value % 10;

        buffer[offset++] = _CharMap[(byte) (value / 10)];
        buffer[offset] = _CharMap[(byte) (value % 10)];
    }

    public override int GetCharCount(byte[] bytes, int index, int count) => GetMaxCharCount(count);
    public override int GetMaxCharCount(int byteCount) => byteCount * 2;
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
    public ushort GetUInt16(ReadOnlySpan<byte> value)
    {
        ushort result = 0;

        return (ushort) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint GetUInt32(ReadOnlySpan<byte> value)
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
    public ulong GetUInt64(ReadOnlySpan<byte> value)
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