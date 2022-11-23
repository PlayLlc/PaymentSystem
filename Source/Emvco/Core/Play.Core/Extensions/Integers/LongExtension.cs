using System;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class LongExtension
{
    #region Instance Members

    public static long GetMaskedValue(this long value, long bitsToMask) => value & ~bitsToMask;

    public static int GetMostSignificantBit(this long value)
    {
        if (value == 0)
            return 0;

        if (value < 0)
            return Specs.Integer.Int64._BitCount;

        if (value < uint.MaxValue)
            return (int) Math.Log(value, 2) + 1;

        long buffer = value;
        int offset = 0;

        for (int i = 0; i < Specs.Integer.Int64._BitCount; i++)
        {
            if (buffer == 0)
                return offset;

            if ((buffer % 10) != 0)
                offset = i + 1;

            buffer >>= 1;
        }

        return offset;
    }

    public static byte GetMostSignificantByte(this long value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out int resultWithoutRemainder) == 0 ? resultWithoutRemainder : resultWithoutRemainder + 1);

    public static byte GetNumberOfDigits(this long value)
    {
        double count = Math.Log10(value);

        return (byte) ((count % 1) == 0 ? count : count + 1);
    }

    #endregion
}