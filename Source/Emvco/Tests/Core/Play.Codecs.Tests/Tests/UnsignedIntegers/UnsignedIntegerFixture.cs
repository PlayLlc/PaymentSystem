using System;
using System.Collections.Generic;

using Play.Randoms;

namespace Play.Codecs.Tests.Tests.UnsignedIntegers;

internal class UnsignedIntegerFixture
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Members

    public static IEnumerable<object[]> GetRandomBytes(int count, int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentOutOfRangeException(nameof(minLength));

        for (int i = 0; i < count; i++)
            yield return new object[] {GetRandomBytesWithoutLeadingZero(minLength, maxLength)};
    }

    private static byte[] GetRandomBytesWithoutLeadingZero(int minLength, int maxLength)
    {
        byte[] result = GetRandomBytes(Randomize.Arrays.Bytes(_Random.Next(minLength, maxLength)));

        if (result[0] <= 0xF)
            return GetRandomBytesWithoutLeadingZero(minLength, maxLength);

        return result;
    }

    public static IEnumerable<object[]> GetRandomString(int count, int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentOutOfRangeException(nameof(minLength));

        for (int i = 0; i < count; i++)
            yield return new object[] {GetRandomStringWithoutLeadingZero(minLength, maxLength)};
    }

    private static string GetRandomStringWithoutLeadingZero(int minLength, int maxLength)
    {
        string result = Randomize.Numeric.String(_Random.Next(minLength, maxLength));

        if (result.StartsWith("0"))
            return GetRandomStringWithoutLeadingZero(minLength, maxLength);

        return result;
    }

    private static byte[] GetRandomBytes(byte[] value)
    {
        // Encoding to unsigned integers will truncate leading 0 values. We only want to 
        if (value[^1] == 0)
            value[^1] = 1;

        return value;
    }

    public static IEnumerable<object[]> GetRandomByte(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Integers.Byte()};
    }

    public static IEnumerable<object[]> GetRandomUShort(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Integers.UShort()};
    }

    public static IEnumerable<object[]> GetRandomUInt(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Integers.UInt()};
    }

    public static IEnumerable<object[]> GetRandomULong(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Integers.ULong()};
    }

    public static IEnumerable<object[]> GetRandomBigInteger(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Integers.BigInteger()};
    }

    #endregion
}