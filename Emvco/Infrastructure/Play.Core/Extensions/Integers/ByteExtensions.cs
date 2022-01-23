﻿using System;

using Play.Core.Exceptions;

namespace Play.Core.Extensions;

public static class ByteExtensions
{
    #region Instance Members

    public static bool AreBitsSet(this byte value, byte bitsToCompare) => (value & bitsToCompare) == bitsToCompare;

    public static bool AreBitsSet(this byte value, params Bits[] bitsToCompare)
    {
        for (int index = 0; index < bitsToCompare.Length; index++)
        {
            if (value.IsBitSet(bitsToCompare[index]))
                return false;
        }

        return true;
    }

    public static ReadOnlySpan<byte> AsReadOnlySpan(this byte input)
    {
        ReadOnlySpan<byte> result = new[] {input};

        return result;
    }

    public static Span<byte> AsSpan(this byte input)
    {
        Span<byte> result = new[] {input};

        return result;
    }

    public static byte ClearBits(this byte input, byte bitsToClear) => (byte) (input & ~bitsToClear);
    public static string GetBinaryString(this byte value) => Convert.ToString(value, 2);

    public static byte GetByteWithBitSet(this byte input, byte pos)
    {
        byte b1 = (byte) (0x01 << (pos - 1));

        return (byte) (b1 | input);
    }

    public static byte GetByteWithBitSet(this byte input, Bits pos) => (byte) ((byte) pos | input);
    public static byte GetLeftNibble(this byte value) => value.GetMaskedValue(0b00001111);
    public static byte GetMaskedValue(this byte value, byte bitsToMask) => (byte) (value & ~bitsToMask);

    public static byte GetMaskedValue(this byte value, params Bits[] bitsToMask)
    {
        byte result = value;

        for (int index = 0; index < bitsToMask.Length; index++)
        {
            Bits bit = bitsToMask[index];
            result = (byte) (result & (byte) ~bit);
        }

        return result;
    }

    public static byte GetMaskedValue(this byte value, Bits bitToMask)
    {
        byte result = value;

        result = (byte) (result & (byte) ~bitToMask);

        return result;
    }

    public static int GetMostSignificantBit(this byte value)
    {
        if (value == 0)
            return 1;

        int bitLog = (int) System.Math.Log2(value);

        return bitLog + 1;
    }

    public static byte GetNumberOfDigits(this byte value)
    {
        double count = System.Math.Log10(System.Math.Pow(2, value.GetMostSignificantBit()));

        return (byte) ((count % 1) == 0 ? count : count + 1);
    }

    public static byte GetRightNibble(this byte value) => value.GetMaskedValue(0b11110000);
    public static bool HasValue(this byte value, byte valueToCheck) => (value & valueToCheck) != 0;

    /// <summary>
    ///     IsBitSet
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bit"></param>
    /// <returns></returns>
    /// <exception cref="PlayInternalException">Ignore.</exception>
    public static bool IsBitSet(this byte value, Bits bit) => (value & BitLookup.GetBit(bit)) != 0;

    public static bool IsEven(this byte value) => (value % 2) == 0;
    public static bool IsInRange(this byte input, MaxBit maxBit) => (input & (byte) maxBit) == 0;

    public static byte LeftPaddedUnsetBitCount(this byte value)
    {
        const byte maxBit = 8;

        return (byte) (maxBit - value.GetMostSignificantBit());
    }

    public static byte ReverseBits(this byte value) => (byte) (value ^ byte.MaxValue);

    /// <summary>
    ///     This will return the number of unset bits there are before the first set bit, starting with the least significant
    ///     bit.
    ///     For example, the following value:
    ///     01011000
    ///     would return 3.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte RightPaddedUnsetBitCount(this byte value)
    {
        const byte numberOfBitsInUlong = 8;

        for (byte i = 0; i < numberOfBitsInUlong; i++)
        {
            if ((value & (ulong) (1 << i)) != 0)
                return i;
        }

        return 0;
    }

    /// <summary>
    ///     This method will modify the byte passed in as an argument. It will not return a copy
    ///     Sets the specified bit for the byte passed in to the method.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="bitsToSet"></param>
    /// <returns></returns>
    public static byte SetBits(this byte input, params Bits[] bitsToSet)
    {
        byte result = input;
        foreach (Bits bitToSet in bitsToSet)
            result |= (byte) bitToSet;

        return result;
    }

    /// <summary>
    ///     This method will modify the byte passed in as an argument. It will not return a copy
    ///     Sets the specified bit for the byte passed in to the method.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="bitsToSet"></param>
    /// <returns></returns>
    public static byte SetBits(this byte input, byte bitsToSet) => (byte) (input | bitsToSet);

    public static byte ShiftNibbleLeft(this byte value, byte rightNibble) => (byte) (value.GetRightNibble() | rightNibble);
    public static byte ShiftNibbleRight(this byte value, byte leftNibble) => (byte) (leftNibble | value.GetLeftNibble());

    /// <summary>
    ///     Returns the value of the remainder, or 0 if there is no remainder. The out variable will return the result
    ///     of the integer division without a remaining value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="divisor"></param>
    /// <param name="resultWithoutRemainder"></param>
    /// <returns></returns>
    public static byte TryGetRemainder(this byte value, byte divisor, out byte resultWithoutRemainder)
    {
        resultWithoutRemainder = (byte) (value / divisor);

        return (byte) (value % divisor);
    }

    /// <summary>
    ///     Returns a value that has each bit after the most significant bit in <see cref="value" /> filled
    /// </summary>
    /// <param name="value"></param>
    public static byte GetMaskAfterMostSignificantBit(this byte value) => (byte) ~value.GetMaskToMostSignificantBit();

    /// <summary>
    ///     Returns a value that has each bit filled to the most significant bit in <see cref="value" />
    /// </summary>
    /// <param name="value"></param>
    public static byte GetMaskToMostSignificantBit(this byte value)
    {
        int mostSignificantBit = value.GetMostSignificantBit();

        int offset = mostSignificantBit;
        byte result = 0;

        for (; offset > 0; offset--)
        {
            result <<= 1;
            result |= 0x1;
        }

        return result;
    }

    #endregion

    public enum MaxBit
    {
        One = 0xFE,
        Two = 0xFC,
        Three = 0xF8,
        Four = 0xF0,
        Five = 0xE0,
        Six = 0xC0,
        Seven = 0x80,
        Eight = 0x00
    }
}