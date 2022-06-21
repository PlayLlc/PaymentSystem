using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Core.Tests.Data.Factories;

namespace Play.Core.Tests.Data.Fixtures;

internal static class CharArrayFixture
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Members

    public static IEnumerable<object[]> GetRandom(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int length = _Random.Next(0, Specifications.Specs.ByteArray.StackAllocateCeiling);
            yield return new object[] { CharArrayFactory.GetRandom(_Random, length) };
        }  
    }

    #endregion
}
