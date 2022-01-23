﻿using Play.Codecs.Tests.Numeric;
using Play.Emv.TestData.Encoding;

using Xunit;

namespace Play.Codecs.Tests.StrictAscii
{
    public class StrictAsciiTests
    {
        #region Instance Values

        private readonly Strings.StrictAscii _SystemUnderTest;

        #endregion

        #region Constructor

        public StrictAsciiTests()
        {
            _SystemUnderTest = PlayEncoding.StrictAscii;
        }

        #endregion

        #region Instance Members

        [Theory]
        [MemberData(nameof(AsciiFixture.GetRandomBytes), new object[] {100, 1, 300}, MemberType = typeof(NumericFixture))]
        public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
        {
            string decoded = _SystemUnderTest.GetString(testValue);

            byte[] encoded = _SystemUnderTest.GetBytes(decoded);

            Assert.Equal(testValue, encoded);
        }

        [Theory]
        [MemberData(nameof(AsciiFixture.GetRandomString), new object[] {100, 1, 300}, MemberType = typeof(NumericFixture))]
        public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
        {
            byte[] decoded = _SystemUnderTest.GetBytes(testValue);
            string encoded = _SystemUnderTest.GetString(decoded);

            Assert.Equal(testValue, encoded);
        }

        [Fact]
        public void ApplicationLabelByteArray_ConvertingToAsciiString_ReturnsExpectedResult()
        {
            byte[] testData = EncodingTestDataFactory.StrictAscii.ApplicationLabelBytes;

            string result = _SystemUnderTest.GetString(testData);

            Assert.Equal(result, EncodingTestDataFactory.StrictAscii.ApplicationLabelAscii);
        }

        [Fact]
        public void ApplicationLabelAsciiString_ConvertingToByteArray_ReturnsExpectedResult()
        {
            string testData = EncodingTestDataFactory.StrictAscii.ApplicationLabelAscii;

            byte[] result = _SystemUnderTest.GetBytes(testData);

            Assert.Equal(result, EncodingTestDataFactory.StrictAscii.ApplicationLabelBytes);
        }

        [Fact]
        public void ApplicationLabelAsciiString_GettingByteCount_ReturnsExpectedResult()
        {
            string testData = EncodingTestDataFactory.StrictAscii.ApplicationLabelAscii;
            int result = _SystemUnderTest.GetByteCount(testData);

            Assert.Equal(result, EncodingTestDataFactory.StrictAscii.ApplicationLabelBytes.Length);
        }

        #endregion
    }
}