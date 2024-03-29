﻿using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Core.Extensions.Types;
using Play.Core.Specifications;

namespace Play.Codecs;

public partial class CompressedNumericCodec : PlayCodec
{
    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(CompressedNumericCodec));
    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap = GetByteMap();
    private static readonly ImmutableSortedDictionary<byte, char> _CharMap = GetCharMap();
    private static readonly Nibble _PaddedNibble = Nibble.MaxValue;
    private const char _PaddingCharKey = 'F';
    private const byte _PaddedLeftNibble = 0xF0;
    private const byte _PaddedRightNibble = 0xF;
    private const byte _PaddedByte = 0xFF;

    #endregion

    #region Count

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

    public static int GetPadCount(ReadOnlySpan<byte> value)
    {
        int offset = value.Length - 1;

        for (int i = value.Length - 1; i > 0; i--)
        {
            if (value[i].GetMaskedValue(0xF0) != _PaddedNibble)
                return offset;

            offset++;

            byte leftNibble = (byte) (value[i] >> 4);

            if (leftNibble != _PaddedNibble)
                return offset;

            offset++;
        }

        return offset;
    }

    public int GetByteCount(byte[] value) => value.Length;
    public int GetByteCount(char[] value) => (value.Length / 2) + (value.Length % 2);

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public override ushort GetByteCount<T>(T value) where T : struct => checked((ushort) Unsafe.SizeOf<T>());

    // DEPRECATING: This method will eventually be deprecated in favor of passing in a Span<byte> buffer as opposed to returning a byte[]
    public override ushort GetByteCount<T>(T[] value) where T : struct
    {
        // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
        if (typeof(T) == typeof(byte))
            return (ushort) (ushort) value.Length;

        if (typeof(T) == typeof(char))
            return (ushort) (((ushort) value.Length / 2) + (value.Length % 2));

        throw new NotImplementedException();
    }

    public int GetMaxByteCount(int charCount) => charCount / 2;

    public int GetCharCount(ReadOnlySpan<byte> value)
    {
        for (int i = value.Length - 1, j = 0; i > 0; i--)
        {
            if (value[i].GetMaskedValue(_PaddedLeftNibble) != _PaddedRightNibble)
                return (value.Length * 2) - j;

            j++;

            if (value[i].GetMaskedValue(_PaddedRightNibble) == _PaddedLeftNibble)
                return (value.Length * 2) - j;

            j++;
        }

        return 0;
    }

    public int GetCharCount(ReadOnlySpan<Nibble> value)
    {
        for (int i = value.Length - 1; i > 0; i--)
        {
            if (value[i] == _PaddedNibble)
                return value.Length - i;
        }

        return 0;
    }

    public int GetMaxCharCount(int byteCount) => byteCount * 2;

    #endregion

    #region Validation

    public bool IsValid(ReadOnlySpan<char> value)
    {
        int padCount = GetPadCount(value);

        for (int i = padCount; i < value.Length; i++)
        {
            if (!_ByteMap.ContainsKey(value[i]))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        int padCount = GetPadCount(value);

        for (int i = 0, j = 0; j < ((value.Length * 2) - padCount); i += j % 2, j++)
        {
            if ((padCount % 2) != 0)
            {
                if (!_CharMap.ContainsKey((byte) (value[i] >> 4)))
                    return false;
            }
            else
            {
                if (!_CharMap.ContainsKey(value[i].GetMaskedValue(_PaddedLeftNibble)))
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

    #endregion

    #region Encode

    /// <summary>
    ///     TryEncoding
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public bool TryEncoding(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        // wrong
        result = AlphaNumericCodec.Encode(value);

        return true;
    }

    public byte[] Encode(string value)
    {
        if ((value.Length % 2) != 0)
            throw new ArgumentOutOfRangeException(nameof(value));

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

    // DEPRECATING: This method will eventually be deprecated in favor of strongly typed arguments
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        if (!typeof(T).IsNumericType())
            throw new CodecParsingException(this, typeof(T));

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

    // DEPRECATING: This method will eventually be deprecated in favor of strongly typed arguments
    /// <exception cref="Exception"></exception>
    public override byte[] Encode<T>(T value, int length)
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

    // DEPRECATING: This method will eventually be deprecated in favor of strongly typed arguments
    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value));

        if (typeof(T) == typeof(Nibble))
            return Encode(Unsafe.As<T[], Nibble[]>(ref value));

        throw new NotImplementedException();
    }

    // DEPRECATING: This method will eventually be deprecated in favor of strongly typed arguments
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value), length);

        throw new NotImplementedException();
    }

    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value) => Encode(value, value.Length);

    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value, int length)
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
                    throw new CodecParsingException(
                        $"The value could not be encoded by {nameof(CompressedNumericCodec)} because there was an invalid character", exception);
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
                    throw new CodecParsingException(
                        $"The value could not be encoded by {nameof(CompressedNumericCodec)} because there was an invalid character", exception);
                }
            }

            return result.ToArray();
        }
    }

    public byte[] Encode(Nibble[] value)
    {
        return value.AsByteArray();
    }

    public byte[] Encode(byte value)
    {
        return new[] {value};
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(uint value)
    {
        byte[] buffer = new byte[Specs.Integer.UInt32.ByteCount];
        int mostSignificantByte = (value.GetNumberOfDigits() / 2) + (value.GetNumberOfDigits() % 2);

        if (mostSignificantByte > Specs.Integer.UInt32.ByteCount)
        {
            throw new CodecParsingException(
                $"The {nameof(Encode)} method expected the {nameof(value)} argument to contain {Specs.Integer.UInt32.ByteCount * 2} digits or less but instead it contained {value.GetNumberOfDigits()} digits");
        }

        int padCount = (buffer.Length * 2) - value.GetNumberOfDigits();

        for (int i = mostSignificantByte - 1, j = padCount; j < (Specs.Integer.UInt32.ByteCount * 2); i -= j % 2, j++)
        {
            if ((j % 2) == 0)
            {
                buffer[i] += (byte) (value % 10);
                value /= 10;
            }
            else
            {
                buffer[i] += (byte) ((value % 10) << 4);
                value /= 10;
            }
        }

        EncodePadding(buffer, padCount, mostSignificantByte);

        return buffer;
    }

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ushort value)
    {
        byte[] buffer = new byte[Specs.Integer.UInt16.ByteCount];
        int mostSignificantByte = (value.GetNumberOfDigits() / 2) + (value.GetNumberOfDigits() % 2);

        if (mostSignificantByte > Specs.Integer.UInt16.ByteCount)
        {
            throw new CodecParsingException(
                $"The {nameof(Encode)} method expected the {nameof(value)} argument to contain {Specs.Integer.UInt16.ByteCount * 2} digits or less but instead it contained {value.GetNumberOfDigits()} digits");
        }

        int padCount = (buffer.Length * 2) - value.GetNumberOfDigits();

        for (int i = mostSignificantByte - 1, j = padCount; j < (Specs.Integer.UInt16.ByteCount * 2); i -= j % 2, j++)
        {
            if ((j % 2) == 0)
            {
                buffer[i] += (byte) (value % 10);
                value /= 10;
            }
            else
            {
                buffer[i] += (byte) ((value % 10) << 4);
                value /= 10;
            }
        }

        EncodePadding(buffer, padCount, mostSignificantByte);

        return buffer;
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public byte[] Encode(uint value, int length)
    {
        const byte byteSize = Specs.Integer.UInt32.ByteCount;

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

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ulong value)
    {
        byte[] buffer = new byte[Specs.Integer.UInt64.ByteCount];
        int mostSignificantByte = (value.GetNumberOfDigits() / 2) + (value.GetNumberOfDigits() % 2);

        if (mostSignificantByte > Specs.Integer.UInt64.ByteCount)
        {
            throw new CodecParsingException(
                $"The {nameof(Encode)} method expected the {nameof(value)} argument to contain {Specs.Integer.UInt64.ByteCount * 2} digits or less but instead it contained {value.GetNumberOfDigits()} digits");
        }

        int padCount = (buffer.Length * 2) - value.GetNumberOfDigits();

        for (int i = mostSignificantByte - 1, j = padCount; j < (Specs.Integer.UInt64.ByteCount * 2); i -= j % 2, j++)
        {
            if ((j % 2) == 0)
            {
                buffer[i] += (byte) (value % 10);
                value /= 10;
            }
            else
            {
                buffer[i] += (byte) ((value % 10) << 4);
                value /= 10;
            }
        }

        EncodePadding(buffer, padCount, mostSignificantByte);

        return buffer;
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public byte[] Encode(ulong value, int length)
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

    public byte[] Encode(BigInteger value)
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

    public byte[] Encode(BigInteger value, int length)
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

    // //////////////////////////////END

    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        ReadOnlySpan<byte> buffer = Encode(chars[charIndex..(charIndex + charCount)]);

        for (int i = 0, j = byteIndex; i < (buffer.Length + byteIndex); i++, j++)
            bytes[j] = buffer[i];

        return buffer.Length;
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value));
        else
            throw new NotImplementedException();
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value), length);
        else
            throw new NotImplementedException();
    }

    // //////////////////////////////START

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<T, byte>(ref value), buffer, ref offset);
        else if (byteSize == Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<T, ushort>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<T, uint>(ref value), buffer, ref offset);
        else if (byteSize <= Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<T, BigInteger>(ref value), buffer, ref offset);
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        if (length == Specs.Integer.UInt8.ByteCount)
            Encode(Unsafe.As<T, byte>(ref value));
        else if (length == Specs.Integer.UInt16.ByteCount)
            Encode(Unsafe.As<T, ushort>(ref value));
        else if (length == 3)
            Encode(Unsafe.As<T, uint>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt32.ByteCount)
            Encode(Unsafe.As<T, uint>(ref value), buffer, ref offset);
        else if (length < Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), length, buffer, ref offset);
        else if (length == Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), buffer, ref offset);
        else
            Encode(Unsafe.As<T, BigInteger>(ref value), length, buffer, ref offset);
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
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
                throw new CodecParsingException($"The value could not be encoded by {nameof(CompressedNumericCodec)} because there was an invalid character",
                    exception);
            }
        }

        offset += byteSize;
    }

    #endregion

    #region Decode To Chars

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
        buffer[offset++] = _CharMap[(byte) (value >> 4)];
        buffer[offset] = _CharMap[(byte) value.GetMaskedValue(0xF0)];
    }

    #endregion

    #region Decode To String

    public string DecodeToString(ReadOnlySpan<byte> value) => new(DecodeToChars(value));

    public bool TryDecodingToString(ReadOnlySpan<byte> value, out string result)
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

    #region Decode To Integers

    public BigInteger DecodeToBigInteger(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(value)));

        BigInteger result = 0;

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == _PaddedByte)
                return result;

            result *= 10;
            result += (byte) (value[i] >> 4);
            byte rightNibble = value[i].GetMaskedValue(0xF0);

            if (rightNibble == _PaddedRightNibble)
                return result;

            result *= 10;
            result += rightNibble;
        }

        return result;
    }

    public byte DecodeToByte(byte value)
    {
        if (value == _PaddedByte)
            return 0;

        byte leftNibble = (byte) (value >> 4);
        byte rightNibble = (byte) (value & ~0xF0);

        if (rightNibble == _PaddedNibble)
            return leftNibble;

        return (byte) ((leftNibble * 10) + rightNibble);
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint DecodeToUInt32(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(value)));

        uint result = 0;

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == _PaddedByte)
                return result;

            result *= 10;
            result += (byte) (value[i] >> 4);
            byte rightNibble = value[i].GetMaskedValue(0xF0);

            if (rightNibble == _PaddedRightNibble)
                return result;

            result *= 10;
            result += rightNibble;
        }

        return result;
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ulong DecodeToUInt64(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(value)));

        ulong result = 0;

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == _PaddedByte)
                return result;

            result *= 10;
            result += (byte) (value[i] >> 4);
            byte rightNibble = value[i].GetMaskedValue(0xF0);

            if (rightNibble == _PaddedRightNibble)
                return result;

            result *= 10;
            result += rightNibble;
        }

        return result;
    }

    /// <summary>
    ///     Takes the Numeric encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ushort DecodeToUInt16(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(value)));

        ushort result = 0;

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == _PaddedByte)
                return result;

            result *= 10;
            result += (byte) (value[i] >> 4);
            byte rightNibble = value[i].GetMaskedValue(0xF0);

            if (rightNibble == _PaddedRightNibble)
                return result;

            result *= 10;
            result += rightNibble;
        }

        return result;
    }

    /// <exception cref="CodecParsingException"></exception>
    private static byte DecodeToByte(char leftChar, char rightChar)
    {
        if (!_ByteMap.ContainsKey(leftChar))
            throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
        if (!_ByteMap.ContainsKey(rightChar))
            throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);

        byte result = _ByteMap[leftChar];
        result <<= 4;
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

        return nibbles[..^(nibbles.Length - charCount)].ToArray();
    }

    #endregion

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        // HACK: We're removing the dynamic decoding capability and using explicit decoding calls
        BigInteger maximumIntegerResult = (BigInteger) Math.Pow(2, value.Length * 8);

        if (maximumIntegerResult <= byte.MaxValue)
        {
            byte result = DecodeToByte(value[0]);

            return new DecodedResult<byte>(result, result.GetNumberOfDigits());
        }

        if (maximumIntegerResult <= ushort.MaxValue)
        {
            ushort result = DecodeToUInt16(value);

            return new DecodedResult<ushort>(result, result.GetNumberOfDigits());
        }

        if (maximumIntegerResult <= uint.MaxValue)
        {
            uint result = DecodeToUInt32(value);

            return new DecodedResult<uint>(result, result.GetNumberOfDigits());
        }

        if (maximumIntegerResult <= ulong.MaxValue)
        {
            ulong result = DecodeToUInt64(value);

            return new DecodedResult<ulong>(result, result.GetNumberOfDigits());
        }
        else
        {
            BigInteger result = DecodeToBigInteger(value);

            return new DecodedResult<BigInteger>(result, result.GetNumberOfDigits());
        }
    }

    #endregion

    #region Instance Members

    private void EncodePadding(Span<byte> buffer, int padCount, int mostSignificantByte)
    {
        if (padCount == 0)
            return;

        if ((padCount % 2) != 0)
            buffer[mostSignificantByte - 1] += 0x0F;

        if (padCount == 1)
            return;

        buffer[mostSignificantByte..].Fill(0xFF);
    }

    private static ImmutableSortedDictionary<byte, char> GetCharMap()
    {
        Dictionary<byte, char> map = Enumerable.Range(0, 10).ToDictionary(a => (byte) a, b => (char) (b + 48));
        map.Add(0xF, 'F');

        return map.ToImmutableSortedDictionary();
    }

    private static ImmutableSortedDictionary<char, byte> GetByteMap()
    {
        Dictionary<char, byte> map = Enumerable.Range(0, 10).ToDictionary(a => (char) (a + 48), b => (byte) b);
        map.Add('F', 0xF);

        return map.ToImmutableSortedDictionary();
    }

    private int Pad(Span<byte> buffer)
    {
        int padCount = GetPadCount(buffer);
        buffer[^(padCount / 2)..].Fill(_PaddedByte);
        if ((padCount % 2) != 0)
            buffer[^((padCount / 2) + 1)] |= _PaddedRightNibble;

        return padCount;
    }

    #endregion
}