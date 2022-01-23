using System;
using System.Collections.Generic;

using Play.Tests.Core.Random;

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
            yield return new object[] {Randomize.Array.Bytes(_Random.Next(minLength, maxLength))};
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