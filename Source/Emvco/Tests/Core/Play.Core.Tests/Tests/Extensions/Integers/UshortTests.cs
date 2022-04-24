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

namespace Play.Core.Tests.Tests.Extensions.Integers
{
    public class UshortTests : TestBase
    {
        #region Instance Members

        [Fact]
        public void Ushort_GetNumberOfDigits5_ReturnsExpectedResult()
        {
            ushort testData = 12345;
            int expected = Specs.Integer.UInt16.MaxDigits;
            byte actual = testData.GetNumberOfDigits();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Ushort_GetNumberOfDigits3_ReturnsExpectedResult()
        {
            ushort testData = 123;
            int expected = 3;
            byte actual = testData.GetNumberOfDigits();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Ushort_GetNumberOfDigits0_ReturnsExpectedResult()
        {
            ushort testData = 0;
            int expected = 1;
            byte actual = testData.GetNumberOfDigits();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Ushort_GetMostSignificantBit0_ReturnsExpectedResult()
        {
            ushort testData = 0;
            int expected = 0;
            int actual = testData.GetMostSignificantBit();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Ushort_GetMostSignificantBit14_ReturnsExpectedResult()
        {
            ushort testData = 12345;
            int expected = 14;
            int actual = testData.GetMostSignificantBit();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Ushort_GetMostSignificantBit11_3_ReturnsExpectedResult()
        {
            ushort testData = 0b0000010110110110;
            int expected = 11;
            int actual = testData.GetMostSignificantBit();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Ushort_GetMostSignificantBit16_ReturnsExpectedResult()
        {
            ushort testData = ushort.MaxValue;
            int expected = Specs.Integer.UInt16.BitCount;
            int actual = testData.GetMostSignificantBit();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Theory]
        [MemberData(nameof(IntFixture.MostSignificantBit.ForUShort), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
        public void RandomByteArray_InvokesConcatArrays_CreatesValueCopyWithCorrectLength(int actual, ushort testData)
        {
            int expected = testData.GetMostSignificantBit();

            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        #endregion
    }
}