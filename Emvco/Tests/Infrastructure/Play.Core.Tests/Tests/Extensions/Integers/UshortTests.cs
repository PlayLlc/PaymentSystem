using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Extensions;
using Play.Core.Specifications;
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
        public void Ushort_GetMostSignificantBit16_ReturnsExpectedResult()
        {
            ushort testData = ushort.MaxValue;
            int expected = Specs.Integer.UInt16.BitCount;
            int actual = testData.GetMostSignificantBit();
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        #endregion
    }
}