using System;

using Play.Ber.Identifiers;
using Play.Ber.Tests.TestData;
using Play.Core.Extensions;
using Play.Core.Iso8825.Tests.Ber.TagTests.Tags.TestData;
using Play.Core.Iso8825.Tests.Ber.TestData;

using Xunit;

namespace Play.Ber.Tests.TagTests.Tags.TagFactories
{
    public partial class TagTests
    {
        #region ShortIdentifiers

        [Fact]
        public void RandomShortTag_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = ShortIdentifierTestValueFactory.CreateByte(_Random);
            var testValue = new Tag(expectedValue.AsReadOnlySpan());

            var sut = testValue.Serialize();

            Assert.Equal(sut[0], expectedValue);
        }

        [Fact]
        public void ShortTag_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = 0b00000001;
            var testValue = new Tag(expectedValue.AsReadOnlySpan());

            var sut = testValue.Serialize();

            Assert.Equal(sut[0], expectedValue);
        }

        [Fact]
        public void ShortTagComponentParts_WhenSerializingInstance_CreatesByteWithCorrectValue()
        {
            byte expectedValue = 0b00000001;
            var testValue = new Tag(expectedValue.AsReadOnlySpan());

            var sut = testValue.Serialize();

            Assert.Equal(sut[0], expectedValue);
        }

        #endregion

        #region Short & Long Identifiers

        [Theory]
        [MemberData(nameof(TagTestValues.GetValidTags), MemberType = typeof(TagTestValues))]
        public void ValidTagTestValue_ExplicitlyCastToInt_ReturnsCorrectValue(TagTestValue value)
        {
            Tag sut = new(value.Value.AsSpan());
            var expected = value.GetUInt32();
            var actual = (uint) sut;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(TagLengthValueTestValues.GetValidTestValues), MemberType = typeof(TagLengthValueTestValues))]
        public void TagLengthValueTestValue_ExplicitlyCastToInt_ReturnsCorrectValue(TagLengthValueTestValue value)
        {
            Tag sut = new(value.Tag.AsSpan());
            Assert.Equal(value.Tag, sut.Serialize());
        }

        #endregion
    }
}