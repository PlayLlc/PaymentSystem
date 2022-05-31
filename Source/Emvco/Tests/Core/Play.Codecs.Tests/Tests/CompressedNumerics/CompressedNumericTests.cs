using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs.Exceptions;
using Play.Codecs.Tests.Tests.AlphaNumerics;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Codecs.Tests.Tests.CompressedNumerics
{
    public class CompressedNumericTests : TestBase
    {
        #region Instance Values

        private readonly CompressedNumericCodec _SystemUnderTest;

        #endregion

        #region Constructor

        public CompressedNumericTests()
        {
            _SystemUnderTest = PlayCodec.CompressedNumericCodec;
        }

        #endregion

        #region Instance Members

        /// <summary>
        ///     RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult
        /// </summary>
        /// <param name="testValue"></param>
        /// <exception cref="CodecParsingException"></exception>
        [Theory]
        [MemberData(nameof(CompressedNumericFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(CompressedNumericFixture))]
        public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
        {
            string decoded = _SystemUnderTest.DecodeToString(testValue);
            byte[] actual = _SystemUnderTest.Encode(decoded);
            Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
        }

        /// <summary>
        ///     RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult
        /// </summary>
        /// <param name="testValue"></param>
        /// <exception cref="CodecParsingException"></exception>
        [Theory]
        [MemberData(nameof(CompressedNumericFixture.GetRandomString), 100, 1, 300, MemberType = typeof(CompressedNumericFixture))]
        public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
        {
            byte[] decoded = _SystemUnderTest.Encode(testValue);
            string actual = _SystemUnderTest.DecodeToString(decoded);
            Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
        }

        [Fact]
        public void GivenInvalidAlphabeticString_GetBytes_ThrowsEncodingException()
        {
            const string testData = "Z383937376377P";

            Assert.Throws<CodecParsingException>(() => _SystemUnderTest.Encode(testData));
        }

        #endregion
    }
}