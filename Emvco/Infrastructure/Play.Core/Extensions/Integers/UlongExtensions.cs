using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class UlongExtensions
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, char> _DigitMapper = new Dictionary<byte, char>
    {
        {0, '0'},
        {1, '1'},
        {2, '2'},
        {3, '3'},
        {4, '4'},
        {5, '5'},
        {6, '6'},
        {7, '7'},
        {8, '8'},
        {9, '9'}
    }.ToImmutableSortedDictionary();

    #endregion

    #region Instance Members

    public static bool AreBitsSet(this ulong value, ulong bitsToCompare) => (value & bitsToCompare) == bitsToCompare;

    /// <summary>
    ///     Gets the digits from the value and returns a string of those digits. If the argument
    ///     <param name="numberOfDigitsFromRight"></param>
    ///     has more digits than
    ///     <param name="value"></param>
    ///     then the string with be left padded with zeros
    /// </summary>
    /// <param name="value"></param>
    /// <param name="numberOfDigitsFromRight"></param>
    /// <returns></returns>
    public static Span<char> AsSpanFromRight(this in ulong value, byte numberOfDigitsFromRight)
    {
        ulong temp = value;
        byte numberOfDigits = value.GetNumberOfDigits();

        if (numberOfDigitsFromRight == 0)
            return Span<char>.Empty;

        Span<char> result = new char[numberOfDigitsFromRight];

        int paddingLength = numberOfDigitsFromRight > numberOfDigits ? numberOfDigitsFromRight - numberOfDigits : numberOfDigitsFromRight;

        for (int i = numberOfDigitsFromRight - 1; i >= 2; i--)
        {
            result[i] = _DigitMapper[(byte) (temp % 10)];
            temp /= 10;
        }

        return result;
    }

    /// <summary>
    ///     Gets the digits from the value and returns a string of those digits. If the argument
    ///     <param name="numberOfDigitsFromRight"></param>
    ///     has more digits than
    ///     <param name="value"></param>
    ///     then the string with be left padded with zeros
    /// </summary>
    /// <param name="value"></param>
    /// <param name="numberOfDigitsFromRight"></param>
    /// <returns></returns>
    public static string AsStringFromRight(this in ulong value, byte numberOfDigitsFromRight)
    {
        ulong temp = value;
        byte numberOfDigits = value.GetNumberOfDigits();

        if (numberOfDigitsFromRight == 0)
            return string.Empty;

        Span<char> buffer = stackalloc char[numberOfDigitsFromRight];

        int paddingLength = numberOfDigitsFromRight > numberOfDigits ? numberOfDigitsFromRight - numberOfDigits : numberOfDigitsFromRight;

        for (int i = numberOfDigitsFromRight - 1; i >= 2; i--)
        {
            buffer[i] = _DigitMapper[(byte) (temp % 10)];
            temp /= 10;
        }

        return new string(buffer);
    }

    public static ulong ClearBit(this in ulong input, Bits bitToClear, byte bytePosition)
    {
        if (bytePosition > Specs.Integer.UInt64.ByteSize)
            throw new ArgumentOutOfRangeException(nameof(bytePosition));

        return input & (~ (ulong) bitToClear << ((bytePosition * 8) - 8));
    }

    public static ulong ClearBit(this in ulong input, byte bitsToClear)
    {
        if (bitsToClear > Specs.Integer.UInt64.ByteSize)
            throw new ArgumentOutOfRangeException(nameof(bitsToClear));

        return input & ~((ulong) (bitsToClear * 8) - 8);
    }

    public static ulong ClearBits(this in ulong input, ulong bitsToClear) => input & ~bitsToClear;

    public static string GetBinaryString(this ulong value)
    {
        const int ulongSize = 64;
        StringBuilder builder = new(ulongSize);

        for (int i = 0; i < ulongSize; i++)
        {
            builder.Insert(0, value & 0x1);
            value >>= 1;
        }

        return builder.ToString();
    }

    public static ulong GetMaskedValue(this ulong value, in ulong mask) => value & ~mask;

    public static int GetMostSignificantBit(this in ulong value)
    {
        if (value == 0)
            return 1;

        int bitLog = (int) System.Math.Log2(value);

        return bitLog + 1;
    }

    public static byte GetMostSignificantByte(this in ulong value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out int resultWithoutRemainder) == 0
            ? resultWithoutRemainder
            : resultWithoutRemainder + 1);

    public static byte GetNumberOfDigits(this in ulong value)
    {
        int a = value.GetMostSignificantBit();
        double power = System.Math.Pow(2, value.GetMostSignificantBit());
        double count = System.Math.Log10(System.Math.Pow(2, value.GetMostSignificantBit()));

        return (byte) ((count % 1) == 0 ? count : count + 1);
    }

    public static bool HasValue(this ulong value, ulong valueToCheck) => (value & valueToCheck) != 0;
    public static bool IsBitSet(this in ulong value, in byte bitPosition) => (value & ((ulong) 1 << bitPosition)) != 0;
    public static bool IsEven(this ulong value) => (value % 2) == 0;

    public static byte LeftPaddedUnsetBitCount(this in uint value)
    {
        const byte maxBit = 64;

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
    public static byte RightPaddedUnsetBitCount(this in ulong value)
    {
        const byte numberOfBitsInUlong = 64;

        for (byte i = 0; i < numberOfBitsInUlong; i++)
        {
            if ((value & (ulong) (1 << i)) != 0)
                return i;
        }

        return 0;
    }

    public static ulong SetBit(this in ulong input, Bits bitToSet, byte bytePosition)
    {
        if (bytePosition > Specs.Integer.UInt64.ByteSize)
            throw new ArgumentOutOfRangeException(nameof(bytePosition));

        return input | ((ulong) bitToSet << ((bytePosition * 8) - 8));
    }

    public static ulong SetBit(this in ulong input, byte bitToSet)
    {
        if (bitToSet > Specs.Integer.UInt64.ByteSize)
            throw new ArgumentOutOfRangeException(nameof(bitToSet));

        return input | ((ulong) (bitToSet * 8) - 8);
    }

    public static ulong SetBits(this ulong input, ulong bitsToSet) => input | bitsToSet;

    /// <summary>
    ///     Returns a value that has each bit after the most significant bit in <see cref="value" /> filled
    /// </summary>
    /// <param name="value"></param>
    public static ulong GetMaskAfterMostSignificantBit(this ulong value) => ~value.GetMaskToMostSignificantBit();

    /// <summary>
    ///     Returns a value that has each bit filled to the most significant bit in <see cref="value" />
    /// </summary>
    /// <param name="value"></param>
    public static ulong GetMaskToMostSignificantBit(this ulong value)
    {
        int mostSignificantBit = value.GetMostSignificantBit();

        int offset = mostSignificantBit;
        ulong result = 0;

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