using System;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class ShortExtension
{
    #region Instance Members

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

        int bitLog = (int) Math.Log(value, 2);

        return bitLog + 1;
    }

    public static byte GetMostSignificantByte(this in short value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out int resultWithoutRemainder) == 0 ? resultWithoutRemainder : resultWithoutRemainder + 1);

    public static byte GetNumberOfDigits(this in short value)
    {
        if (value == 0)
            return 1;

        return (byte) Math.Floor(Math.Log10(value) + 1);
    }

    #endregion
}