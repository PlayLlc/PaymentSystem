using System;
using System.Collections.Generic;

using Play.Randoms;
using Play.Testing.BaseTestClasses;

namespace Play.Codecs.Tests.Tests.Hexadecimals;

internal class HexadecimalFixture : TestBase
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
            yield return new object[] {Randomize.Hex.Bytes(_Random.Next(minLength, maxLength))};
    }

    public static IEnumerable<object[]> GetRandomString(int count, int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentOutOfRangeException(nameof(minLength));

        for (int i = 0; i < count; i++)
        {
            int hexLength = _Random.Next(minLength, maxLength);
            hexLength += hexLength % 2;

            yield return new object[] {Randomize.Hex.String(hexLength)};
        }
    }

    #endregion
}