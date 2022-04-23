using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integersd
{
    public class ByteTests : TestBase
    {
        #region Instance Members

        [Fact]
        public void Byte_GetNumberOfDigits3_ReturnsExpectedResult()
        {
            byte testData = byte.MaxValue;
            int expected = Specs.Integer.UInt8.MaxDigits;
            byte actual = testData.GetNumberOfDigits();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Byte_GetNumberOfDigits2_ReturnsExpectedResult()
        {
            byte testData = 12;
            int expected = 2;
            byte actual = testData.GetNumberOfDigits();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Byte_GetNumberOfDigits0_ReturnsExpectedResult()
        {
            byte testData = 0;
            int expected = 1;
            byte actual = testData.GetNumberOfDigits();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Byte_GetMostSignificantBit0_ReturnsExpectedResult()
        {
            byte testData = 0;
            int expected = 0;
            int actual = testData.GetMostSignificantBit();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Byte_GetMostSignificantBit14_ReturnsExpectedResult()
        {
            byte testData = 12;
            int expected = 4;
            int actual = testData.GetMostSignificantBit();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Byte_GetMostSignificantBit16_ReturnsExpectedResult()
        {
            byte testData = byte.MaxValue;
            int expected = Specs.Integer.UInt8.BitCount;
            int actual = testData.GetMostSignificantBit();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Theory]
        [MemberData(nameof(IntFixture.MostSignificantBit.ForByte), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
        public void RandomByteArray_InvokesConcatArrays_CreatesValueCopyWithCorrectLength(int actual, byte testData)
        {
            int expected = testData.GetMostSignificantBit();

            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        #endregion
    }
}