//using System;
//using Play.Core.Extensions;
//using Play.Core.Iso8825.Ber.Identifiers;
//using Play.Core.Iso8825.Tests.Ber.TagTests.Tags.TestData;
//using Play.Core.Iso8825.Tests.Ber.TestData;
//using Xunit;

//namespace Play.Core.Iso8825.Tests.Ber.TagTests.Tags.TagFactories
//{
//    public partial class TagTests
//    {
//        #region ShortIdentifiers

//        [Fact]
//        public void RandomShortTag_WhenSerializingInstance_CreatesByteWithCorrectValue()
//        {
//            var expectedValue = ShortIdentifierTestValueFactory.CreateByte(_Random);
//            var testValue = _TagFactory.ParseFirstTag(expectedValue.AsReadOnlySpan());

//            var sut = testValue.Serialize();

//            Assert.Equal(sut[0], expectedValue);
//        }

//        [Fact]
//        public void ShortTag_WhenSerializingInstance_CreatesByteWithCorrectValue()
//        {
//            byte expectedValue = 0b00000001;
//            var testValue = _TagFactory.ParseFirstTag(expectedValue.AsReadOnlySpan());

//            var sut = testValue.Serialize();

//            Assert.Equal(sut[0], expectedValue);
//        }

//        [Fact]
//        public void ShortTagComponentParts_WhenSerializingInstance_CreatesByteWithCorrectValue()
//        {
//            byte expectedValue = 0b00000001;
//            var testValue = _TagFactory.ParseFirstTag(expectedValue.AsReadOnlySpan());

//            var sut = testValue.Serialize();

//            Assert.Equal(sut[0], expectedValue);
//        }

//        #endregion

//        #region Short & Long Identifiers

//        [Theory]
//        [MemberData(nameof(TagTestValues.GetValidTags), MemberType = typeof(TagTestValues))]
//        public void ValidTagTestValue_ExplicitlyCastToInt_ReturnsCorrectValue(TagTestValue value)
//        {
//            Tag sut = _TagFactory.ParseFirstTag(value.Value.AsSpan());
//            Assert.Equal((int) value.GetInt(), (int) sut);
//        }

//        [Theory]
//        [MemberData(nameof(TagLengthValueTestValues.GetValidTestValues),
//            MemberType = typeof(TagLengthValueTestValues))]
//        public void TagLengthValueTestValue_ExplicitlyCastToInt_ReturnsCorrectValue(TagLengthValueTestValue value)
//        {
//            Tag sut = _TagFactory.ParseFirstTag(value.Tag.AsSpan());
//            Assert.Equal((int) value.Tag.GetInt32(), (int) sut);
//        }

//        #endregion

//    }
//}

