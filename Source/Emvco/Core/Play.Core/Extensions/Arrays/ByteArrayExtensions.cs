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

        if (value.Length < Specs.ByteArray._StackAllocateCeiling)
        {
            Span<byte> result = stackalloc byte[value.Length - (paddedNibbles / 2)];
            value[^result.Length..].CopyTo(result);

            if ((paddedNibbles % 2) != 0)
                result[0] = result[0].GetMaskedValue(LeftNibble._MaxValue);

            return result.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length - ((paddedNibbles / 2) + (paddedNibbles % 2)));
            Span<byte> result = spanOwner.Span;
            value[^result.Length..].CopyTo(result);

            if ((paddedNibbles % 2) != 0)
                result[0] = result[0].GetMaskedValue(LeftNibble._MaxValue);

            return result.ToArray();
        }
    }

    public static byte[] CopyValue(this byte[] value)
    {
        if (value.Length > Specs.ByteArray._StackAllocateCeiling)
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

    /// <exception cref="OverflowException"></exception>
    public static Nibble[] AsNibbleArray(this byte[] value)
    {
        Nibble[] result = new Nibble[value.Length * 2];

        for (nint i = 0; i < result.Length; i++)
            if ((i % 2) == 0)
                result[i] = new Nibble((byte) (value[i / 2] >> 4));
            else
                result[i] = new Nibble(value[i / 2].GetMaskedValue(0xF0));

        return result;
    }

    #endregion
}