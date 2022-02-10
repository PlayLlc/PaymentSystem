using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs.Integers;

public class SignedInteger : PlayEncoding
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        Enumerable.Range(48, 57 - 48).ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(48, 57 - 48).ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

    public static string Name = nameof(SignedInteger);

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

    private bool IsValid(char value)
    {
        const char minCharValue = '0';
        const char maxCharValue = '9';

        if (value is >= minCharValue and <= maxCharValue)
            return true;

        return false;
    }

    private bool IsValid(byte value)
    {
        const byte minCharValue = (byte) '0';
        const byte maxCharValue = (byte) '9';

        if (value is >= minCharValue and <= maxCharValue)
            return true;

        return false;
    }

    private void Validate(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Validate(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                throw new ArgumentOutOfRangeException();
        }
    }

    public sbyte GetByte(ReadOnlySpan<byte> value)
    {
        if (value[0] > sbyte.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value));

        return (sbyte) value[0];
    }

    public byte[] GetBytes(sbyte value) => BitConverter.GetBytes(value);
    public byte[] GetBytes(short value) => BitConverter.GetBytes(value);
    public byte[] GetBytes(int value) => BitConverter.GetBytes(value);
    public byte[] GetBytes(long value) => BitConverter.GetBytes(value);

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        Validate(chars[charIndex..charCount]);

        Array.ConstrainedCopy(GetBytes(chars), charIndex, bytes, byteIndex, charCount);

        return charCount;
    }

    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        ReadOnlySpan<char> buffer = value[0] == '-' ? value[1..] : value;
        Validate(value);
        byte[] result = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
            result[i] = _ByteMap[value[i]];

        return result;
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

    public override int GetByteCount(char[] chars, int index, int count) => GetBytes(chars, index, count).Length;

    public override int GetMaxByteCount(int charCount)
    {
        double maximumIntegerResult = Math.Pow(10, charCount) - 1;

        double result = Math.Log(maximumIntegerResult, 2);

        return (int) ((result % 1) == 0 ? result : result + 1);
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        dynamic integerValue = GetInteger(bytes[byteIndex..byteCount]);

        for (int i = charIndex; i < integerValue.GetNumberOfDigits(); i++)
        {
            chars[i] = _CharMap[(byte) (integerValue % 10)];
            integerValue /= 10;
        }

        return byteCount;
    }

    public char[] GetChars(ReadOnlySpan<byte> value)
    {
        Validate(value);

        dynamic integerValue = GetInteger(value);
        char[] result = new char[integerValue.GetNumberOfDigits()];

        result[0] = '-';

        for (int i = 1; i < integerValue.GetNumberOfDigits(); i++)
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

        dynamic integerValue = GetInteger(value);
        result = new char[integerValue.GetNumberOfDigits()];

        for (int i = 0; i < integerValue.GetNumberOfDigits(); i++)
        {
            result[i] = _CharMap[(byte) (integerValue % 10)];
            integerValue /= 10;
        }

        return true;
    }

    public override int GetCharCount(byte[] bytes, int index, int count) => GetChars(bytes, index, count).Length;

    public nint GetCharCount(sbyte value)
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

    public nint GetCharCount(short value)
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

    public nint GetCharCount(int value)
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

    public nint GetCharCount(long value)
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

    public nint GetCharCount(BigInteger value) => GetCharCount(value.ToByteArray());

    // There is an extra character for a negative sign '-'
    public override int GetMaxCharCount(int byteCount) => ((BigInteger) Math.Pow(2, byteCount * 8)).GetNumberOfDigits() + 1;
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

    public short GetInt16(ReadOnlySpan<byte> value)
    {
        const byte byteLength = Specs.Integer.Int16.ByteSize;
        const ushort max = ushort.MaxValue;
        short result = 0;
        byte bitShift = 0;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return GetInt16(value);
        }

        if (value.Length > byteLength)
            return GetInt16(value[..byteLength]);

        for (int i = 0; i < value.Length; i++)
        {
            result |= (short) (value[i] << bitShift);
            bitShift += 8;
        }

        result |= (short) (max & (ushort) ~GetMask(value.Length));

        return result;
    }

    public int GetInt32(ReadOnlySpan<byte> value)
    {
        const uint max = uint.MaxValue;
        const byte byteLength = Specs.Integer.Int32.ByteSize;
        int result = 0;
        byte bitShift = 0;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return GetInt32(value);
        }

        if (value.Length > byteLength)
            return GetInt32(value[..byteLength]);

        for (int i = 0; i < value.Length; i++)
        {
            result |= value[i] << bitShift;
            bitShift += 8;
        }

        result |= (int) (max & (uint) ~GetMask(value.Length));

        return result;
    }

    public long GetInt64(ReadOnlySpan<byte> value)
    {
        const ulong max = ulong.MaxValue;
        const byte byteLength = Specs.Integer.Int64.ByteSize;
        long result = 0;
        int bitShift = 0;

        if (value.Length < byteLength)
        {
            Span<byte> buffer = stackalloc byte[byteLength];
            value.CopyTo(buffer);

            return GetInt64(value);
        }

        if (value.Length > byteLength)
            return GetInt64(value[..byteLength]);

        for (int i = 0; i < value.Length; i++)
        {
            result |= (long) value[i] << bitShift;
            bitShift += 8;
        }

        result |= (long) (max & (ulong) ~GetMask(value.Length));

        return result;
    }

    public dynamic GetInteger(ReadOnlySpan<byte> value)
    {
        return value.Length switch
        {
            1 => GetByte(value),
            2 => GetInt16(value),
            4 => GetInt32(value),
            8 => GetInt64(value),
            _ => new BigInteger(value)
        };
    }

    private dynamic GetMask(int byteCount)
    {
        const byte byteMax = byte.MaxValue;
        int result = 0;
        for (int i = 0; i < byteCount; i++)
            result |= byteMax << (i * 8);

        return result;
    }

    #endregion
}