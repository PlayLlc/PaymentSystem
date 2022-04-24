//using Play.Core.Extensions;
//using Play.Core.Iso8825.Ber.Identifiers;
//using Play.Core.Iso8825.Tests.Ber.TestData;
//using Xunit;

//namespace Play.Core.Iso8825.Tests.Ber.TagTests.Tags.TagFactories
//{
//    public partial class TagTests
//    {
//        #region ShortIdentifiers

//        [Fact]
//        public void RandomShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
//        {
//            var expectedValue = ShortIdentifierTestValueFactory.CreateByte(_Random);
//            var testValue = _TagFactory.ParseFirstTag(expectedValue.AsReadOnlySpan());

//            var sut = (int) testValue;

//            Assert.Equal(sut, expectedValue);
//        }

//        [Fact]
//        public void RandomShortIdentifier_WhenExplicitlyCastingToTag_CreatesTagWithCorrectInstanceValues()
//        {
//            var testValue =
//                _TagFactory.ParseFirstTag(
//                    ShortIdentifierTestValueFactory.CreateByte(_Random).AsReadOnlySpan());
//            var sut = (Tag) testValue;

//            Assert.Equal(testValue.TagNumber, sut.TagNumber);
//            Assert.Equal(testValue.Class, sut.Class);
//            Assert.Equal(testValue.DataObject, sut.DataObject);
//            Assert.Equal(testValue.ByteCount, sut.ByteCount);
//        }

//        [Fact]
//        public void ShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
//        {
//            var testValue = _TagFactory.ParseFirstTag(((byte) 0b_00000001).AsReadOnlySpan());
//            var sut = (int) testValue;

//            Assert.Equal(0x01, sut);
//        }

//        [Fact]
//        public void ShortIdentifier_WhenExplicitlyCastingToTag_CreatesTagWithCorrectInstanceValues()
//        {
//            var testValue = _TagFactory.ParseFirstTag(((byte) 0b_00000001).AsReadOnlySpan());
//            var sut = (Tag) testValue;

//            Assert.Equal(0x01, sut.TagNumber);
//            Assert.Equal(ClassType.Universal, sut.Class);
//            Assert.Equal(DataObjectType.Primitive, sut.DataObject);
//            Assert.Equal(1, sut.ByteCount);
//        }

//        #endregion

//    }
//}

