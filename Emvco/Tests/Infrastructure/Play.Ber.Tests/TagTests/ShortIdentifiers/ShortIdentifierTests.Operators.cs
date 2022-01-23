//using Play.Core.Iso8825.Ber.Identifiers;
//using Play.Core.Iso8825.Tests.Ber.TestData;
//using Xunit;

//namespace Play.Core.Iso8825.Tests.Ber.TagTests.ShortIdentifiers
//{
//    public partial class TagFactoryTests
//    {
//        [Fact]
//        public void RandomShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
//        {
//            var expectedValue = ShortIdentifierTestValueFactory.CreateByte(_Random);
//            var testValue = new ShortIdentifier(expectedValue);

//            var sut = (int) testValue;

//            Assert.Equal(sut, expectedValue);
//        }

//        [Fact]
//        public void RandomShortIdentifier_WhenExplicitlyCastingToTag_CreatesTagWithCorrectInstanceValues()
//        {
//            var testValue = ShortIdentifierTestValueFactory.Create(_Random);
//            var sut = (Tag) testValue;

//            Assert.Equal(testValue.TagNumber, sut.TagNumber);
//            Assert.Equal(testValue.Class, sut.Class);
//            Assert.Equal(testValue.DataObject, sut.DataObject);
//            Assert.Equal(testValue.ByteCount, sut.ByteCount);
//        }

//        [Fact]
//        public void ShortIdentifier_WhenExplicitlyCastingToInt_CreatesIntWithCorrectValue()
//        {
//            var testValue = new ShortIdentifier(0b_00000001);
//            var sut = (int) testValue;

//            Assert.Equal(0x01, sut);
//        }

//        [Fact]
//        public void ShortIdentifier_WhenExplicitlyCastingToTag_CreatesTagWithCorrectInstanceValues()
//        {
//            var testValue = new ShortIdentifier(0b_00000001);
//            var sut = (Tag) testValue;

//            Assert.Equal(0x01, sut.TagNumber);
//            Assert.Equal(ClassType.Universal, sut.Class);
//            Assert.Equal(DataObjectType.Primitive, sut.DataObject);
//            Assert.Equal(1, sut.ByteCount);
//        }
//    }
//}

