using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Specifications;

namespace Play.Core.Extensions.IEnumerable
{
    public static class ByteArrayExtensions
    {
        #region Instance Members

        public static BigInteger AsBigInteger(this IEnumerable<byte> value) => new(value.AsSpan());

        public static ReadOnlySpan<byte> AsSpan(this IEnumerable<byte> value)
        {
            Span<byte> result = new();

            for (int i = 0; i < value.Count(); i++)
                result[i] = value.ElementAt(i);

            return result;
        }

        #endregion
    }
}