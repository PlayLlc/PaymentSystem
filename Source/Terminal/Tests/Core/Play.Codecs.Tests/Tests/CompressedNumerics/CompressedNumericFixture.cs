using System;
using System.Collections.Generic;

using Play.Randoms;

namespace Play.Codecs.Tests.Tests.CompressedNumerics;

internal class CompressedNumericFixture
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Members

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<object[]> GetRandomBytes(int count, int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentOutOfRangeException(nameof(minLength));

        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.CompressedNumeric.Bytes(_Random.Next(minLength, maxLength))};
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<object[]> GetRandomString(int count, int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentOutOfRangeException(nameof(minLength));

        for (int i = 0; i < count; i++)
        {
            int a = _Random.Next(minLength, maxLength);

            yield return new object[] {Randomize.CompressedNumeric.String(a - (a % 2))};
        }
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<object[]> GetRandomUShort(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.CompressedNumeric.UShort()};
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<object[]> GetRandomUInt(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.CompressedNumeric.UInt()};
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<object[]> GetRandomULong(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.CompressedNumeric.ULong()};
    }

    #endregion
}