﻿using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static partial class ReadOnlySpanExtensions
{
    #region Instance Members

    public static bool IsValueEqual(this ReadOnlySpan<char> value, ReadOnlySpan<char> other)
    {
        if (value.Length != other.Length)
            return false;

        for (int i = 0; i < value.Length; i++)
            if (value[i] != other[i])
                return false;

        return true;
    }

    public static char[] CopyValue(this ReadOnlySpan<char> value)
    {
        if (value.Length > Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<char> spanOwner = SpanOwner<char>.Allocate(value.Length);
            Span<char> buffer = spanOwner.Span;

            value.CopyTo(buffer);

            return buffer.ToArray();
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length];
            value.CopyTo(buffer);

            return buffer.ToArray();
        }
    }

    #endregion
}