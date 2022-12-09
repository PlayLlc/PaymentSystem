﻿using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Core.Extensions.Types;
using Play.Core.Specifications;

namespace Play.Codecs;

public class NumericCodec : PlayCodec
{
    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(NumericCodec));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    private const byte _PadValue = 0;

    #endregion

    #region Count

    public ushort GetByteCount(ReadOnlySpan<char> value) => (ushort) ((value.Length / 2) + (value.Length % 2));

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public override ushort GetByteCount<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(byte))
            return (ushort) (ushort) value.Length;

        if (typeof(T) == typeof(char))
            return (ushort) checked((value.Length % 2) + (value.Length / 2));

        throw new NotImplementedException();
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public override ushort GetByteCount<_T>(_T value) where _T : struct
    {
        Type type = typeof(_T);

        if (!type.IsNumericType())
            throw new CodecParsingException(this, type);

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            return GetByteCount(Unsafe.As<_T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteCount)
            return GetByteCount(Unsafe.As<_T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteCount)
            return GetByteCount(Unsafe.As<_T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return GetByteCount(Unsafe.As<_T, ulong>(ref value));

        return GetByteCount(Unsafe.As<_T, BigInteger>(ref value));
    }

    public ushort GetByteCount(byte value) => (ushort)(value.GetNumberOfDigits() / 2 + value.GetNumberOfDigits() % 2);

    public ushort GetByteCount(ushort value) => (ushort)(value.GetNumberOfDigits() / 2 + value.GetNumberOfDigits() % 2);

    public ushort GetByteCount(uint value) => (ushort)(value.GetNumberOfDigits() / 2 + value.GetNumberOfDigits() % 2);

    public ushort GetByteCount(ulong value) => (ushort)(value.GetNumberOfDigits() / 2 + value.GetNumberOfDigits() % 2);

    public ushort GetByteCount(BigInteger value) => unchecked((ushort)(value.GetNumberOfDigits() / 2 + value.GetNumberOfDigits() % 2));

    public int GetMaxByteCount(int charCount) => charCount / 2;

    public int GetCharCount(ReadOnlySpan<byte> value)
    {
        for (int i = 0, j = 0; i < value.Length; i++)
        {
            if ((value[i] >> 4) != _PadValue)
                return (value.Length * 2) - j;

            j++;

            if (value[i].GetMaskedValue(0xF0) != _PadValue)
                return (value.Length * 2) - j;

            j++;
        }

        return 0;
    }

    public int GetMaxCharCount(int byteCount) => byteCount * 2;

    #endregion

    #region Validation

    public bool IsValid(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    /// <exception cref="OverflowException"></exception>
    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        Nibble[] buffer = value.AsNibbleArray();

        foreach (Nibble nibble in buffer)
        {
            if (!IsValid(nibble))
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

    #endregion

    #region Encode

    public bool TryEncoding(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = HexadecimalCodec.Encode(value);

        return true;
    }

    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(string value)
    {
        if ((value.Length % 2) != 0)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(value)));

        int length = value.Length / 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[length];

            for (int i = 0, j = 0; i < length; i++, j += 2)
                buffer[i] = DecodeToByte(value[j], value[j + 1]);

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < length; i++, j += 2)
                buffer[i] = DecodeToByte(value[j], value[j + 1]);

            return buffer.ToArray();
        }
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]

    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T, char>(ref value));

        if (!type.IsNumericType())
            throw new CodecParsingException(this, type);

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<_T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<_T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<_T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<_T, ulong>(ref value));

        return Encode(Unsafe.As<_T, BigInteger>(ref value));
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]

    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T value, int length)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T, char>(ref value));

        if (!type.IsNumericType())
            throw new CodecParsingException(this, type);

        if (length == Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<_T, byte>(ref value));
        if (length == Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<_T, ushort>(ref value));
        if (length == 3)
            return Encode(Unsafe.As<_T, uint>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<_T, uint>(ref value));
        if (length < Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<_T, ulong>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<_T, ulong>(ref value));

        return Encode(Unsafe.As<_T, BigInteger>(ref value), length);
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]

    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T[] value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T[], char[]>(ref value));

        if (!type.IsByte())
            throw new CodecParsingException(this, type);

        return Encode(Unsafe.As<_T[], byte[]>(ref value));
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]

    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T[] value, int length)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
            return Encode(Unsafe.As<_T[], char[]>(ref value), length);

        if (!type.IsByte())
            throw new CodecParsingException(this, type);

        return Encode(Unsafe.As<_T[], byte[]>(ref value), length);
    }

    public byte[] Encode(ReadOnlySpan<char> value) => Encode(value, value.Length);

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
        byte numberOfDigits = value.GetNumberOfDigits();
        byte[] buffer = new byte[(numberOfDigits / 2) + (numberOfDigits % 2)];

        for (int i = buffer.Length - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value % 10);
            value /= 10;
            buffer[i] += (byte) ((value % 10) * 10);
            value /= 10;
        }

        return buffer;
    }

    public byte[] Encode(ushort value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        byte[] buffer = new byte[(numberOfDigits / 2) + (numberOfDigits % 2)];

        for (int i = buffer.Length - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value % 10);
            value /= 10;
            buffer[i] += (byte) ((value % 10) * 10);
            value /= 10;
        }

        return buffer;
    }

    public byte[] Encode(ushort value, int length)
    {
        byte[] buffer;

        if (length == Specs.Integer.UInt16.ByteCount)
        {
            buffer = new byte[Specs.Integer.UInt16.ByteCount];

            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (value % 10);
                value /= 10;
                buffer[i] += (byte) ((value % 10) * 10);
                value /= 10;
            }

            return buffer;
        }

        if (length < Specs.Integer.UInt16.ByteCount)
        {
            buffer = new byte[Specs.Integer.UInt16.ByteCount];

            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (value % 10);
                value /= 10;
                buffer[i] += (byte) ((value % 10) * 10);
                value /= 10;
            }

            return buffer[^length..];
        }

        buffer = new byte[length];

        for (int i = buffer.Length - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value % 10);
            value /= 10;
            buffer[i] += (byte) ((value % 10) * 10);
            value /= 10;
        }

        return buffer;
    }

    public byte[] Encode(uint value, int length)
    {
        byte[] buffer;

        if (length == Specs.Integer.UInt32.ByteCount)
        {
            buffer = new byte[Specs.Integer.UInt32.ByteCount];

            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (value % 10);
                value /= 10;
                buffer[i] += (byte) ((value % 10) * 10);
                value /= 10;
            }

            return buffer;
        }

        if (length < Specs.Integer.UInt32.ByteCount)
        {
            buffer = new byte[Specs.Integer.UInt32.ByteCount];

            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (value % 10);
                value /= 10;
                buffer[i] += (byte) ((value % 10) * 10);
                value /= 10;
            }

            return buffer[^length..];
        }

        buffer = new byte[length];

        for (int i = buffer.Length - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value % 10);
            value /= 10;
            buffer[i] += (byte) ((value % 10) * 10);
            value /= 10;
        }

        return buffer;
    }

    public byte[] Encode(uint value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        byte[] buffer = new byte[(numberOfDigits / 2) + (numberOfDigits % 2)];

        for (int i = buffer.Length - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value % 10);
            value /= 10;
            buffer[i] += (byte) ((value % 10) * 10);
            value /= 10;
        }

        return buffer;
    }

    public byte[] Encode(ulong value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        byte[] buffer = new byte[(numberOfDigits / 2) + (numberOfDigits % 2)];

        for (int i = buffer.Length - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value % 10);
            value /= 10;
            buffer[i] += (byte) ((value % 10) * 10);
            value /= 10;
        }

        return buffer;
    }

    public byte[] Encode(ulong value, int length)
    {
        byte[] buffer;

        if (length == Specs.Integer.UInt64.ByteCount)
        {
            buffer = new byte[Specs.Integer.UInt64.ByteCount];

            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (value % 10);
                value /= 10;
                buffer[i] += (byte) ((value % 10) * 10);
                value /= 10;
            }

            return buffer;
        }

        if (length < Specs.Integer.UInt64.ByteCount)
        {
            buffer = new byte[Specs.Integer.UInt64.ByteCount];

            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                buffer[i] = (byte) (value % 10);
                value /= 10;
                buffer[i] += (byte) ((value % 10) * 10);
                value /= 10;
            }

            return buffer[^length..];
        }

        buffer = new byte[length];

        for (int i = buffer.Length - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value % 10);
            value /= 10;
            buffer[i] += (byte) ((value % 10) * 10);
            value /= 10;
        }

        return buffer;
    }

    // TODO: This is likely wrong
    public byte[] Encode(BigInteger value)
    {
        int numberOfDigits = value.GetNumberOfDigits();
        byte[] buffer = new byte[(numberOfDigits / 2) + (numberOfDigits % 2)];

        for (int i = buffer.Length - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value % 10);
            value /= 10;
            buffer[i] += (byte) ((value % 10) * 10);
            value /= 10;
        }

        return buffer;
    }

    // TODO: This is likely wrong
    /// <exception cref="OverflowException"></exception>
    public byte[] Encode(BigInteger value, int length)
    {
        byte[] buffer = new byte[length];

        for (int i = buffer.Length - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value % 10);
            value /= 10;

            if (value == 0)
                break;

            buffer[i] += (byte) ((value % 10) * 10);
            value /= 10;
        }

        return buffer;
    }

    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        ReadOnlySpan<byte> buffer = Encode(chars[charIndex..(charIndex + charCount)]);

        for (int i = 0, j = byteIndex; i < (buffer.Length + byteIndex); i++, j++)
            bytes[j] = buffer[i];

        return buffer.Length;
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]

    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T value, Span<byte> buffer, ref int offset)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
        {
            Encode(Unsafe.As<_T, char>(ref value), buffer, ref offset);

            return;
        }

        if (!type.IsNumericType())
            throw new CodecParsingException(this, type);

        nint byteSize = Unsafe.SizeOf<_T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<_T, byte>(ref value), buffer, ref offset);
        else if (byteSize == Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<_T, ushort>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<_T, uint>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<_T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<_T, BigInteger>(ref value), buffer, ref offset);
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]

    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T value, int length, Span<byte> buffer, ref int offset)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (type.IsChar())
        {
            Encode(Unsafe.As<_T, char>(ref value), length, buffer, ref offset);

            return;
        }

        if (!type.IsNumericType())
            throw new CodecParsingException(this, type);

        if (length == Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<_T, byte>(ref value));
        else if (length == Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<_T, ushort>(ref value));
        else if (length == 3)
            Encode(Unsafe.As<_T, uint>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<_T, uint>(ref value), buffer, ref offset);
        else if (length < Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<_T, ulong>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<_T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<_T, BigInteger>(ref value), length, buffer, ref offset);
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]

    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(_T).IsChar())
            Encode(Unsafe.As<_T[], char[]>(ref value));
        else
            throw new CodecParsingException(this, typeof(_T));
    }

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]

    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(_T) == typeof(char))
            Encode(Unsafe.As<_T[], char[]>(ref value), length);
        else
            throw new CodecParsingException(this, typeof(_T));
    }

    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    public void Encode(ReadOnlySpan<Nibble> value, Span<byte> buffer)
    {
        int byteCount = (value.Length / 2) + (value.Length % 2);

        if (byteCount < buffer.Length)
            throw new CodecParsingException($"The {nameof(buffer)} argument was too small to encode the provided value");

        // BUG: Numeric Encoding is left padded. This looks like it's right padded
        for (int i = 0; i < value.Length; i++)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) (value[i] << 4);
            else
                buffer[i / 2] |= value[i];
        }
    }

    /// <exception cref="CodecParsingException"></exception>
    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        int byteSize = (value.Length / 2) + (value.Length % 2);

        if (byteSize == length)
        {
            HexadecimalCodec.Encode(value, buffer, ref offset);

            return;
        }

        if (byteSize > length)
        {
            HexadecimalCodec.Encode(value, length, buffer, ref offset);

            return;
        }

        HexadecimalCodec.Encode(value[..length], buffer, ref offset);
    }

    #endregion

    #region Decode To Chars

    public void DecodeToChars(ReadOnlySpan<byte> value, Span<char> buffer, ref int offset)
    {
        for (int i = 0, j = 0; i < value.Length; i++, j += 2)
            DecodeToChars(value[i], buffer[(offset + j)..], j);
    }

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                DecodeToChars(value[i], buffer, j);

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                DecodeToChars(value[i], buffer, j);

            return buffer.ToArray();
        }
    }

    private void DecodeToChars(byte value, Span<char> buffer, int offset)
    {
        buffer[offset++] = _CharMap[(byte) (value / 10)];
        buffer[offset] = _CharMap[(byte) (value % 10)];
    }

    #endregion

    #region Decode To String

    public string DecodeToString(ReadOnlySpan<byte> value) => new(DecodeToChars(value));

    #endregion

    #region Decode To Integers

    /// <summary>
    ///     DecodeToBigInteger
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public BigInteger DecodeToBigInteger(ReadOnlySpan<byte> value)
    {
        BigInteger resultBuffer = 0;

        for (int i = value.Length - 1, j = 0; i >= 0; i--, j += 2)
        {
            resultBuffer += (BigInteger) ((value[i] % 10) * Math.Pow(10, j));
            resultBuffer += (BigInteger) (((value[i] / 10) % 10) * Math.Pow(10, j + 1));
        }

        return resultBuffer;
    }

    public int GetBigIntegerByteCount(BigInteger input)
    {
        int result = 0;

        while(input != 0)
        {
            input = input / 100;
            result++;
        }

        return result;
    }

    public byte DecodeToByte(ReadOnlySpan<byte> value)
    {
        CheckCore.ForExactLength(value, 1, nameof(value));

        return value[0];
    }

    /// <summary>
    ///     DecodeToByte
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public byte DecodeToByte(byte value)
    {
        byte resultBuffer = 0;

        resultBuffer += (byte) (value % 10);
        resultBuffer += (byte) (((value / 10) % 10) * 10);

        return resultBuffer;
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public uint DecodeToUInt32(ReadOnlySpan<byte> value)
    {
        uint resultBuffer = 0;

        for (int i = value.Length - 1, j = 0; i >= 0; i--, j += 2)
        {
            resultBuffer += (uint) ((value[i] % 10) * Math.Pow(10, j));
            resultBuffer += (uint) (((value[i] / 10) % 10) * Math.Pow(10, j + 1));
        }

        return resultBuffer;
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public ulong DecodeToUInt64(ReadOnlySpan<byte> value)
    {
        ulong resultBuffer = 0;

        for (int i = value.Length - 1, j = 0; i >= 0; i--, j += 2)
        {
            resultBuffer += (ulong) ((value[i] % 10) * Math.Pow(10, j));
            resultBuffer += (ulong) (((value[i] / 10) % 10) * Math.Pow(10, j + 1));
        }

        return resultBuffer;
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public ushort DecodeToUInt16(ReadOnlySpan<byte> value)
    {
        ushort resultBuffer = 0;

        for (int i = value.Length - 1, j = 0; i >= 0; i--, j += 2)
        {
            resultBuffer += (ushort) ((value[i] % 10) * Math.Pow(10, j));
            resultBuffer += (ushort) (((value[i] / 10) % 10) * Math.Pow(10, j + 1));
        }

        return resultBuffer;
    }

    private static byte DecodeToByte(char leftChar, char rightChar)
    {
        byte result = _ByteMap[leftChar];
        result *= 10;
        result += _ByteMap[rightChar];

        return result;
    }

    #endregion

    #region Decode To Nibbles

    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public Nibble[] DecodeToNibbles(ReadOnlySpan<byte> value)
    {
        ReadOnlySpan<Nibble> nibbles = value.AsNibbleArray();
        int charCount = GetCharCount(value);

        return nibbles[^charCount..].ToArray();
    }

    #endregion

    #region Decode To DecodedMetadata

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Exceptions.CodecParsingException"></exception>
    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        ReadOnlySpan<byte> trimmedValue = value.TrimStart((byte) 0);

        if (value.Length <= Specs.Integer.UInt8.ByteCount)
        {
            byte byteResult = DecodeToByte(trimmedValue[0]);

            return new DecodedResult<byte>(byteResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt16.ByteCount)
        {
            ushort shortResult = DecodeToUInt16(trimmedValue);

            return new DecodedResult<ushort>(shortResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt32.ByteCount)
        {
            uint intResult = DecodeToUInt32(trimmedValue);

            return new DecodedResult<uint>(intResult, value.Length * 2);
        }

        if (value.Length <= Specs.Integer.UInt64.ByteCount)
        {
            ulong longResult = DecodeToUInt64(trimmedValue);

            return new DecodedResult<ulong>(longResult, value.Length * 2);
        }

        BigInteger bigIntegerResult = DecodeToBigInteger(trimmedValue);

        return new DecodedResult<BigInteger>(bigIntegerResult, value.Length * 2);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Validate
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="CodecParsingException"></exception>
    private void Validate(byte value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(CodecParsingException.ByteArrayContainsInvalidValue);
    }

    /// <summary>
    ///     Validate
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="CodecParsingException"></exception>
    public void Validate(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(CodecParsingException.ByteArrayContainsInvalidValue);
    }

    public bool TryDecodeToString(ReadOnlySpan<byte> value, out string result)
    {
        if (!IsValid(value))
        {
            result = string.Empty;

            return false;
        }

        result = DecodeToString(value);

        return true;
    }

    #endregion

    ///// <summary>
    /////     Decode
    ///// </summary>
    ///// <param name="value"></param>
    ///// <returns></returns>
    ///// <exception cref="Exceptions.CodecParsingException"></exception>
    //public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    //{
    //    ReadOnlySpan<byte> trimmedValue = value.TrimStart((byte) 0);

    //    if (value.Length == Specs.Integer.UInt8.ByteCount)
    //    {
    //        byte byteResult = DecodeToByte(trimmedValue[0]);

    //        return new DecodedResult<byte>(byteResult, value.Length * 2);
    //    }

    //    if (value.Length <= Specs.Integer.UInt16.ByteCount)
    //    {
    //        ushort shortResult = DecodeToUInt16(trimmedValue);

    //        return new DecodedResult<ushort>(shortResult, value.Length * 2);
    //    }

    //    if (value.Length <= Specs.Integer.UInt32.ByteCount)
    //    {
    //        uint intResult = DecodeToUInt32(trimmedValue);

    //        return new DecodedResult<uint>(intResult, value.Length * 2);
    //    }

    //    if (value.Length <= Specs.Integer.UInt64.ByteCount)
    //    {
    //        ulong longResult = DecodeToUInt64(trimmedValue);

    //        return new DecodedResult<ulong>(longResult, value.Length * 2);
    //    }

    //    BigInteger bigIntegerResult = DecodeToBigInteger(trimmedValue);

    //    return new DecodedResult<BigInteger>(bigIntegerResult, value.Length * 2);
    //}
}