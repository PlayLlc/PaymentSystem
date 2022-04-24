using System;
using System.Numerics;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class ByteArrayExtensions
{
    #region Instance Members

    public static BigInteger AsBigInteger(this byte[] value) => new(value);

    public static byte[] ConcatArrays(this byte[] value, ReadOnlySpan<byte> other)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + other.Length);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        other.CopyTo(buffer[value.Length..]);

        return buffer.ToArray();
    }

    /// <exception cref="OverflowException"></exception>
    public static byte[] RemoveLeftPadding(this byte[] value, Nibble paddingValue)
    {
        int paddedNibbles = 0;
        LeftNibble leftNibble = new(paddingValue);
        RightNibble rightNibble = new(paddingValue);

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != leftNibble)
                break;

            paddedNibbles++;

            if (value[i] != rightNibble)
                break;

            paddedNibbles++;
        }

        if (value.Length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> result = stackalloc byte[value.Length - ((paddedNibbles / 2) + (paddedNibbles % 2))];
            value[^result.Length..].CopyTo(result);

            if ((paddedNibbles % 2) != 0)
                result[0] = result[0].GetMaskedValue(LeftNibble.MaxValue);

            return result.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length - ((paddedNibbles / 2) + (paddedNibbles % 2)));
            Span<byte> result = spanOwner.Span;
            value[^result.Length..].CopyTo(result);

            if ((paddedNibbles % 2) != 0)
                result[0] = result[0].GetMaskedValue(LeftNibble.MaxValue);

            return result.ToArray();
        }
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

    public static byte[] ShiftLeftOneNibble(this byte[] value)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + 1);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 1; i < value.Length; i++)
            buffer[i] = value[i].ShiftNibbleLeft(value[i + 1].GetLeftNibble());

        buffer[^1] = value[^1].ShiftNibbleLeft(0x00);

        return buffer.ToArray();
    }

    /// <exception cref="OverflowException"></exception>
    public static Nibble[] AsNibbleArray(this byte[] value)
    {
        Nibble[] result = new Nibble[value.Length * 2];

        for (nint i = 0; i < result.Length; i++)
        {
            if ((i % 2) == 0)
                result[i] = new Nibble((byte) (value[i / 2] >> 4));
            else
                result[i] = new Nibble(value[i / 2].GetMaskedValue(0xF0));
        }

        return result;
    }

    #endregion
}