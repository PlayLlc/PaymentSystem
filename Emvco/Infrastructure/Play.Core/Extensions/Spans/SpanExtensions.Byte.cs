using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Core.Extensions;

public static partial class SpanExtensions
{
    #region Instance Members

    /// <exception cref="OverflowException"></exception>
    public static byte[] RemoveLeftPadding(this Span<byte> value, Nibble paddingValue)
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

        byte[] result = new byte[value.Length - ((paddedNibbles / 2) + (paddedNibbles % 2))];
        value[^result.Length..].CopyTo(result);

        if ((paddedNibbles % 2) != 0)
            result[0] = result[0].GetMaskedValue(LeftNibble.MaxValue);

        return result;
    }

    public static byte[] ConcatArrays(this Span<byte> value, ReadOnlySpan<byte> other)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + other.Length);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        other.CopyTo(buffer[value.Length..]);

        return buffer.ToArray();
    }

    public static byte[] LeftPaddedArray(this Span<byte> value, byte padValue, int padCount)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + padCount);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer[^value.Length..]);
        buffer[..padCount].Fill(padValue);

        return buffer.ToArray();
    }

    public static byte[] RightPaddedArray(this Span<byte> value, byte padValue, int padCount)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + padCount);
        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        buffer[^padCount..].Fill(padValue);

        return buffer.ToArray();
    }

    public static byte[] ShiftRightOneNibble(this Span<byte> value)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = value[0].ShiftNibbleRight(0x0);

        for (int i = 1; i < value.Length; i++)
            buffer[i] = value[i].ShiftNibbleRight(value[i - 1].GetRightNibble());

        return buffer.ToArray();
    }

    public static byte[] ShiftLeftOneNibble(this Span<byte> value)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length + 1);
        Span<byte> buffer = spanOwner.Span;

        for (int i = 1; i < value.Length; i++)
            buffer[i] = value[i].ShiftNibbleLeft(value[i + 1].GetLeftNibble());

        buffer[^1] = value[^1].ShiftNibbleLeft(0x00);

        return buffer.ToArray();
    }

    /// <exception cref="OverflowException"></exception>
    public static Nibble[] AsNibbleArray(this Span<byte> value)
    {
        Nibble[] result = new Nibble[value.Length * 2];

        for (int i = 0; i < result.Length; i++)
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