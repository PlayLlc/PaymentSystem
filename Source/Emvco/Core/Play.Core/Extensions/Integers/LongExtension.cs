using System;

namespace Play.Core.Extensions;

public static class LongExtension
{
    #region Instance Members

    public static string GetBinaryString(this long value) => Convert.ToString(value, 2);
    public static long GetMaskedValue(this long value, long bitsToMask) => value & ~bitsToMask;

    public static int GetMostSignificantBit(this long value)
    {
        double count = Math.Log2(value);

        return (int) ((count % 1) == 0 ? count : count + 1);
    }

    public static byte GetMostSignificantByte(this long value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out int resultWithoutRemainder) == 0 ? resultWithoutRemainder : resultWithoutRemainder + 1);

    public static byte GetNumberOfDigits(this long value)
    {
        double count = Math.Log10(Math.Pow(2, value.GetMostSignificantBit()));

        return (byte) ((count % 1) == 0 ? count : count + 1);
    }

    public static bool HasValue(this long value, long valueToCheck) => (value & valueToCheck) != 0;

    #endregion
}