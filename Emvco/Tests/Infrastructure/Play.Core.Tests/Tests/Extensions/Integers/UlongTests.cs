using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Extensions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers
{
    public class UlongTests : TestBase
    {
        #region Instance Members

        [Fact]
        public void Uint_SettingBitOne_ReturnsExpectedResult()
        {
            ulong testData = 0b11111110;
            ulong expected = 0b11111111;
            ulong actual = testData.SetBit(1);
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Uint_SettingBitOneWhenAlreadySet_ReturnsExpectedResult()
        {
            ulong testData = 0b11111111;
            ulong expected = 0b11111111;
            ulong actual = testData.SetBit(1);
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void Uint_SettingBitThirtyOne_ReturnsExpectedResult()
        {
            ulong testData = 0b10000010000000000000010000000010;
            ulong expected = 0b11000010000000000000010000000010;
            ulong actual = testData.SetBit(31);
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        #endregion
    }
}