using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;

using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs.Integers;

public class UnsignedInteger : PlayEncoding
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = new(nameof(UnsignedInteger));

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

    private static readonly ImmutableSortedDictionary<int, char> _CharMap = new Dictionary<int, char>
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

    #endregion

    #region Instance Members

    public PlayEncodingId GetPlayEncodingId() => PlayEncodingId;
    public override bool IsValid(ReadOnlySpan<byte> value) => true;

    public override bool IsValid(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    private bool IsValid(char value)
    {
        const char minIntegerChar = '0';
        const char maxIntegerChar = '0';

        return value is >= minIntegerChar and <= maxIntegerChar;
    }

    protected void Validate(ReadOnlySpan<char> value)
    { }

    // ///////////////// 

    public byte[] GetBytes(ushort value, bool trimEmptyBytes = false)
    {
        const byte byteCount = sizeof(ushort);

        return trimEmptyBytes ? GetTrimmedBytes(value) : GetAllBytes(value, byteCount);
    }

    public byte[] GetBytes(uint value, bool trimEmptyBytes = false)
    {
        const byte byteCount = sizeof(uint);

        return trimEmptyBytes ? GetTrimmedBytes(value) : GetAllBytes(value, byteCount);
    }

    public byte[] GetBytes(ulong value, bool trimEmptyBytes = false)
    {
        const byte byteCount = sizeof(uint);

        return trimEmptyBytes ? GetTrimmedBytes(value) : GetAllBytes(value, byteCount);
    }

    public byte[] GetBytes(BigInteger value) => value.ToByteArray();

    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        Validate(value);
        byte[] result = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
            result[i] = _ByteMap[value[i]];

        return result;
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        Validate(chars[charIndex..charCount]);

        Array.ConstrainedCopy(GetBytes(chars), charIndex, bytes, byteIndex, charCount);

        return charCount;
    }

    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
            result[i] = _ByteMap[value[i]];

        return true;
    }

    public override int GetByteCount(char[] chars, int index, int count) => GetBytes(chars).Length;

    // you're smarter than you think you are - this checks out
    public override int GetMaxByteCount(int charCount)
    {
        double maximumIntegerResult = Math.Pow(10, charCount) - 1;

        double result = Math.Log(maximumIntegerResult, 2);

        return (int) ((result % 1) == 0 ? result : result + 1);
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        // TODO: optimize this later instead of defaulting to BigInteger
        BigInteger integerValue = GetBigInteger(bytes[byteIndex..byteCount]);

        for (int i = charIndex; i < integerValue.GetNumberOfDigits(); i++)
        {
            chars[i] = _CharMap[(byte) (integerValue % 10)];
            integerValue /= 10;
        }

        return byteCount;
    }

    public char[] GetChars(ReadOnlySpan<byte> value)
    {
        // TODO: optimize this later instead of defaulting to BigInteger
        BigInteger integerValue = GetBigInteger(value);
        char[] result = new char[integerValue.GetNumberOfDigits()];

        for (int i = 0; i < integerValue.GetNumberOfDigits(); i++)
        {
            result[i] = _CharMap[(byte) (integerValue % 10)];
            integerValue /= 10;
        }

        return result;
    }

    public bool TryGetChars(ReadOnlySpan<byte> value, out char[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<char>();

            return false;
        }

        // TODO: optimize this later instead of defaulting to BigInteger
        BigInteger integerValue = GetBigInteger(value);
        result = new char[integerValue.GetNumberOfDigits()];

        for (int i = 0; i < integerValue.GetNumberOfDigits(); i++)
        {
            result[i] = _CharMap[(byte) (integerValue % 10)];
            integerValue /= 10;
        }

        return true;
    }

    public nint GetCharCount(byte value)
    {
        const nint valueLength = 3;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(ushort value)
    {
        const nint valueLength = 5;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(uint value)
    {
        const nint valueLength = 10;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(ulong value)
    {
        const nint valueLength = 20;

        for (int i = 0; i < valueLength; i++)
        {
            if ((value % 10) == 0)
                return i;

            value /= 10;
        }

        return valueLength;
    }

    public nint GetCharCount(BigInteger value) => GetCharCount(value.ToByteArray());
    public override int GetCharCount(byte[] bytes, int index, int count) => throw new NotImplementedException();
    public override int GetMaxCharCount(int byteCount) => ((BigInteger) Math.Pow(2, byteCount * 8)).GetNumberOfDigits();
    public override string GetString(ReadOnlySpan<byte> value) => new(GetChars(value));

    public override bool TryGetString(ReadOnlySpan<byte> value, out string result)
    {
        if (!TryGetChars(value, out char[] buffer))
        {
            result = string.Empty;

            return false;
        }

        result = new string(buffer);

        return true;
    }

    public BigInteger GetBigInteger(ReadOnlySpan<byte> value) => new(value);

    public ushort GetUInt16(ReadOnlySpan<byte> value)
    {
        const byte byteLength = Specs.Integer.UInt16.ByteSize;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return GetUInt16(value);
        }

        if (value.Length > byteLength)
            return GetUInt16(value[..byteLength]);

        ushort result = 0;

        for (int i = 0; i < (value.Length - 1); i++)
        {
            result |= value[i];
            result <<= 8;
        }

        result |= value[^1];

        return result;
    }

    public uint GetUInt32(ReadOnlySpan<byte> value)
    {
        const byte byteLength = Specs.Integer.UInt32.ByteSize;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return GetUInt32(value);
        }

        if (value.Length > byteLength)
            return GetUInt32(value[..byteLength]);

        uint result = 0;

        for (int i = 0; i < (value.Length - 1); i++)
        {
            result |= value[i];
            result <<= 8;
        }

        result |= value[^1];

        return result;
    }

    public ulong GetUInt64(ReadOnlySpan<byte> value)
    {
        const byte byteLength = Specs.Integer.UInt64.ByteCount;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return GetUInt64(value);
        }

        if (value.Length > byteLength)
            return GetUInt64(value[..byteLength]);

        ulong result = 0;

        for (int i = 0; i < (value.Length - 1); i++)
        {
            result |= value[i];
            result <<= 8;
        }

        result |= value[^1];

        return result;
    }

    private byte[] GetAllBytes(ulong value, byte byteCount)
    {
        Span<byte> buffer = stackalloc byte[byteCount];
        byte bitShift = 0;
        byte mostSignificantByte = value.GetMostSignificantByte();

        for (int i = 0, j = mostSignificantByte; i < mostSignificantByte; i++, j--)
        {
            buffer[j] = (byte) (value >> bitShift);
            bitShift += 8;
        }

        return buffer.ToArray();
    }

    private byte[] GetTrimmedBytes(ulong value)
    {
        byte mostSignificantByte = value.GetMostSignificantByte();
        Span<byte> buffer = stackalloc byte[mostSignificantByte];
        byte bitShift = 0;

        for (int i = mostSignificantByte - 1; i >= 0; i--)
        {
            buffer[i] = (byte) (value >> bitShift);
            bitShift += 8;
        }

        return buffer.ToArray();
    }

    #endregion
}