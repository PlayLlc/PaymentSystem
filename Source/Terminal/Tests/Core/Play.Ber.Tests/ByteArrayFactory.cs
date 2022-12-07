using System;
using System.Collections.Generic;

namespace Play.Ber.Tests;
public static class ByteArrayFactory
{
    public static IEnumerable<byte> GetRandom(Random random, int count, int minValue, int maxValue)
    {
        if (minValue > maxValue)
            throw new ArgumentOutOfRangeException(nameof(minValue));

        for (int i = 0; i < count; i++)
            yield return (byte)random.Next(minValue, maxValue);
    }
}
