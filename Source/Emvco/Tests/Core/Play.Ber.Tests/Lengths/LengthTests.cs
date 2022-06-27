using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Lengths;
using Play.Testing.BaseTestClasses;

using Xunit;
using Xunit.Sdk;

namespace Play.Ber.Tests.Lengths
{
    public partial class LengthTests : TestBase
    {
        #region Instance Members

        [Fact]
        public void LongLengthByteArray0b1000000110000000_ParsingThenSerializing_ReturnsExpectedResult()
        {
            byte[] testData = new byte[] {0b10000001, 0b10000000};
            byte[] expected = testData;
            Length sut = Length.Parse(testData);
            byte[] actual = sut.Serialize();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LongLengthByteArray100000101000000100000001_ParsingThenSerializing_ReturnsExpectedResult()
        {
            byte[] testData = new byte[] {0b10000010, 0b10000001, 0b00000001};
            byte[] expected = testData;
            Length sut = Length.Parse(testData);
            byte[] actual = sut.Serialize();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LongLengthByteArray10000010100100101001001_ParsingThenSerializing_ReturnsExpectedResult()
        {
            byte[] testData = new byte[] {0b10000010, 0b1001001, 0b01001001};
            byte[] expected = testData;
            Length sut = Length.Parse(testData);
            byte[] actual = sut.Serialize();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Parsing

        [Fact]
        public void ArrayOfLengthOctets_Parsing_ReturnsExpectedResult()
        {
            byte[] testData = new byte[] {0b10000001, 0b10000000};
            uint expected = 0b10000001_10000000;
            Length actual = Length.Parse(testData);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ArrayOfShortLength0b00000001_Parsing_ReturnsExpectedResult()
        {
            byte[] testData = new byte[] {0b00000001};
            uint expected = 1;
            Length actual = Length.Parse(testData);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ArrayOfLongLength_Parsing_ReturnsExpectedResult()
        {
            byte[] testData = new byte[] {0b10000001, 0b10011111};
            uint expected = 0b10000001_10011111;
            Length actual = Length.Parse(testData);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ArrayOfShortLength0b00011111_Parsing_ReturnsExpectedResult()
        {
            byte[] testData = new byte[] {0b00011111};
            uint expected = 0b00011111;
            Length actual = Length.Parse(testData);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ArrayOfShortLengthInLongLengthFormat_Parsing_ReturnsShortLengthResult()
        {
            byte[] testData = new byte[] {0b10000001, 0b00011111};
            uint expected = 0b00011111;
            Length actual = Length.Parse(testData);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Operators

        [Fact]
        public void ArrayOfContentOctets_Equality_ReturnsExpectedResult()
        {
            byte[] testData = new byte[128];
            Array.Fill<byte>(testData, 0xFF);
            Length expected = new(128);
            Length actual = new((uint) testData.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LongIdentifier_CastingToUInt_ReturnsExpectedLength()
        {
            uint testData = 201;
            Length actual = new(testData);
            uint expected = 0b10000001_11001001;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MinimumLengthValue_CastingToUInt_ReturnsExpectedResult()
        {
            uint testValue = 0;
            Length actual = new(testValue);
            uint expected = 0;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ArrayOfContentOctets_CastingToUInt_ReturnsExpectedResult()
        {
            byte[] testData = new byte[128];
            Array.Fill<byte>(testData, 0xFF);
            uint expected = 0b10000001_10000000;
            Length actual = new((uint) testData.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LongLength_CastingToUInt_ReturnsExpectedResult()
        {
            uint testData = 128;
            uint expected = 0b10000001_10000000;
            Length actual = new(testData);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Length_CastingToUInt_ReturnsExpectedResult()
        {
            uint testData = 11;
            uint expected = 11;
            Length actual = new(testData);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Content Length

        [Fact]
        public void LongIdentifier_InvokingGetContentLength_ReturnsExpectedLength()
        {
            uint testData = 0b11001001;
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

        #region GetByteCount

        [Fact]
        public void LongLength_GetByteCount_ReturnsExpectedResult()
        {
            byte[] testData = new byte[128];
            int expected = 2;
            Length sut = new((uint) testData.Length);
            int actual = sut.GetByteCount();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShortLength_GetByteCount_ReturnsExpectedResult()
        {
            byte[] testData = new byte[127];
            int expected = 1;
            Length sut = new((uint) testData.Length);
            int actual = sut.GetByteCount();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetContentLength

        [Fact]
        public void LongLength_GetContentLength_ReturnsExpectedResult1()
        {
            byte[] testData = new byte[128];
            int expected = 128;
            Length sut = new((uint) testData.Length);
            int actual = sut.GetContentLength();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShortLength_GetContentLength_ReturnsExpectedResult1()
        {
            byte[] testData = new byte[127];
            int expected = 127;
            Length sut = new((uint) testData.Length);
            int actual = sut.GetContentLength();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MinShortLength_GetContentLength_ReturnsExpectedResult1()
        {
            uint expected = 0;
            Length sut = new(expected);
            uint actual = sut.GetContentLength();

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}