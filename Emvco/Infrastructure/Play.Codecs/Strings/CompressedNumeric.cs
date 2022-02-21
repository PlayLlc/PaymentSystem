using System;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs.Strings;

// TODO: need to move Play.Codec.CompressedNumeric logic into here
public class CompressedNumeric : PlayEncoding
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    private const byte _PadValue = 0xF;

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<char> value) => throw new NotImplementedException();
    public override bool IsValid(ReadOnlySpan<byte> value) => throw new NotImplementedException();

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
        throw new NotImplementedException();

    public static int GetPadCount(ReadOnlySpan<byte> value)
    {
        int offset = value.Length;

        for (; offset > 0;)
        {
            if (value[offset] != 0xFF)
                break;

            offset -= 2;
        }

        if (value[offset].AreBitsSet(0xF))
            offset++;

        return offset;
    }

    public override byte[] GetBytes(ReadOnlySpan<char> value) => throw new NotImplementedException();
    public override bool TryGetBytes(ReadOnlySpan<char> value, out byte[] result) => throw new NotImplementedException();
    public override int GetByteCount(char[] chars, int index, int count) => throw new NotImplementedException();
    public override int GetMaxByteCount(int charCount) => throw new NotImplementedException();

    public char[] GetChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                GetChars(value[i], buffer, j);

            string? a = buffer.ToArray().ToString();
            string? b = new(buffer);
            string? c = new(buffer.ToArray());

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(length);
            Span<char> buffer = spanOwner.Span;

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                GetChars(value[i], buffer, j);

            return buffer.ToArray();
        }
    }

    private void GetChars(byte value, Span<char> buffer, int offset)
    {
        buffer[offset++] = _CharMap[(byte) (value / 10)];
        buffer[offset] = _CharMap[(byte) (value % 10)];
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public override int GetCharCount(byte[] bytes, int index, int count) => throw new NotImplementedException();
    public override int GetMaxCharCount(int byteCount) => throw new NotImplementedException();
    public override string GetString(ReadOnlySpan<byte> value) => throw new NotImplementedException();
    public override bool TryGetString(ReadOnlySpan<byte> value, out string result) => throw new NotImplementedException();

    public int GetNumberOfDigits(byte[] value)
    {
        int padCount = 0;

        for (int i = value.Length; i > 0; i--)
        {
            if (value[i].GetRightNibble() != _PadValue)
                break;

            padCount++;

            if (value[i].GetRightNibble() != _PadValue)
                break;

            padCount++;
        }

        return (value.Length * 2) - padCount;
    }

    #endregion
}