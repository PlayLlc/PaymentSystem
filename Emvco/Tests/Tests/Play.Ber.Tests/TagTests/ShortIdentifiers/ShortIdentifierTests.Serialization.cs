//using Play.Core.Iso8825.Ber.Identifiers;
//using Play.Core.Iso8825.Tests.Ber.TestData;
//using Xunit;

//namespace Play.Core.Iso8825.Tests.Ber.TagTests.ShortIdentifiers
//{
//    public partial class TagFactoryTests
//    {
//        [Fact]
//        public void RandomShortIdentifier_WhenSerializingInstance_CreatesByteWithCorrectValue()
//        {
//            var expectedValue = ShortIdentifierTestValueFactory.CreateByte(_Random);
//            var testValue = new ShortIdentifier(expectedValue);

//            var sut = testValue.Serialize();

//            Assert.Equal(sut, expectedValue);
//        }

//        [Fact]
//        public void
//            RandomShortIdentifierComponentParts_WhenInitializingAndSerializingInstance_CreatesByteWithCorrectValue()
//        {
//            var expectedClassType = ShortIdentifierTestValueFactory.GetClassType(_Random);
//            var expectedDataObjectType = ShortIdentifierTestValueFactory.GetDataObjectType(_Random);
//            var expectedTagNumber = ShortIdentifierTestValueFactory.GetTagNumber(_Random);

//            var initializationValue = (byte) ((byte) expectedClassType | (byte) expectedDataObjectType |
//                                              expectedTagNumber);

//            var testThing = new ShortIdentifier(initializationValue);
//            var sut = testThing.Serialize();

//            Assert.Equal(sut, initializationValue);
//        }

//        [Fact]
//        public void
//            RandomShortIdentifierComponentParts_WhenSerializingFromStatic_CreatesByteWithCorrectValue()
//        {
//            var expectedClassType = ShortIdentifierTestValueFactory.GetClassType(_Random);
//            var expectedDataObjectType = ShortIdentifierTestValueFactory.GetDataObjectType(_Random);
//            var expectedTagNumber = ShortIdentifierTestValueFactory.GetTagNumber(_Random);

//            var expectedValue = (byte) ((byte) expectedClassType | (byte) expectedDataObjectType |
//                                        expectedTagNumber);

//            var sut = ShortIdentifier.Serialize(expectedClassType, expectedDataObjectType, expectedTagNumber);

//            Assert.Equal(sut, expectedValue);
//        }

//        [Fact]
//        public void ShortIdentifier_WhenSerializingInstance_CreatesByteWithCorrectValue()
//        {
//            byte expectedValue = 0b00000001;
//            var testValue = new ShortIdentifier(expectedValue);

//            var sut = testValue.Serialize();

//            Assert.Equal(sut, expectedValue);
//        }

//        [Fact]
//        public void ShortIdentifierComponentParts_WhenSerializingInstance_CreatesByteWithCorrectValue()
//        {
//            byte expectedValue = 0b00000001;
//            var testValue = new ShortIdentifier(expectedValue);

//            var sut = testValue.Serialize();

//            Assert.Equal(sut, expectedValue);
//        }
//    }
//}

