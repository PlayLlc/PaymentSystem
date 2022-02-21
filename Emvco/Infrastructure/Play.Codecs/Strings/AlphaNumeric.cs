using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Exceptions;

namespace Play.Codecs.Strings;

/// <summary>
///     An encoder for encoding and decoding alphabetic and numeric ASCII characters
/// </summary>
/// <remarks>
///     Strict parsing is enforced. Exceptions will be raised if invalid data is attempted to be parsed
/// </remarks>
public class AlphaNumeric : PlayEncoding
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMapper = new Dictionary<char, byte>
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
        {'9', 57},
        {'A', 65},
        {'B', 66},
        {'C', 67},
        {'D', 68},
        {'E', 69},
        {'F', 70},
        {'G', 71},
        {'H', 72},
        {'I', 73},
        {'J', 74},
        {'K', 75},
        {'L', 76},
        {'M', 77},
        {'N', 78},
        {'O', 79},
        {'P', 80},
        {'Q', 81},
        {'R', 82},
        {'S', 83},
        {'T', 84},
        {'U', 85},
        {'V', 86},
        {'W', 87},
        {'X', 88},
        {'Y', 89},
        {'Z', 90},
        {'a', 97},
        {'b', 98},
        {'c', 99},
        {'d', 100},
        {'e', 101},
        {'f', 102},
        {'g', 103},
        {'h', 104},
        {'i', 105},
        {'j', 106},
        {'k', 107},
        {'l', 108},
        {'m', 109},
        {'n', 110},
        {'o', 111},
        {'p', 112},
        {'q', 113},
        {'r', 114},
        {'s', 115},
        {'t', 116},
        {'u', 117},
        {'v', 118},
        {'w', 119},
        {'x', 120},
        {'y', 121},
        {'z', 122}
    }.ToImmutableSortedDictionary();

    private static readonly ImmutableSortedDictionary<int, char> _CharMapper = new Dictionary<int, char>
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
        {57, '9'},
        {65, 'A'},
        {66, 'B'},
        {67, 'C'},
        {68, 'D'},
        {69, 'E'},
        {70, 'F'},
        {71, 'G'},
        {72, 'H'},
        {73, 'I'},
        {74, 'J'},
        {75, 'K'},
        {76, 'L'},
        {77, 'M'},
        {78, 'N'},
        {79, 'O'},
        {80, 'P'},
        {81, 'Q'},
        {82, 'R'},
        {83, 'S'},
        {84, 'T'},
        {85, 'U'},
        {86, 'V'},
        {87, 'W'},
        {88, 'X'},
        {89, 'Y'},
        {90, 'Z'},
        {97, 'a'},
        {98, 'b'},
        {99, 'c'},
        {100, 'd'},
        {101, 'e'},
        {102, 'f'},
        {103, 'g'},
        {104, 'h'},
        {105, 'i'},
        {106, 'j'},
        {107, 'k'},
        {108, 'l'},
        {109, 'm'},
        {110, 'n'},
        {111, 'o'},
        {112, 'p'},
        {113, 'q'},
        {114, 'r'},
        {115, 's'},
        {116, 't'},
        {117, 'u'},
        {118, 'v'},
        {119, 'w'},
        {120, 'x'},
        {121, 'y'},
        {122, 'z'}
    }.ToImmutableSortedDictionary();

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public bool IsValid(byte value)
    {
        const byte bigA = (byte) 'A';
        const byte littleA = (byte) 'a';
        const byte bigZ = (byte) 'Z';
        const byte littleZ = (byte) 'z';
        const byte zero = (byte) '0';
        const byte nine = (byte) '9';

        return value is >= bigA and <= bigZ or >= littleA and <= littleZ or >= zero and <= nine;
    }

    public bool IsValid(char value)
    {
        const char bigA = 'A';
        const char littleA = 'a';
        const char bigZ = 'Z';
        const char littleZ = 'z';
        const char zero = '0';
        const char nine = '9';

        return value is >= bigA and <= bigZ or >= littleA and <= littleZ or >= zero and <= nine;
    }

    /// <exception cref="EncodingException"></exception>
    private void Validate(byte value)
    {
        if (!IsValid(value))
            throw new EncodingException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    /// <exception cref="EncodingException"></exception>
    private void Validate(char value)
    {
        if (!IsValid(value))
            throw new EncodingException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    /// <exception cref="EncodingException"></exception>
    protected void Validate(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        for (int i = 0; i <= (value.Length - 1); i++)
        {
            if (!IsValid(value[i]))
                throw new EncodingException(EncodingException.CharacterArrayContainsInvalidValue);
        }
    }

    /// <exception cref="EncodingException"></exception>
    private void Validate(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        for (int i = 0; i <= (value.Length - 1); i++)
        {
            if (!IsValid(value[i]))
                throw new EncodingException(EncodingException.CharacterArrayContainsInvalidValue);
        }
    }

    /// <summary>
    ///     GetByte
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public byte GetByte(char value)
    {
        Validate(value);

        return (byte) value;
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        Validate(value);

        byte[] byteArray = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
        {
            if (!_ByteMapper.ContainsKey(value[i]))
                throw new EncodingException(EncodingException.CharacterArrayContainsInvalidValue);

            byteArray[i] = _ByteMapper[value[i]];
        }

        return byteArray;
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        if (charIndex > chars.Length)
            throw new ArgumentNullException();

        if (byteIndex > bytes.Length)
            throw new ArgumentOutOfRangeException();

        if ((byteIndex + charCount) > bytes.Length)
            throw new ArgumentOutOfRangeException();

        if (charIndex > chars.Length)
            throw new ArgumentOutOfRangeException();

        if ((byteIndex + charCount) > (bytes.Length - byteIndex))
            throw new ArgumentOutOfRangeException();

        if ((bytes.Length - byteIndex) < charCount)
            throw new ArgumentOutOfRangeException(nameof(bytes), "The byte array buffer provided was smaller than expected");

        for (int i = charIndex, j = byteIndex; j < charCount; i++, j++)
            bytes[j] = _ByteMapper[chars[i]];

        return charCount;
    }

    /// <exception cref="EncodingException">Ignore.</exception>
    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        if (IsValid(value))
        {
            result = GetBytes(value);

            return true;
        }

        result = Array.Empty<byte>();

        return false;
    }

    public override int GetByteCount(char[] chars, int index, int count) => count;
    public override int GetMaxByteCount(int charCount) => charCount;

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        if (byteIndex > chars.Length)
            throw new ArgumentNullException();

        if (charIndex > chars.Length)
            throw new ArgumentOutOfRangeException();

        if ((charIndex + byteCount) > bytes.Length)
            throw new ArgumentOutOfRangeException();

        if (byteIndex > bytes.Length)
            throw new ArgumentOutOfRangeException();

        if ((charIndex + byteCount) > (chars.Length - charIndex))
            throw new ArgumentOutOfRangeException();

        if ((chars.Length - charIndex) < byteCount)
            throw new ArgumentOutOfRangeException(nameof(bytes), "The byte array buffer provided was smaller than expected");

        for (int i = byteIndex, j = charIndex; j < byteCount; i++, j++)
            bytes[j] = _ByteMapper[chars[i]];

        return byteCount;
    }

    public char[] GetChars(ReadOnlySpan<byte> value)
    {
        char[] result = new char[value.Length];
        for (int i = 0; i < value.Length; i++)
            result[i] = _CharMapper[value[i]];

        return result;
    }

    public override int GetCharCount(byte[] bytes, int index, int count) => count;
    public override int GetMaxCharCount(int byteCount) => byteCount;

    /// <exception cref="EncodingException"></exception>
    public override string GetString(ReadOnlySpan<byte> value)
    {
        Validate(value);

        if (value.Length >= StackallocThreshold)
        {
            using SpanOwner<char> owner = SpanOwner<char>.Allocate(value.Length);
            Span<char> buffer = owner.Span;

            for (int i = 0; i <= (value.Length - 1); i++)
                buffer[i] = _CharMapper[value[i]];

            return new string(buffer);
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length];
            for (int i = 0; i <= (value.Length - 1); i++)
                buffer[i] = _CharMapper[value[i]];

            return new string(buffer);
        }
    }

    /// <exception cref="EncodingException">Ignore.</exception>
    public override bool TryGetString(ReadOnlySpan<byte> value, out string result)
    {
        result = string.Empty;

        if (IsValid(value))
        {
            result = GetString(value);

            return true;
        }

        return false;
    }

    /// <summary>
    ///     GetChar
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public char GetChar(byte value)
    {
        Validate(value);

        return _CharMapper[value];
    }

    #endregion
}