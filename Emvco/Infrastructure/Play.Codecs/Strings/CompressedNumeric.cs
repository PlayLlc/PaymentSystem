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

// TODO: need to move Play.Codec.CompressedNumeric logic into here
public class CompressedNumeric : PlayEncoding
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = new(nameof(CompressedNumeric));

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap = new Dictionary<byte, char>
    {
        {48, '0'},
        {49, '1'},
        {50, '2'},
        {51, '3'},
        {52, '4'},
        {53, '5'},
        {54, '6'},
        {55, '7'},
        {56, '8'},
        {57, '9'}
    }.ToImmutableSortedDictionary();

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap = new Dictionary<char, byte>
    {
        {'0', 48},
        {'1', 49},
        {'2', 50},
        {'3', 51},
        {'4', 52},
        {'5', 53},
        {'6', 54},
        {'7', 55},
        {'8', 56},
        {'9', 57}
    }.ToImmutableSortedDictionary();

    private const byte _PaddingByteKey = 70;
    private const char _PaddingCharKey = 'F';
    private const byte _LeftNibbleMask = (byte) (Bits.Eight | Bits.Seven | Bits.Six | Bits.Five);
    private const byte _PaddedNibble = 0xF;

    #endregion

    #region Instance Members

    public PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    public override bool IsValid(ReadOnlySpan<char> value)
    {
        ReadOnlySpan<char> trimmedValue = TrimPadding(value);

        for (int i = 0; i < trimmedValue.Length; i++)
        {
            if (!_ByteMap.Keys.Contains(value[i]))
                return false;
        }

        return true;
    }

    public ReadOnlySpan<char> TrimPadding(ReadOnlySpan<char> value)
    {
        for (int i = value.Length; i > 0; i--)
        {
            if (value[i] != _PaddingCharKey)
                return value[..i];
        }

        return value;
    }

    public override bool IsValid(ReadOnlySpan<byte> value) => IsNumericEncodingValid(value[..^GetPaddingIndexFromEnd(value)]);

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        Encode(chars[charIndex..charCount].AsSpan(), bytes[byteIndex..].AsSpan());

    public override byte[] GetBytes(ReadOnlySpan<char> value) => Encode(value);

    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        if (IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = Encode(value);

        return true;
    }

    public override int GetByteCount(char[] chars, int index, int count) => (count / 2) + (count % 2);
    public override int GetMaxByteCount(int charCount) => (charCount / 2) + (charCount % 2);

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public override int GetCharCount(byte[] bytes, int index, int count) => throw new NotImplementedException();
    public override int GetMaxCharCount(int byteCount) => byteCount * 2;
    public override string GetString(ReadOnlySpan<byte> value) => throw new NotImplementedException();
    public override bool TryGetString(ReadOnlySpan<byte> value, out string result) => throw new NotImplementedException();

    public int GetNumberOfDigits(byte[] value)
    {
        int padCount = 0;

        for (int i = value.Length; i > 0; i--)
        {
            if (value[i].GetRightNibble() != _PaddingByteKey)
                break;

            padCount++;

            if (value[i].GetRightNibble() != _PaddingByteKey)
                break;

            padCount++;
        }

        return (value.Length * 2) - padCount;
    }

    public byte[] Encode<T>(T[] value) where T : struct => throw new NotImplementedException();
    public byte[] Encode<T>(T[] value, int length) where T : struct => throw new NotImplementedException();

    public void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteSize)
        {
            buffer[offset++] = Unsafe.As<T, byte>(ref value);

            return;
        }

        if (byteSize == Specs.Integer.UInt16.ByteSize)
            Encode(Unsafe.As<T, ushort>(ref value), buffer, ref offset);
        if (byteSize <= Specs.Integer.UInt32.ByteSize)
            Encode(Unsafe.As<T, uint>(ref value), buffer, ref offset);
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            Encode(Unsafe.As<T, ulong>(ref value), buffer, ref offset);

        Encode(Unsafe.As<T, BigInteger>(ref value), buffer, ref offset);
    }

    public void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public byte[] Encode<T>(T value) where T : struct
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteSize)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteSize)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteSize)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value));
    }

    public byte[] Encode<T>(T value, int length) where T : struct
    {
        if (length == Specs.Integer.UInt8.ByteSize)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (length == Specs.Integer.UInt16.ByteSize)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (length == 3)
            return Encode(Unsafe.As<T, uint>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteSize)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (length < Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value), length);
    }

    public new int GetByteCount(ReadOnlySpan<char> value) => (value.Length / 2) + (value.Length % 2);

    private void Validate(ReadOnlySpan<char> value)
    {
        if (!IsValid(value))
            throw new EncodingException($"The codec {nameof(CompressedNumeric)} encountered an invalid character");
    }

    public byte[] Encode(ReadOnlySpan<char> value)
    {
        try
        {
            int byteCount = (value.Length / 2) + (value.Length % 2);
            int paddingCount = GetPaddingCount(value);

            Span<byte> result = new byte[GetByteCount(value)];

            Pad(result, paddingCount);

            for (int i = 0, j = 0; i < (byteCount - (paddingCount / 2)); i += j % 2)
            {
                if ((j++ % 2) == 0)
                    result[i] = _ByteMap[value[j]];
                else
                    result[i] |= (byte) (_ByteMap[value[j]] << 4);
            }

            return result.ToArray();
        }
        catch (IndexOutOfRangeException e)
        {
            throw new EncodingException($"The codec {nameof(CompressedNumeric)} encountered an invalid character");
        }
        catch (Exception e)
        {
            throw new EncodingException($"The codec {nameof(CompressedNumeric)} encountered an invalid character");
        }
    }

    public int Encode(ReadOnlySpan<char> value, Span<byte> buffer)
    {
        try
        {
            int byteCount = (value.Length / 2) + (value.Length % 2);
            int paddingCount = GetPaddingCount(value);

            Pad(buffer, paddingCount);

            for (int i = 0, j = 0; i < (byteCount - (paddingCount / 2)); i += j % 2)
            {
                if ((j++ % 2) == 0)
                    buffer[i] = _ByteMap[value[j]];
                else
                    buffer[i] |= (byte) (_ByteMap[value[j]] << 4);
            }

            return byteCount;
        }
        catch (IndexOutOfRangeException e)
        {
            throw new EncodingException($"The codec {nameof(CompressedNumeric)} encountered an invalid character");
        }
        catch (Exception e)
        {
            throw new EncodingException($"The codec {nameof(CompressedNumeric)} encountered an invalid character");
        }
    }

    private void Pad(Span<byte> result, int padCount)
    {
        for (int i = result.Length, j = 0; j < padCount; i += j % 2)
        {
            if ((j++ % 2) == 0)
                result[i] = _PaddingByteKey;
            else
                result[i] |= 0xF0;
        }
    }

    private int GetPaddingCount(ReadOnlySpan<char> value)
    {
        for (int i = value.Length; i > 0; i--)
        {
            if (value[i] != _PaddingCharKey)
                return i;
        }

        return value.Length;
    }

    public byte[] Encode(byte value)
    {
        const byte compressedNumericByteSize = Specs.Integer.UInt8.CompressedNumericByteSize;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(compressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;
        byte numberOfDigits = value.GetNumberOfDigits();

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(ushort value)
    {
        const byte compressedNumericByteSize = Specs.Integer.UInt16.CompressedNumericByteSize;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(compressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;
        byte numberOfDigits = value.GetNumberOfDigits();

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(uint value)
    {
        const byte compressedNumericByteSize = Specs.Integer.UInt32.CompressedNumericByteSize;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(compressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;
        byte numberOfDigits = value.GetNumberOfDigits();

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(uint value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();

        if (((numberOfDigits % 2) + (numberOfDigits / 2)) == length)
            return Encode(value);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(Specs.Integer.UInt32.CompressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;

        // space for padding
        if (length > ((numberOfDigits % 2) + (numberOfDigits / 2)))
        {
            for (int i = 0, j = (length * 2) - ((length * 2) - numberOfDigits) - 1; i < numberOfDigits; i++, j--)
            {
                if ((i % 2) == 0)
                    buffer[i / 2] = (byte) ((byte) ((value / (uint) Math.Pow(10, j)) % 10) << 4);
                else
                    buffer[i / 2] |= (byte) ((value / (uint) Math.Pow(10, j)) % 10);
            }
        }

        // truncate
        for (int i = 0, j = numberOfDigits - 1; i < (length * 2); i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (uint) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (uint) Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(ulong value)
    {
        const byte compressedNumericByteSize = Specs.Integer.UInt64.CompressedNumericByteSize;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(compressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;
        byte numberOfDigits = value.GetNumberOfDigits();

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(ulong value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();

        if (((numberOfDigits % 2) + (numberOfDigits / 2)) == length)
            return Encode(value);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(Specs.Integer.UInt64.CompressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;

        // space for padding
        if (length > ((numberOfDigits % 2) + (numberOfDigits / 2)))
        {
            for (int i = 0, j = (length * 2) - ((length * 2) - numberOfDigits) - 1; i < numberOfDigits; i++, j--)
            {
                if ((i % 2) == 0)
                    buffer[i / 2] = (byte) ((byte) ((value / (ulong) Math.Pow(10, j)) % 10) << 4);
                else
                    buffer[i / 2] |= (byte) ((value / (ulong) Math.Pow(10, j)) % 10);
            }
        }

        // truncate
        for (int i = 0, j = numberOfDigits - 1; i < (length * 2); i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (ulong) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (ulong) Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(BigInteger value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((numberOfDigits % 2) + (numberOfDigits / 2));
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(BigInteger value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();

        if (((numberOfDigits % 2) + (numberOfDigits / 2)) == length)
            return Encode(value);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
        Span<byte> buffer = spanOwner.Span;

        // space for padding
        if (length > ((numberOfDigits % 2) + (numberOfDigits / 2)))
        {
            for (int i = 0, j = (length * 2) - ((length * 2) - numberOfDigits) - 1; i < numberOfDigits; i++, j--)
            {
                if ((i % 2) == 0)
                    buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
                else
                    buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
            }
        }

        // truncate
        for (int i = 0, j = numberOfDigits - 1; i < (length * 2); i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public ushort GetByteCount<T>(T value) where T : struct => checked((ushort) Unsafe.SizeOf<T>());
    public ushort GetByteCount<T>(T[] value) where T : struct => throw new NotImplementedException();

    public BigInteger DecodeBigInteger(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
        }

        BigInteger result = 0;

        return BuildInteger(result, value);
    }

    public byte DecodeByte(byte value)
    {
        if (value.GetMaskedValue(_LeftNibbleMask) == _PaddedNibble)
            return (byte) (value >> 4);

        byte result = (byte) (value >> 4);
        result *= 10;
        result += value.GetMaskedValue(_LeftNibbleMask);

        return result;
    }

    public ushort DecodeUInt16(ReadOnlySpan<byte> value)
    {
        ushort result = 0;

        return BuildInteger(result, value);
    }

    public uint DecodeUInt32(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
        }

        uint result = 0;

        return BuildInteger(result, value);
    }

    public ulong DecodeUInt64(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
        }

        ulong result = 0;

        return BuildInteger(result, value);
    }

    /// <summary>
    ///     Validates that the left justified numeric values are encoded correctly
    /// </summary>
    /// <exception cref="EmvEncodingFormatException"></exception>
    private bool ValidateNumericEncoding(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!_CharMap.ContainsKey(value[i]))
            {
                throw new EmvEncodingFormatException(
                    $"The argument could not be parsed. The argument contained the value: [{value[i]}], which is an invalid {nameof(CompressedNumeric)} encoding");
            }
        }

        return true;
    }

    /// <summary>
    ///     Validates that the left justified numeric values are encoded correctly
    /// </summary>
    /// <exception cref="EmvEncodingFormatException"></exception>
    private bool IsNumericEncodingValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!_CharMap.ContainsKey(value[i]))
                return false;
        }

        return true;
    }

    private int GetPaddingIndexFromEnd(ReadOnlySpan<byte> value)
    {
        for (int i = value.Length; i > 0; i--)
        {
            if (value[i] != _PaddingByteKey)
                return i;
        }

        return 0;
    }

    private dynamic BuildInteger(dynamic resultBuffer, ReadOnlySpan<byte> value)
    {
        if (resultBuffer != byte.MinValue)
            resultBuffer = 0;

        for (byte i = 0; i < value.Length; i++)
        {
            resultBuffer += DecodeByte(value[i]);
            resultBuffer <<= 8;
        }

        return resultBuffer;
    }

    public byte[] TrimTrailingBytes(ReadOnlySpan<byte> value)
    {
        int padding = 0;

        for (int i = value.Length; i > 0; i--)
        {
            if (((value[i] >> 4) == _PaddedNibble) && (value[i].GetMaskedValue(0xF0) == _PaddedNibble))
            {
                padding++;

                continue;
            }

            break;
        }

        byte[] result = value[..^padding].ToArray();
        if (result[^1].GetMaskedValue(0xF0) == _PaddedNibble)
            result[^1] = result[^1].GetMaskedValue(0x0F);

        return result;
    }

    public void Validate(ReadOnlySpan<byte> value)
    {
        ValidateNumericEncoding(value[..^GetPaddingIndexFromEnd(value)]);
    }

    #endregion

    // /////////////
}