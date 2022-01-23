using System;

namespace Play.Core.Tests.TestData.Factories
{
    internal static class ByteFactory
    {
        #region Instance Members

        public static byte GetRandom(Random random)
        {
            return (byte) random.Next(byte.MinValue, byte.MaxValue);
        }

        public static byte GetRandom(Random random, byte min, byte max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min));

            return (byte) random.Next(min, max);
        }

        #endregion
    }
}