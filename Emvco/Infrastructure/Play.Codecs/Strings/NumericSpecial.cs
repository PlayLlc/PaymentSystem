using System;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Codecs.Strings;

/// <summary>
///     NumericCodec Special Consists of NumericCodec Hex values in the range of '0' – '9' per byte or Special Ascii
///     characters in
///     the ranges of 32 - 47, 58 - 64, 91 - 96, 123 - 126
/// </summary>
public class NumericSpecial : PlayEncoding
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = new(typeof(NumericSpecial));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap = Enumerable.Range(0, 10).Concat(Enumerable.Range(32, 47 - 32))
        .Concat(Enumerable.Range(58, 64 - 58)).Concat(Enumerable.Range(91, 96 - 91)).Concat(Enumerable.Range(123, 126 - 123))
        .ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap = Enumerable.Range(0, 10).Concat(Enumerable.Range(32, 47 - 32))
        .Concat(Enumerable.Range(58, 64 - 58)).Concat(Enumerable.Range(91, 96 - 91)).Concat(Enumerable.Range(123, 126 - 123))
        .ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!_ByteMap.Keys.Contains(value[i]))
                return false;
        }

        return true;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!_CharMap.Keys.Contains(value[i]))
                return false;
        }

        return true;
    }

    public bool IsValid(char value)
    {
        if (_ByteMap.Keys.Contains(value))
            return true;

        return false;
    }

    public new byte[] Encode(string value)
    {
        if (value.Length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[value.Length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
        else
        {
            SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                buffer[i] = _ByteMap[value[i]];

            return buffer.ToArray();
        }
    }

    public override int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        ReadOnlySpan<byte> buffer = Encode(chars[charIndex..(charIndex + charCount)]);

        for (int i = 0, j = byteIndex; i < (buffer.Length + byteIndex); i++, j++)
            bytes[j] = buffer[i];

        return buffer.Length;
    }

    public override byte[] Encode(ReadOnlySpan<char> value) => Encode(value, value.Length);

    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        if (value.Length == length)
            return Encode(value);

        if (value.Length > length)
            return Encode(value)[^length..];

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> tempBuffer = stackalloc char[value.Length];
            value.CopyTo(tempBuffer);

            return Encode(tempBuffer);
        }
        else
        {
            using SpanOwner<char> spanCharOwner = SpanOwner<char>.Allocate(length);
            Span<char> tempBuffer = spanCharOwner.Span;
            value.CopyTo(tempBuffer);

            return Encode(tempBuffer);
        }
    }

    public override bool TryEncoding(ReadOnlySpan<char> value, out byte[] result)
    {
        if (!IsValid(value))
        {
            result = Array.Empty<byte>();

            return false;
        }

        result = Encode(value);

        return true;
    }

    public override int GetByteCount(char[] chars, int index, int count) => count;
    public override int GetMaxByteCount(int charCount) => charCount;

    public override int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                buffer[i++] = _CharMap[(byte) value[i]];

            return buffer.ToArray();
        }
        else
        {
            SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                buffer[i++] = _CharMap[(byte) value[i]];

            return buffer.ToArray();
        }
    }

    public override int GetCharCount(byte[] bytes, int index, int count) => GetMaxCharCount(count);
    public override int GetMaxCharCount(int byteCount) => byteCount * 2;
    public override string DecodeToString(ReadOnlySpan<byte> value) => new(DecodeToChars(value));

    public override bool TryDecodingToString(ReadOnlySpan<byte> value, out string result)
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
}