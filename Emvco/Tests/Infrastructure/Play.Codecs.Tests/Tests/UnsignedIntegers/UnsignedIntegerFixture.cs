using System;
using System.Collections.Generic;

using Play.Randoms;

namespace Play.Codecs.Tests.UnsignedIntegers;

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
            yield return new object[] {GetRandomBytes(Randomize.Arrays.Bytes(_Random.Next(minLength, maxLength)))};
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

    #endregion
}