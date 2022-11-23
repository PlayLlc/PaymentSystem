using System;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class UShortExtension
{
    #region Instance Members

    public static byte GetNumberOfDigits(this in ushort value)
    {
        if (value == 0)
            return 1;

        return (byte) Math.Floor(Math.Log10(value) + 1);
    }

    public static byte GetMostSignificantBit(this in ushort value)
    {
        if (value == 0)
            return 0;

        int bitLog = (int) Math.Log(value, 2);

        return (byte) (bitLog + 1);
    }

    public static byte GetMaskedValue(this in ushort value, in ushort bitsToMask) => (byte) (value & ~bitsToMask);

    /// <summary>
    ///     Determines if any of the bits provided are set in the value to be compared
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bitsToCompare"></param>
    /// <returns></returns>
    public static bool AreAnyBitsSet(this ushort value, ushort bitsToCompare) => (value & bitsToCompare) != 0;

    /// <summary>
    ///     This method gets the number of bits that are set to 1 in the integer
    /// </summary>
    public static int GetSetBitCount(this ushort value)
    {
        int result = 0;

        for (byte i = 1; i <= Specs.Integer.UInt16._BitCount; i++)
            if (value.IsBitSet((byte) i))
                result++;

        return result;
    }

    public static byte GetMostSignificantByte(this in ushort value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out byte resultWithoutRemainder) == 0 ? resultWithoutRemainder : resultWithoutRemainder + 1);

    public static bool IsBitSet(this ushort value, byte bitPosition) => (value & (1 << (bitPosition - 1))) != 0;
    public static bool IsBitSet(this ushort value, Bits bit) => (value & BitLookup.GetBit(bit)) != 0;
    public static bool IsEven(this ushort value) => (value % 2) == 0;

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ushort SetBit(this in ushort input, byte bitPosition)
    {
        if (bitPosition > Specs.Integer.UInt32._BitCount)
            throw new ArgumentOutOfRangeException(nameof(bitPosition));

        return (ushort) (input | (1 << (bitPosition - 1)));
    }

    #endregion
}