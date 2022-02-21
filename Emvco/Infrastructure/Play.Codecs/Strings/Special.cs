using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Exceptions;

namespace Play.Codecs.Strings;

public class Special : PlayEncoding
{
    #region Static Metadata

    // 32 - 126
    private static readonly ImmutableSortedDictionary<char, byte> _ByteMapper = Enumerable.Range(32, 47 - 32)
        .Concat(Enumerable.Range(58, 64 - 58)).Concat(Enumerable.Range(91, 96 - 91)).Concat(Enumerable.Range(123, 126 - 123))
        .ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMapper = Enumerable.Range(32, 47 - 32)
        .Concat(Enumerable.Range(58, 64 - 58)).Concat(Enumerable.Range(91, 96 - 91)).Concat(Enumerable.Range(123, 126 - 123))
        .ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

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

    public bool IsValid(byte value) => _CharMapper.Keys.Contains(value);
    public bool IsValid(char value) => _ByteMapper.Keys.Contains(value);

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

        return _ByteMapper[value];
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        Validate(value);

        byte[] byteArray = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
            byteArray[i] = _ByteMapper[value[i]];

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