using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class ByteArrayExtensions
{
    #region Instance Members

    public static byte[] ConcatArrays(this byte[] value, ReadOnlySpan<byte> other)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + other.Length);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        other.CopyTo(buffer[value.Length..]);

        return buffer.ToArray();
    }

    public static byte[] ConcatArrays(this byte[] value, byte[] other)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + other.Length);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        other.CopyTo(buffer[value.Length..]);

        return buffer.ToArray();
    }

    public static byte[] CopyValue(this byte[] value)
    {
        if (value.Length > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
            Span<byte> buffer = spanOwner.Span;

            value.AsSpan().CopyTo(buffer);

            return buffer.ToArray();
        }
        else
        {
            Span<byte> buffer = stackalloc byte[value.Length];
            value.AsSpan().CopyTo(buffer);

            return buffer.ToArray();
        }
    }

    public static byte[] LeftPaddedArray(this byte[] value, byte padValue, int padCount)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + padCount);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer[^value.Length..]);
        buffer[..padCount].Fill(padValue);

        return buffer.ToArray();
    }

    public static byte[] RightPaddedArray(this byte[] value, byte padValue, int padCount)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + padCount);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        buffer[^padCount..].Fill(padValue);

        return buffer.ToArray();
    }

    /// <summary>
    ///     Shifts the array of bytes one nibble to the right. The far right nibble will be removed and the far left
    ///     nibble will be a 0x0 value
    /// </summary>
    /// <param name="value"></param>
    /// <param name=""></param>
    /// <returns></returns>
    public static byte[] ShiftRightOneNibble(this byte[] value)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = value[0].ShiftNibbleRight(0x0);

        for (int i = 1; i < value.Length; i++)
            buffer[i] = value[i].ShiftNibbleRight(value[i - 1].GetRightNibble());

        return buffer.ToArray();
    }

    #endregion
}