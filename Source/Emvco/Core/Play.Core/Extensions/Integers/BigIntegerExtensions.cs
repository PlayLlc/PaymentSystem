using System;
using System.Numerics;

using Play.Core.Specifications;

namespace Play.Core.Extensions;

public static class BigIntegerExtensions
{
    #region Instance Members

    /// <param name="buffer"></param>
    /// <returns>The amount of bytes written to the buffer</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static int AsSpan(this BigInteger value, Span<byte> buffer)
    {
        if (buffer.Length < value.GetByteCount())
            throw new ArgumentOutOfRangeException(nameof(buffer));

        value.TryWriteBytes(buffer, out int bytesWritten);

        return bytesWritten;
    }

    public static bool AreBitsSet(this BigInteger value, BigInteger valueToCheck) => (value & valueToCheck) != 0;
    public static BigInteger ClearBits(this in BigInteger input, BigInteger bitsToClear) => input & ~bitsToClear;

    /// <summary>
    ///     Returns the most significant bit in the <see cref="BigInteger" />
    /// </summary>
    /// <remarks>
    ///     Signed Integers will always return the most significant bit
    /// </remarks>
    public static int GetMostSignificantBit(this in BigInteger value)
    {
        if (value == 0)
            return 0;

        if (value < uint.MaxValue)
            return (int) Math.Log((double) value, 2) + 1;

        BigInteger buffer = value;
        int offset = 0;

        for (int i = 0; i < Specs.Integer.Int64.BitCount; i++)
        {
            if (buffer == 0)
                return offset;

            if ((buffer % 10) != 0)
                offset = i + 1;

            buffer >>= 1;
        }

        return offset;
    }

    public static byte GetMostSignificantByte(this in BigInteger value) =>
        (byte) (value.GetMostSignificantBit().TryGetRemainder(8, out int resultWithoutRemainder) == 0 ? resultWithoutRemainder : resultWithoutRemainder + 1);

    public static byte GetNumberOfDigits(this in BigInteger value)
    {
        double count = Math.Log10(Math.Pow(2, value.GetMostSignificantBit()));

        return (byte) ((count % 1) == 0 ? count : count + 1);
    }

    public static bool IsBitSet(this in BigInteger value, in byte bitPosition) => (value & ((BigInteger) 1 << (bitPosition - 1))) != 0;

    public static byte RightPaddedUnsetBitCount(this in BigInteger value)
    {
        byte mostSignificantByte = value.GetMostSignificantByte();
        byte paddedCount = 0;

        for (int i = 0; i < mostSignificantByte; i++)
        {
            if (((byte) (value >> i) % 10) != 0)
                return paddedCount;

            paddedCount++;
            i++;
        }

        return paddedCount;
    }

    public static BigInteger SetBit(this in BigInteger input, Bits bitToSet, byte bytePosition) =>
        input | ((BigInteger) (byte) bitToSet << ((bytePosition * 8) - 8));

    /// <summary>
    ///     Returns the value of the remainder, or 0 if there is no remainder. The out variable will return the result
    ///     of the integer division without a remaining value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="divisor"></param>
    /// <param name="resultWithoutRemainder"></param>
    /// <returns></returns>
    public static int TryGetRemainder(this in BigInteger value, int divisor, out BigInteger resultWithoutRemainder)
    {
        resultWithoutRemainder = (uint) (value / divisor);

        return (int) (value % divisor);
    }

    public static int GetNumberOfDigits(this BigInteger value)
    {
        checked
        {
            if (value == 0)
                return 1;

            int offset = 0;
            for (; value > 0; offset++)
                value /= 10;

            return offset;
        }
    }

    #endregion
}