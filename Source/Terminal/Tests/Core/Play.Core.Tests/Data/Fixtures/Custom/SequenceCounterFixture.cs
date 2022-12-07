using System;
using System.Collections.Generic;

using Play.Randoms;

namespace Play.Core.Tests.Data.Fixtures.Custom;

internal static class SequenceCounterFixture
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    public static class SequenceCounterTreshold
    {
        public static IEnumerable<object[]> GetRandom(int count)
        {
            for (int i = 0; i < count; i++)
            {
                (int minValue, int maxValue, int increment) = GetSequenceTresholdParameters();

                yield return new object[] { minValue, maxValue, increment };
            }
        }

        private static (int, int, int) GetSequenceTresholdParameters()
        {
            int minValue = Randomize.Integers.Int(0, int.MaxValue);
            int maxValue = Randomize.Integers.Int(minValue, int.MaxValue);
            int increment = Randomize.Integers.Int(minValue, maxValue);

            return (minValue, maxValue, increment);
        }
    }
}

