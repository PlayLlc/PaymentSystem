using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs.Strings;

// TODO: need to move Play.Codec.CompressedNumericCodec logic into here
public class CompressedNumericCodec : PlayCodec
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = new(typeof(CompressedNumericCodec));

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (byte) a, b => (char) (b + 48));

    private static readonly ImmutableSortedDictionary<char, byte> _ByteMap =
        Enumerable.Range(0, 10).ToImmutableSortedDictionary(a => (char) (a + 48), b => (byte) b);

    private const byte _PadValue = 0xF;

    #endregion

    #region Instance Members

    public BigInteger DecodeToBigInteger(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        BigInteger result = 0;

        return BuildInteger(result, value);
    }

    public byte DecodeToByte(byte value)
    {
        int leftNibble = value >> 4;
        byte rightNibble = (byte) (value & ~0xF0);

        return (byte) ((leftNibble * 10) + rightNibble);
    }

    /// <summary>
    ///     Takes the NumericCodec encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint DecodeToUInt32(ReadOnlySpan<byte> value)
    {
        if (!IsValid(value))
            throw new ArgumentOutOfRangeException(nameof(value));

        uint result = 0;

        return (uint) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the NumericCodec encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ulong DecodeToUInt64(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        ulong result = 0;

        return (ulong) BuildInteger(result, value);
    }

    /// <summary>
    ///     Takes the NumericCodec encoded byte array provided to <see cref="value" /> and returns an integer value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ushort DecodeToUInt16(ReadOnlySpan<byte> value)
    {
        ushort result = 0;

        return (ushort) BuildInteger(result, value);
    }

    private static byte DecodeToByte(char leftChar, char rightChar)
    {
        byte result = _ByteMap[leftChar];
        result *= 10;
        result += _ByteMap[rightChar];

        return result;
    }

    private dynamic BuildInteger(dynamic resultBuffer, ReadOnlySpan<byte> value)
    {
        if (resultBuffer != byte.MinValue)
            resultBuffer = 0;

        for (int i = 0, j = (value.Length * 2) - 2; i < value.Length; i++, j -= 2)
            resultBuffer += DecodeToByte(value[i]) * Math.Pow(10, j);

        return resultBuffer;
    }

    public override bool IsValid(ReadOnlySpan<char> value) => throw new NotImplementedException();
    public override bool IsValid(ReadOnlySpan<byte> value) => throw new NotImplementedException();

    public override int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) =>
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

    public override byte[] Encode(ReadOnlySpan<char> value) => throw new NotImplementedException();
    public override bool TryEncoding(ReadOnlySpan<char> value, out byte[] result) => throw new NotImplementedException();
    public override int GetByteCount(char[] chars, int index, int count) => throw new NotImplementedException();
    public override int GetMaxByteCount(int charCount) => throw new NotImplementedException();

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        int length = value.Length * 2;

        if (length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<char> buffer = stackalloc char[length];

            for (int i = 0, j = 0; i < value.Length; i++, j += 2)
                DecodeToChars(value[i], buffer, j);

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
                DecodeToChars(value[i], buffer, j);

            return buffer.ToArray();
        }
    }

    private void DecodeToChars(byte value, Span<char> buffer, int offset)
    {
        buffer[offset++] = _CharMap[(byte) (value / 10)];
        buffer[offset] = _CharMap[(byte) (value % 10)];
    }

    public override int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) =>
        throw new NotImplementedException();

    public override int GetCharCount(byte[] bytes, int index, int count) => throw new NotImplementedException();
    public override int GetMaxCharCount(int byteCount) => throw new NotImplementedException();
    public override string DecodeToString(ReadOnlySpan<byte> value) => throw new NotImplementedException();
    public override bool TryDecodingToString(ReadOnlySpan<byte> value, out string result) => throw new NotImplementedException();

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