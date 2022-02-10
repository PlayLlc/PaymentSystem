using System;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Codecs.Strings;

// TODO: need to move Play.Codec.AlphaNumericSpecial logic into here
public class AlphaNumericSpecial : PlayEncoding
{
    #region Static Metadata

    public static string Name = nameof(AlphaNumericSpecial);

    // 32 - 126
    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(32, 126 - 32).ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<char> value)
    {
        foreach (char character in value)
        {
            if (!_ByteMap.ContainsKey(character))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        foreach (byte character in value)
        {
            if (!_CharMap.ContainsKey(character))
                return false;
        }

        return true;
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        GetBytes(chars[charIndex..(charIndex + charCount)]).AsSpan().CopyTo(bytes[byteIndex..]);

        return charCount;
    }

    public override byte[] GetBytes(ReadOnlySpan<char> value)
    {
        if (value.Length > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            Span<byte> buffer = stackalloc byte[value.Length];

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
    }

    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = GetBytes(value);

        return true;
    }

    public override int GetByteCount(ReadOnlySpan<char> value) => value.Length;
    public int GetByteCount(ReadOnlySpan<byte> value) => value.Length;
    public override int GetByteCount(char[] chars, int index, int count) => count;
    public override int GetMaxByteCount(int charCount) => charCount;

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        GetChars(bytes[byteIndex..(byteIndex + byteCount)]).AsSpan().CopyTo(chars[charIndex..]);

        return byteCount;
    }

    public char[] GetChars(ReadOnlySpan<byte> value)
    {
        if (value.Length > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(value.Length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _CharMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length];

            for (int i = 0; i < value.Length; i++)
                buffer[i] = _CharMap[value[i]];

            return buffer.ToArray();
        }
    }

    public override int GetCharCount(byte[] bytes, int index, int count) => count;
    public override int GetMaxCharCount(int byteCount) => byteCount;
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

    #endregion
}