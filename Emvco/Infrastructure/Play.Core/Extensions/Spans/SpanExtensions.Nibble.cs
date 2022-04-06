using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Core.Extensions;

public static partial class SpanExtensions
{
    #region Instance Members

    /// <summary>
    ///     If the length of the array is odd the least most significant byte's right nibble will be 0x0
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException"></exception>
    public static byte[] AsByteArray(this Span<byte> value)
    {
        byte[] result = new byte[(value.Length / 2) + (value.Length % 2)];

        for (int i = 0; i < value.Length; i++)
        {
            if ((i % 2) == 0)
                result[i / 2] = (byte) (value[i] << 4);
            else
                result[i / 2] |= value[i];
        }

        return result;
    }

    #endregion
}