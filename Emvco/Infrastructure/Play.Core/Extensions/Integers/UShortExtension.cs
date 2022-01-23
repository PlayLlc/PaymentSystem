using System;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class UShortExtension
{
    #region Instance Members

    public static ushort ClearBits(this in ushort input, ushort bitsToClear) => (ushort) (input & ~bitsToClear);
    public static string GetBinaryString(this ushort value) => Convert.ToString(value, 2);
    public static byte GetMaskedValue(this in ushort value, in ushort bitsToMask) => (byte) (value & ~bitsToMask);

    public static byte GetMostSignificantBit(this in ushort value)
    {
        if (value == 0)
            return 1;

        byte bitLog = (byte) System.Math.Log2(value);

        return (byte) (bitLog + 1);
    }

    public static byte GetMostSignificantByte(this in ushort value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out byte resultWithoutRemainder) == 0
            ? resultWithoutRemainder
            : resultWithoutRemainder + 1);

    public static byte GetNumberOfDigits(this in ushort value)
    {
        double count = System.Math.Log10(System.Math.Pow(2, value.GetMostSignificantBit()));

        return (byte) ((count % 1) == 0 ? count : count + 1);
    }

    public static bool HasValue(this ushort value, ushort valueToCheck) => (value & valueToCheck) != 0;
    public static bool IsBitSet(this in ushort value, byte bitPosition) => (value & (1 << bitPosition)) != 0;
    public static bool IsEven(this ushort value) => (value % 2) == 0;

    public static byte LeftPaddedUnsetBitCount(this in ushort value)
    {
        const byte maxUshortBit = 16;

        return (byte) (maxUshortBit - value.GetMostSignificantBit());
    }

    /// <summary>
    ///     This will return the number of unset bits there are before the first set bit, starting with the least significant
    ///     bit.
    ///     For example, the following value:
    ///     01011000
    ///     would return 3.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte RightPaddedUnsetBitCount(this ushort value)
    {
        const byte numberOfBitsInUlong = 16;

        for (byte i = 0; i < numberOfBitsInUlong; i++)
        {
            if ((value & (ulong) (1 << i)) != 0)
                return i;
        }

        return 0;
    }

    /// <summary>
    ///     Returns a value that has each bit after the most significant bit in <see cref="value" /> filled
    /// </summary>
    /// <param name="value"></param>
    public static ushort GetMaskAfterMostSignificantBit(this ushort value) => (ushort) ~value.GetMaskToMostSignificantBit();

    /// <summary>
    ///     Returns a value that has each bit filled to the most significant bit in <see cref="value" />
    /// </summary>
    /// <param name="value"></param>
    public static ushort GetMaskToMostSignificantBit(this ushort value)
    {
        byte mostSignificantBit = value.GetMostSignificantBit();

        byte offset = mostSignificantBit;
        ushort result = 0;

        for (; offset < Specs.Integer.UInt8.BitCount; offset -= Specs.Integer.UInt8.BitCount)
        {
            result <<= 8;
            result |= byte.MaxValue;
        }

        for (; offset > 0; offset--)
        {
            result <<= 1;
            result |= 0x1;
        }

        return result;
    }

    #endregion
}