using System;
using System.Collections.Generic;

using Play.Core.Extensions;
using Play.Core.Tests.Data.Factories;

namespace Play.Core.Tests.Data.Fixtures;

internal class ByteArrayFixture
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Members

    public static IEnumerable<object[]> GetRandom(int count, int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentOutOfRangeException(nameof(minLength));

        for (int i = 0; i < count; i++)
            yield return new object[] {ByteArrayFactory.GetRandom(_Random, _Random.Next(minLength, maxLength))};
    }

    public static IEnumerable<object[]> GetRandomNibble(int count, int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentOutOfRangeException(nameof(minLength));

        for (int i = 0; i < count; i++)
            yield return new object[] { ByteArrayFactory.GetRandom(_Random, _Random.Next(minLength, maxLength)).AsNibbleArray() };
    }

    #endregion
}