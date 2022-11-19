using System;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class UIntExtension
{
    #region Instance Members

    public static bool AreAnyBitsSet(this uint value, uint bitsToCompare) => (value & bitsToCompare) != 0;
    public static uint ClearBits(this in uint input, uint bitsToClear) => input & ~bitsToClear;
    public static string GetBinaryString(this uint value) => Convert.ToString(value, 2);
    public static uint GetMaskedValue(this uint value, in uint mask) => value & ~mask;

    public static int GetMostSignificantBit(this uint value)
    {
        if (value == 0)
            return 0;

        int bitLog = (int) Math.Log(value, 2);

        return bitLog + 1;
    }

    public static uint ClearBit(this in uint input, Bits bitToClear, byte bytePosition)
    {
        if (bytePosition > Specs.Integer.UInt32.ByteCount)
            throw new ArgumentOutOfRangeException(nameof(bytePosition));

        return input & ~((uint) bitToClear << ((bytePosition * 8) - 8));
    }

    public static byte GetMostSignificantByte(this uint value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out int resultWithoutRemainder) == 0 ? resultWithoutRemainder : resultWithoutRemainder + 1);

    public static byte GetNumberOfDigits(this uint value)
    {
        if (value == 0)
            return 1;

        return (byte) Math.Floor(Math.Log10(value) + 1);
    }

    public static bool HasValue(this uint value, uint valueToCheck) => (value & valueToCheck) != 0;
    public static bool IsBitSet(this uint value, Bits bit) => (value & BitLookup.GetBit(bit)) != 0;
    public static bool IsBitSet(this uint value, byte bitPosition) => (value & ((uint) 1 << (bitPosition - 1))) != 0;
    public static bool IsEven(this uint value) => (value % 2) == 0;

    /// <summary>
    ///     This method gets the number of bits that are set to 1 in the integer
    /// </summary>
    public static int GetSetBitCount(this uint value)
    {
        int result = 0;

        for (byte i = 1; i <= Specs.Integer.UInt32.BitCount; i++)
        {
            if (value.IsBitSet((byte) i))
                result++;
        }

        return result;
    }

    public static byte LeftPaddedUnsetBitCount(this in uint value)
    {
        const byte maxBit = 32;

        return (byte) (maxBit - value.GetMostSignificantBit());
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
    public static byte RightPaddedUnsetBitCount(this uint value)
    {
        const byte numberOfBitsInUlong = 32;

        for (byte i = 0; i < numberOfBitsInUlong; i++)
        {
            if ((value & (ulong) (1 << i)) != 0)
                return i;
        }

        return 0;
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static uint SetBit(this in uint input, byte bitPosition)
    {
        if (bitPosition > Specs.Integer.UInt32.BitCount)
            throw new ArgumentOutOfRangeException(nameof(bitPosition));

        return input | (uint)(1 << (bitPosition - 1));
    }

    public static uint SetBits(this in uint input, uint bitsToSet) => input | bitsToSet;

    /// <summary>
    ///     Returns the value of the remainder, or 0 if there is no remainder. The out variable will return the result
    ///     of the integer division without a remaining value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="divisor"></param>
    /// <param name="resultWithoutRemainder"></param>
    /// <returns></returns>
    public static uint TryGetRemainder(this uint value, int divisor, out uint resultWithoutRemainder)
    {
        resultWithoutRemainder = (uint) (value / divisor);

        return (uint) (value % divisor);
    }

    /// <summary>
    ///     Returns a value that has each bit after the most significant bit in <see cref="value" /> filled
    /// </summary>
    /// <param name="value"></param>
    public static uint GetMaskAfterMostSignificantBit(this uint value) => ~value.GetMaskToMostSignificantBit();

    /// <summary>
    ///     Returns a value that has each bit filled to the most significant bit in <see cref="value" />
    /// </summary>
    /// <param name="value"></param>
    public static uint GetMaskToMostSignificantBit(this uint value)
    {
        int mostSignificantBit = value.GetMostSignificantBit();

        int offset = mostSignificantBit;
        uint result = 0;

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