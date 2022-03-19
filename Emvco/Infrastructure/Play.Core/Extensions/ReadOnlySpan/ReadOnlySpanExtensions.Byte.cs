using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Core.Extensions;

public static partial class ReadOnlySpanExtensions
{
    #region Instance Members

    public static bool IsValueEqual(this ReadOnlySpan<byte> value, ReadOnlySpan<byte> other)
    {
        if (value.Length != other.Length)
            return false;

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != other[i])
                return false;
        }

        return true;
    }

    public static byte[] ConcatArrays(this ReadOnlySpan<byte> value, ReadOnlySpan<byte> other)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + other.Length);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        other.CopyTo(buffer[value.Length..]);

        return buffer.ToArray();
    }

    public static byte[] LeftPaddedArray(this ReadOnlySpan<byte> value, byte padValue, int padCount)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + padCount);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer[^value.Length..]);
        buffer[..padCount].Fill(padValue);

        return buffer.ToArray();
    }

    public static byte[] ReverseBits(this ReadOnlySpan<byte> value)
    {
        byte[] result = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
            result[i] = value[i].ReverseBits();

        return result;
    }

    public static byte[] RightPaddedArray(this ReadOnlySpan<byte> value, byte padValue, int padCount)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + padCount);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        buffer[^padCount..].Fill(padValue);

        return buffer.ToArray();
    }

    public static int GetLeftPaddedZeroCount(this ReadOnlySpan<byte> value)
    {
        int result = 0;
        int offset = 0;

        for (; offset < value.Length; offset++)
        {
            if (value[offset] != offset)
                break;

            result += 2;
        }

        for (; offset < value.Length; offset++)
        {
            if ((value[offset] >> 4) != 0)
                break;

            result++;

            if (value[offset].GetMaskedValue(0xF0) != 0)
                break;

            result++;
        }

        return result;
    }

    #endregion
}