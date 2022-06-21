using System;

namespace Play.Core.Extensions;

public static class LongExtension
{
    #region Instance Members

    public static long GetMaskedValue(this long value, long bitsToMask) => value & ~bitsToMask;

    public static int GetMostSignificantBit(this long value)
    {
        if (value == 0)
            return 0;

        int bitLog = (int) Math.Log(value, 2);

        return bitLog + 1;
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