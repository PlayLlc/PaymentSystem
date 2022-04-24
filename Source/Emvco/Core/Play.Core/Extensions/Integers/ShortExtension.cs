using System;

namespace Play.Core.Extensions;

public static class ShortExtension
{
    #region Instance Members

    public static string GetBinaryString(this short value) => Convert.ToString(value, 2);
    public static short GetMaskedShort(this in short value, short bitsToMask) => (short) (value & ~bitsToMask);

    public static short GetMaskedShort(this in short value, Bits[] bitsToMaskForLeftByte, Bits[] bitsToMaskForRightByte)
    {
        byte temp = 0;
        short result = value;

        foreach (Bits bit in bitsToMaskForLeftByte)
            temp = (byte) (temp | (byte) bit);

        short left = (short) (temp << 8);
        temp = 0;

        foreach (Bits bit in bitsToMaskForRightByte)
            temp = (byte) (temp | (byte) bit);

        short right = temp;
        result = (short) (result & (short) ~left);
        result = (short) (result & (short) ~right);

        return result;
    }

    /// <summary>
    ///     Returns the most significant bit in the <see cref="short" />
    /// </summary>
    /// <remarks>
    ///     Signed Integers will always return the most significant bit
    /// </remarks>
    public static int GetMostSignificantBit(this in short value)
    {
        if (value == 0)
            return 0;

        double count = Math.Log2(value);

        return (int) ((count % 1) == 0 ? count : count + 1);
    }

    public static byte GetMostSignificantByte(this in short value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out int resultWithoutRemainder) == 0
            ? resultWithoutRemainder
            : resultWithoutRemainder + 1);

    public static byte GetNumberOfDigits(this in short value)
    {
        double count = Math.Log10(Math.Pow(2, value.GetMostSignificantBit()));

        return (byte) ((count % 1) == 0 ? count : count + 1);
    }

    public static bool HasValue(this short value, short valueToCheck) => (value & valueToCheck) != 0;

    /// <summary>
    ///     This method will modify the value passed in as an argument. It will not return a copy
    /// </summary>
    /// <param name="input"></param>
    /// <param name="leftBit"></param>
    /// <param name="rightBit"></param>
    /// <returns></returns>
    public static short SetBit(this short input, Bits leftBit, Bits rightBit)
    {
        input = (short) (input & ((short) leftBit << 8));
        input = (short) (input & (byte) rightBit);

        return input;
    }

    /// <summary>
    ///     This method will return a value copy with the specified bits set
    /// </summary>
    /// <param name="input"></param>
    /// <param name="leftBit"></param>
    /// <param name="rightBit"></param>
    /// <returns></returns>
    public static short SetBit(this short input, in byte leftBit, in byte rightBit)
    {
        input = (short) (input & (short) (leftBit << 8));
        input = (short) (input & rightBit);

        return input;
    }

    #endregion
}