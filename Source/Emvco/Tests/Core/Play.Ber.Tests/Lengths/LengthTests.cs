using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Lengths;

using Xunit;
using Xunit.Sdk;

namespace Play.Ber.Tests.Lengths
{
    public partial class LengthTests
    {
        #region Instance Members

        [Fact]
        public void LongIdentifier_InvokingGetContentLength_ReturnsExpectedLength()
        {
            uint testData = 0b10000001_11001001;
            Length sut = new(testData);
            int expected = 0b11001001;
            int actual = sut.GetContentLength();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShortIdentifier_InvokingGetContentLength_ReturnsExpectedLength()
        {
            uint testData = 0b01111111;
            Length sut = new(testData);
            int expected = 0b01111111;
            int actual = sut.GetContentLength();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ZeroLength_InvokingGetContentLength_ReturnsExpectedLength()
        {
            uint testData = 0;
            Length sut = new(testData);
            int expected = 0;
            int actual = sut.GetContentLength();

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}