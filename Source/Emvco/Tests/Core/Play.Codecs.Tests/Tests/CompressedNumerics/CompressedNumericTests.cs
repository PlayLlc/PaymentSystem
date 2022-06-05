using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs.Exceptions;
using Play.Codecs.Tests.Tests.AlphaNumerics;
using Play.Randoms;
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

        [Theory]
        [MemberData(nameof(CompressedNumericFixture.GetRandomUShort), 100, MemberType = typeof(CompressedNumericFixture))]
        public void RandomUShort_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
        {
            byte[] decoded = _SystemUnderTest.Encode(testValue);
            ushort actual = _SystemUnderTest.DecodeToUInt16(decoded);
            Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
        }

        [Fact]
        public void Ushort_Encoding_ReturnsExpectedResult()
        {
            ushort testValue = 12345;
            byte[] expected = new byte[] {0x12, 0x34, 0x5F};
            var actual = PlayCodec.CompressedNumericCodec.Encode(testValue);
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Theory]
        [MemberData(nameof(CompressedNumericFixture.GetRandomUInt), 100, MemberType = typeof(CompressedNumericFixture))]
        public void RandomUInt_EncodingThenDecoding_ReturnsExpectedResult(uint testValue)
        {
            byte[] decoded = _SystemUnderTest.Encode(testValue);
            uint actual = _SystemUnderTest.DecodeToUInt32(decoded);
            Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
        }

        [Theory]
        [MemberData(nameof(CompressedNumericFixture.GetRandomUInt), 100, MemberType = typeof(CompressedNumericFixture))]
        public void RandomULong_EncodingThenDecoding_ReturnsExpectedResult(ulong testValue)
        {
            byte[] decoded = _SystemUnderTest.Encode(testValue);
            ulong actual = _SystemUnderTest.DecodeToUInt64(decoded);
            Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
        }

        [Fact]
        public void GivenInvalidString_Encoding_ThrowsEncodingException()
        {
            const string testData = "Z383937376377P";

            Assert.Throws<CodecParsingException>(() => _SystemUnderTest.Encode(testData));
        }

        [Fact]
        public void GivenValidByteArray_GetByteCount_ReturnsExpectedResult()
        {
            byte[] testData = {0x39, 0x39, 0x44, 0x11, 0x00};
            const int expected = 5;
            int actual = _SystemUnderTest.GetByteCount(testData);

            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void GivenValidString_GetByteCount_ReturnsExpectedResult()
        {
            byte[] testData = {0x39, 0x39, 0x44, 0x11, 0x00};
            const int expected = 5;
            int actual = _SystemUnderTest.GetByteCount(testData);

            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        [Fact]
        public void GivenValidString_IsValid_ReturnsTrue()
        {
            string testData = "8373849348FFFFFFFF";

            Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
        }

        [Fact]
        public void GivenInvalidString_IsValid_ReturnsFalse()
        {
            string testData = "(8373?8493+48FFFFFFFF";

            Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
        }

        [Fact]
        public void GivenValidByteArray_IsValid_ReturnsTrue()
        {
            byte[] testData = {0x35, 0x98, 0x74, 0xFF};

            Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
        }

        [Fact]
        public void GivenInvalidByteArray_IsValid_ReturnsFalse()
        {
            byte[] testData = {0x3C, 0x39, 0x44, 0x1D, 0x00};

            Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
        }

        #endregion
    }
}