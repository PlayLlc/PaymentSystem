using System;

using Play.Core.Exceptions;

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
    public static byte[] AsByteArray(this Span<Nibble> value)
    {
        byte[] result = new byte[(value.Length / 2) + (value.Length % 2)];

        for (int i = 0; i < value.Length; i++)
            if ((i % 2) == 0)
                result[i / 2] = (byte) (value[i] << 4);
            else
                result[i / 2] |= value[i];

        return result;
    }

    /// <exception cref="PlayInternalException"></exception>
    public static void CopyTo(this Span<Nibble> value, Span<byte> buffer)
    {
        int byteCount = (value.Length / 2) + (value.Length % 2);

        if (buffer.Length < byteCount)
            throw new PlayInternalException($"The {nameof(buffer)} argument was too small to perform the {nameof(CopyTo)} sequence");

        for (int i = 0; i < value.Length; i++)
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) (value[i] << 4);
            else
                buffer[i / 2] |= value[i];
    }

    #endregion
}