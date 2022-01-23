//using System;
//using Play.Core.Extensions;

//namespace Play.Core.Iso8825.Tests.Ber.TagTests.SubsequentOctets.TestData
//{
//    public static class SubsequentOctetsTestValueFactory
//    {
//        public static byte[] GetValidSubsequentOctetsValue(Random random)
//        {
//            var byteLength = (byte) random.Next(1, 4);
//            var lastSubsequentOctet = (byte) random.Next(1, 0b01111111);
//            var testValue = new byte[byteLength];
//            for (int i = 0; i < byteLength - 1; i++)
//                testValue[i] = ((byte)random.Next(0, 0b11111111)).SetBit(BitCount.Eight);

//            testValue[byteLength - 1] = lastSubsequentOctet;

//            return testValue;
//        }
//    }
//}

