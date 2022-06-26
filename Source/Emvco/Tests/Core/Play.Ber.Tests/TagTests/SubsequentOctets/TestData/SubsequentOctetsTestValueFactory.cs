using System;

using Play.Core.Extensions;

namespace Play.Core.Iso8825.Tests.Ber.TagTests.SubsequentOctets.TestData
{
    public static class SubsequentOctetsTestValueFactory
    {
        #region Instance Members

        public static byte[] GetValidSubsequentOctetsValue(Random random)
        {
            byte byteLength = (byte) random.Next(1, 4);
            byte lastSubsequentOctet = (byte) random.Next(1, 0b01111111);
            byte[]? testValue = new byte[byteLength];
            for (int i = 0; i < (byteLength - 1); i++)
                testValue[i] = ((byte) random.Next(0, 0b11111111)).SetBit(Bits.Eight);

            testValue[byteLength - 1] = lastSubsequentOctet;

            return testValue;
        }

        #endregion
    }
}