using System;
using System.Collections.Immutable;
using System.Linq;

namespace Play.Core.Tests.Data.Factories;

internal class CharArrayFactory
{
    public static char[] GetRandom(Random random, int length)
    {
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
            result[i] = GetRandomChar(random);

        return result;
    }

    private static char GetRandomChar(Random random) => (char)random.Next(char.MinValue, char.MaxValue);
}

