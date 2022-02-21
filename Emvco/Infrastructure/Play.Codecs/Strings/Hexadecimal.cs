using System;
using System.Numerics;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Codecs.Strings;

public class Hexadecimal : PlayEncoding
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = new(typeof(Hexadecimal));

    #endregion

    #region Instance Members

    /// <summary>
    ///     Checks if the value contains valid hexadecimal bytes
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        try
        {
            CheckCore.ForEmptySequence(value, nameof(value));

            return true;
        }
        catch (Exception)
        {
            // TODO: log and catch less ambiguous exceptions
            return false;
        }
    }

    /// <summary>
    ///     Checks if the value contains valid Hexadecimal characters
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        for (int i = 0; i < (value.Length - 1); i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public static bool IsValid(char c)
    {
        if ((c > 0xFF) || (c < 0))
            return false;

        return Lookup.CharToHex[c] != 0xFF;
    }

    /// <summary>
    ///     Encodes a valid Hexadecimal character into its equivalent byte value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    private static byte GetByte(char value)
    {
        if ((value > 0xFF) || (value < 0))
            throw new EncodingException(EncodingException.ByteWasOutOfRangeOfHexadecimalCharacter);

        byte result = Lookup.CharToHex[value];

        return result == 0xFF ? throw new EncodingException(EncodingException.ByteWasOutOfRangeOfHexadecimalCharacter) : result;
    }

    /// <exception cref="EncodingException"></exception>
    /// <exception cref="EncodingException"></exception>
    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        CheckCore.ForEmptySequence(chars, nameof(chars));

        if ((charCount + charIndex) > chars.Length)
            throw new ArgumentOutOfRangeException();

        if (byteIndex > (bytes.Length - 1))
            throw new ArgumentOutOfRangeException();

        int indexRangeEnd = charIndex + charCount;
        int indexRangeLength = indexRangeEnd - charIndex;
        int bytesEncoded = indexRangeLength / 2;

        if (bytes.Length < (byteIndex + bytesEncoded))
            throw new ArgumentOutOfRangeException(nameof(bytes), "The byte buffer passed to the argument is smaller than expected");

        if ((indexRangeLength % 2) != 0)
            throw new ArgumentOutOfRangeException();

        for (int i = charIndex, j = byteIndex; i <= indexRangeEnd; i += 2, j++)
            bytes[j] = (byte) ((GetByte(chars[i]) << 4) | GetByte(chars[i + 1]));

        return bytesEncoded;
    }

    /// <summary>
    ///     Encodes a sequence of characters into a byte array
    /// </summary>
    /// <param name="value"></param>
    /// < returns></returns>
    /// < exception cref="EncodingException"></exception>
    /// <exception cref="EncodingException"></exception>
    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        bool lengthIsEven = (value.Length % 2) == 0;
        byte[] result = new byte[lengthIsEven ? value.Length / 2 : (value.Length + 1) / 2];
        int resultIndex = 0;
        int stringIndex = 0;

        if (!lengthIsEven)
            result[resultIndex++] = (byte) (0x00 | GetByte(value[stringIndex++]));

        for (; resultIndex < result.Length; stringIndex += 2)
            result[resultIndex++] = (byte) ((GetByte(value[stringIndex]) << 4) | GetByte(value[stringIndex + 1]));

        return result;
    }

    /// <summary>
    ///     Encodes a sequence of characters into a byte array
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        try
        {
            CheckCore.ForEmptySequence(value, nameof(value));

            result = GetBytes(value);

            return true;
        }
        catch (EncodingException)
        {
            result = new byte[0];

            return false;
        }
    }

    public override int GetByteCount(char[] chars, int index, int count)
    {
        if ((index + count) > chars.Length)
            throw new ArgumentOutOfRangeException();

        if ((count % 2) != 0)
            throw new ArgumentOutOfRangeException();

        return count / 2;
    }

    public override int GetMaxByteCount(int charCount)
    {
        if (charCount.TryGetRemainder(2, out int resultWithoutRemainder) == 0)
            return resultWithoutRemainder;

        return resultWithoutRemainder + 1;
    }

    /// <summary>
    ///     GetChars
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="byteIndex"></param>
    /// <param name="byteCount"></param>
    /// <param name="chars"></param>
    /// <param name="charIndex"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        if (byteIndex > bytes.Length)
            throw new ArgumentOutOfRangeException();

        if ((byteIndex + byteCount) > bytes.Length)
            throw new ArgumentOutOfRangeException();

        if (charIndex > chars.Length)
            throw new ArgumentOutOfRangeException();

        if ((charIndex + (byteCount * 2)) > (chars.Length - charIndex))
            throw new ArgumentOutOfRangeException();

        Array.ConstrainedCopy(bytes, byteIndex, GetChars(bytes[byteIndex..byteCount]), 0, byteCount);

        return byteCount;
    }

    /// <summary>
    ///     Decodes a sequence of bytes into a Hexadecimal string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public char[] GetChars(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length >= StackallocThreshold)
        {
            using SpanOwner<char> owner = SpanOwner<char>.Allocate(value.Length * 2);
            Span<char> buffer = owner.Span;
            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                ToCharsBuffer(value[i], buffer, j);

            return buffer.ToArray();
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length * 2];
            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                ToCharsBuffer(value[i], buffer, j);

            return buffer.ToArray();
        }
    }

    public override int GetCharCount(byte[] bytes, int index, int count)
    {
        if (index > bytes.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        if ((index + count) > bytes.Length)
            throw new ArgumentOutOfRangeException();

        return (index + count) * 2;
    }

    public override int GetMaxCharCount(int byteCount) => byteCount * 2;

    /// <summary>
    ///     Decodes a sequence of bytes into a Hexadecimal string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public override string GetString(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length >= StackallocThreshold)
        {
            using SpanOwner<char> owner = SpanOwner<char>.Allocate(value.Length * 2);
            Span<char> buffer = owner.Span;
            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                ToCharsBuffer(value[i], buffer, j);

            return new string(buffer.ToArray());
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length * 2];
            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                ToCharsBuffer(value[i], buffer, j);

            return new string(buffer.ToArray());
        }
    }

    /// <summary>
    ///     Decodes a sequence of bytes into a Hexadecimal string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public override bool TryGetString(ReadOnlySpan<byte> value, out string result)
    {
        try
        {
            CheckCore.ForEmptySequence(value, nameof(value));

            result = GetString(value);

            return true;
        }
        catch (EncodingException)
        {
            result = null;

            return false;
        }
    }

    public BigInteger GetBigInteger(ReadOnlySpan<char> value) => UnsignedInteger.GetBigInteger(GetBytes(value));

    /// <summary>
    ///     Returns an unsigned integer from the Hexadecimal string provided
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ushort GetUInt16(ReadOnlySpan<char> value) => UnsignedInteger.GetUInt16(GetBytes(value));

    /// <summary>
    ///     Returns an unsigned integer from the Hexadecimal string provided
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint GetUInt32(ReadOnlySpan<char> value) => UnsignedInteger.GetUInt32(GetBytes(value));

    /// <summary>
    ///     Returns an unsigned integer from the Hexadecimal string provided
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ulong GetUIn64(ReadOnlySpan<char> value) => UnsignedInteger.GetUInt64(GetBytes(value));

    private static void ToCharsBuffer(byte value, Span<char> buffer, int startingIndex = 0)
    {
        uint difference = (((value & 0xF0U) << 4) + (value & 0x0FU)) - 0x8989U;
        uint packedResult = (((uint) -(int) difference & 0x7070U) >> 4) + difference + 0xB9B9U;

        buffer[startingIndex + 1] = (char) (packedResult & 0xFF);
        buffer[startingIndex] = (char) (packedResult >> 8);
    }

    #endregion

    private static class Lookup
    {
        #region Instance Values

        public static ReadOnlySpan<char> CharDictionary =>
            new[]
            {
                '0', '1', '2', '3', '4', '5', '6', '7',
                '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
            };

        /// <summary>Map from an ASCII char to its hex value, e.g. arr['B'] == 11. 0xFF means it's not a hex digit.</summary>
        public static ReadOnlySpan<byte> CharToHex =>
            new byte[]
            {
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 15
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 31
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 47
                0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7,
                0x8, 0x9, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 63
                0xFF, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 79
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 95
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 111
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 127
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 143
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 159
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 175
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 191
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 207
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 223
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 239
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF // 255
            };

        #endregion
    }
}