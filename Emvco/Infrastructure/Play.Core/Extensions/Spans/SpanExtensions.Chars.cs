using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Core.Extensions.Spans;

public static class SpanExtensions
{
    #region Instance Members

    public static char[] CopyValue(this Span<char> value)
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