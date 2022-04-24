﻿using System;
using System.Collections.Generic;

using Play.Randoms;

namespace Play.Codecs.Tests.Tests.Alphabetic;

internal class AlphabeticFixture
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
            yield return new object[] {Randomize.Alpha.Bytes(_Random.Next(minLength, maxLength))};
    }

    public static IEnumerable<object[]> GetRandomString(int count, int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentOutOfRangeException(nameof(minLength));

        for (int i = 0; i < count; i++)
            yield return new object[] {Randomize.Alpha.String(_Random.Next(minLength, maxLength))};
    }

    #endregion
}