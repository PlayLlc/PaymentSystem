using System;

namespace Play.Core.Tests.TestData.Factories;

internal class ByteArrayFactory
{
    #region Instance Members

    public static byte[] GetRandom(Random random, int length)
    {
        byte[] result = new byte[length];

        for (int i = 0; i < length; i++)
            result[i] = ByteFactory.GetRandom(random, byte.MinValue, byte.MaxValue);

        return result;
    }

    #endregion
}