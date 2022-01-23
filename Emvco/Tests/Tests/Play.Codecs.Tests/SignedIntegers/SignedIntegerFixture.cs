using System;
using System.Collections.Generic;

using Play.Tests.Core.Random;

namespace Play.Codecs.Tests.SignedIntegers;

internal class SignedIntegerFixture
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

    public static IEnumerable<object[]> GetRandomSByte(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Integers.SByte()};
    }

    public static IEnumerable<object[]> GetRandomShort(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Integers.Short()};
    }

    public static IEnumerable<object[]> GetRandomInt(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Integers.Int()};
    }

    public static IEnumerable<object[]> GetRandomLong(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Integers.Long()};
    }

    #endregion
}