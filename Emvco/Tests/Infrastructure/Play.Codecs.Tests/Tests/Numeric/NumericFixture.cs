using System;
using System.Collections.Generic;

using Play.Randoms;

namespace Play.Codecs.Tests.Tests.Numeric;

internal class NumericFixture
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
            yield return new object[] {Randomize.Numeric.Bytes(_Random.Next(minLength, maxLength))};
    }

    public static IEnumerable<object[]> GetRandomString(int count, int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentOutOfRangeException(nameof(minLength));

        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Numeric.String(_Random.Next(minLength, maxLength))};
    }

    public static IEnumerable<object[]> GetRandomByte(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Numeric.Byte()};
    }

    public static IEnumerable<object[]> GetRandomUShort(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Numeric.UShort()};
    }

    public static IEnumerable<object[]> GetRandomUInt(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Numeric.UInt()};
    }

    public static IEnumerable<object[]> GetRandomULong(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Numeric.ULong()};
    }

    #endregion
}