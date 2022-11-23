using System;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class IntExtension
{
    #region Instance Members

    /// <summary>
    ///     Returns the most significant bit in the <see cref="int" />
    /// </summary>
    /// <remarks>
    ///     Signed Integers will always return the most significant bit
    /// </remarks>
    public static int GetMostSignificantBit(this int value)
    {
        if (value == 0)
            return 0;

        if (value < 0)
            return Specs.Integer.Int32._BitCount;

        int bitLog = (int) Math.Log(value, 2);

        return bitLog + 1;
    }

    public static byte GetMostSignificantByte(this int value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out int resultWithoutRemainder) == 0 ? resultWithoutRemainder : resultWithoutRemainder + 1);

    public static byte GetNumberOfDigits(this int value)
    {
        double count = Math.Log10(value);

        return (byte) ((count % 1) == 0 ? count : count + 1);
    }

    /// <summary>
    ///     Returns the value of the remainder, or 0 if there is no remainder. The out variable will return the result
    ///     of the integer division without a remaining value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="divisor"></param>
    /// <param name="resultWithoutRemainder"></param>
    /// <returns></returns>
    public static int TryGetRemainder(this int value, int divisor, out int resultWithoutRemainder)
    {
        resultWithoutRemainder = value / divisor;

        return value % divisor;
    }

    #endregion
}