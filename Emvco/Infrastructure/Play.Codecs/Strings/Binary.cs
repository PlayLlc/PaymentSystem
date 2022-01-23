using System;
using System.Collections.Generic;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs.Strings;

public class Binary : PlayEncoding
{
    #region Static Metadata

    private static readonly Dictionary<byte, char[]> _CharArrayMap = new()
    {
        {0b0000, new[] {'0', '0', '0', '0'}},
        {0b0001, new[] {'0', '0', '0', '1'}},
        {0b0010, new[] {'0', '0', '1', '0'}},
        {0b0011, new[] {'0', '0', '1', '1'}},
        {0b0100, new[] {'0', '1', '0', '0'}},
        {0b0101, new[] {'0', '1', '0', '1'}},
        {0b0110, new[] {'0', '1', '1', '0'}},
        {0b0111, new[] {'0', '1', '1', '1'}},
        {0b1000, new[] {'1', '0', '0', '0'}},
        {0b1001, new[] {'1', '0', '0', '1'}},
        {0b1010, new[] {'1', '0', '1', '0'}},
        {0b1011, new[] {'1', '0', '1', '1'}},
        {0b1100, new[] {'1', '1', '0', '0'}},
        {0b1101, new[] {'1', '1', '0', '1'}},
        {0b1110, new[] {'1', '1', '1', '0'}},
        {0b1111, new[] {'1', '1', '1', '1'}}
    };

    private static readonly Dictionary<Memory<char>, byte> _NibbleMap = new()
    {
        {new[] {'0', '0', '0', '0'}, 0b0000},
        {new[] {'0', '0', '0', '1'}, 0b0001},
        {new[] {'0', '0', '1', '0'}, 0b0010},
        {new[] {'0', '0', '1', '1'}, 0b0011},
        {new[] {'0', '1', '0', '0'}, 0b0100},
        {new[] {'0', '1', '0', '1'}, 0b0101},
        {new[] {'0', '1', '1', '0'}, 0b0110},
        {new[] {'0', '1', '1', '1'}, 0b0111},
        {new[] {'1', '0', '0', '0'}, 0b1000},
        {new[] {'1', '0', '0', '1'}, 0b1001},
        {new[] {'1', '0', '1', '0'}, 0b1010},
        {new[] {'1', '0', '1', '1'}, 0b1011},
        {new[] {'1', '1', '0', '0'}, 0b1100},
        {new[] {'1', '1', '0', '1'}, 0b1101},
        {new[] {'1', '1', '1', '0'}, 0b1110},
        {new[] {'1', '1', '1', '1'}, 0b1111}
    };

    #endregion

    #region Instance Members

    private void Validate(ReadOnlySpan<char> value)
    {
        if ((value.Length % 8) != 0)
            throw new ArgumentOutOfRangeException(nameof(value), $"The {nameof(Binary)} Encoding expects a string that is divisible by 8");

        for (int i = 0; i < value.Length; i++)
        {
            if ((value[i] != '0') && (value[i] != '1'))
            {
                throw new ArgumentOutOfRangeException(
                    $"The {nameof(Binary)} Encoding expects all string values to be either a '1' or a '0'");
            }
        }
    }

    public override bool IsValid(ReadOnlySpan<char> value)
    {
        if ((value.Length % 8) != 0)
            return false;

        for (int i = 0; i < value.Length; i++)
        {
            if ((value[i] != '0') && (value[i] != '1'))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value) => true;

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        throw new NotImplementedException();

    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        Validate(value);

        char[] charBuffer = new char[8];
        int byteCount = value.Length / 8;

        if (byteCount > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(byteCount);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++)
            {
                value[j..(j + 8)].CopyTo(charBuffer);
                GetByteFromString(charBuffer, j, buffer);
            }

            return buffer.ToArray();
        }
        else
        {
            Span<byte> buffer = stackalloc byte[byteCount];

            for (int i = 0, j = 0; i < value.Length; i++)
            {
                value[j..(j + 8)].CopyTo(charBuffer);
                GetByteFromString(charBuffer, j, buffer);
            }

            return buffer.ToArray();
        }
    }

    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result) => throw new NotImplementedException();
    public override int GetByteCount(char[] chars, int index, int count) => throw new NotImplementedException();
    public override int GetMaxByteCount(int charCount) => throw new NotImplementedException();

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public override int GetCharCount(byte[] bytes, int index, int count) => throw new NotImplementedException();
    public override int GetMaxCharCount(int byteCount) => throw new NotImplementedException();
    public string GetString(byte value) => System.Convert.ToString(value, 2);
    public string GetString(short value) => System.Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string GetString(ushort value) => System.Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string GetString(int value) => System.Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string GetString(uint value) => System.Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string GetString(long value) => System.Convert.ToString(value, 2).PadLeft(value.GetMostSignificantByte() * 8, '0');
    public string GetString(ulong value) => throw new NotImplementedException();

    public override string GetString(ReadOnlySpan<byte> value)
    {
        int charCount = value.Length * 8;

        if (charCount > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(charCount);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 8)
                GetStringFromByte(value[i], j, buffer);

            return new string(buffer);
        }
        else
        {
            Span<char> buffer = stackalloc char[charCount];
            for (int i = 0, j = 0; i < value.Length; i++, j += 8)
                GetStringFromByte(value[i], j, buffer);

            return new string(buffer);
        }
    }

    public override bool TryGetString(ReadOnlySpan<byte> value, out string result) => throw new NotImplementedException();

    private void GetByteFromString(char[] charBuffer, int offset, Span<byte> buffer)
    {
        if (charBuffer.Length != 8)
            throw new ArgumentOutOfRangeException(nameof(charBuffer), "The sequence must have a length of 8");

        buffer[offset] = (byte) (_NibbleMap[charBuffer[..4]] >> 4);
        buffer[offset] = _NibbleMap[charBuffer[4..]];
    }

    private void GetStringFromByte(byte value, int offset, Span<char> buffer)
    {
        _CharArrayMap[value.GetMaskedValue(0xF0)].CopyTo(buffer[..(offset + 4)]);
        _CharArrayMap[(byte) (value >> 4)].CopyTo(buffer[(offset + 4)..]);
    }

    #endregion
}