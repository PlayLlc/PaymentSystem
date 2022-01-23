//using Play.Core.Extensions;
//using Play.Core.Iso8825.Ber.Identifiers;
//using Play.Core.Iso8825.Ber.Identifiers.Specifications;
//using Play.Core.Iso8825.Tests.Ber.TestData;
//using Xunit;

//namespace Play.Core.Iso8825.Tests.Ber.TagTests.ShortIdentifiers
//{
//    public partial class TagFactoryTests
//    {
//        [Fact]
//        public void Byte_WithApplicationClassFlag_CreatesTagWithCorrectClass()
//        {
//            var testValue = Spec.ShortIdentifier.TagNumber.MaxLength.GetMaskedByte(BitCount.Eight, BitCount.Seven)
//                .SetBits((byte) ClassType.Application);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.Class;

//            Assert.Equal(result, ClassType.Application);
//        }

//        [Fact]
//        public void Byte_WithConstructedFlag_CreatesTagWithCorrectDataObjectType()
//        {
//            byte testValue = 0b_01111011;

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.DataObject;

//            Assert.Equal(result, DataObjectType.Constructed);
//        }

//        [Fact]
//        public void Byte_WithContextSpecificClassFlag_CreatesTagWithCorrectClass()
//        {
//            var testValue = Spec.ShortIdentifier.TagNumber.MaxLength.GetMaskedByte(BitCount.Eight, BitCount.Seven)
//                .SetBits((byte) ClassType.ContextSpecific);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.Class;

//            Assert.Equal(result, ClassType.ContextSpecific);
//        }

//        [Fact]
//        public void Byte_WithPrimitiveFlag_CreatesTagWithCorrectDataObjectType()
//        {
//            byte testValue = 0b_01011011;

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.DataObject;

//            Assert.Equal(result, DataObjectType.Primitive);
//        }

//        [Fact]
//        public void Byte_WithPrivateClassFlag_CreatesTagWithCorrectClass()
//        {
//            var testValue = Spec.ShortIdentifier.TagNumber.MaxLength.GetMaskedByte(BitCount.Eight, BitCount.Seven)
//                .SetBits((byte) ClassType.Private);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.Class;

//            Assert.Equal(result, ClassType.Private);
//        }

//        [Fact]
//        public void Byte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
//        {
//            var testValue = Spec.ShortIdentifier.TagNumber.MaxLength.GetMaskedByte(BitCount.Eight, BitCount.Seven);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.Class;

//            Assert.Equal(result, ClassType.Universal);
//        }

//        [Fact]
//        public void Byte_WithValueLessThan31_CreatesTagWithCorrectTagNumber()
//        {
//            byte tagNumber = 0b_00000101;
//            var testValue = (byte) (0b_10100000 | tagNumber);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.TagNumber;

//            Assert.Equal(result, tagNumber);
//        }

//        [Fact]
//        public void RandomByte_WithApplicationClassFlag_CreatesTagWithCorrectClass()
//        {
//            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedByte(BitCount.Eight, BitCount.Seven)
//                .SetBits((byte) ClassType.Application);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.Class;

//            Assert.Equal(result, ClassType.Application);
//        }

//        [Fact]
//        public void RandomByte_WithConstructedFlag_CreatesTagWithCorrectDataObjectType()
//        {
//            var testValue =
//                ((byte) _Random.Next(0, byte.MaxValue)).SetBits((byte) DataObjectType.Constructed);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.DataObject;

//            Assert.Equal(result, DataObjectType.Constructed);
//        }

//        [Fact]
//        public void RandomByte_WithContextSpecificClassFlag_CreatesTagWithCorrectClass()
//        {
//            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedByte(BitCount.Eight, BitCount.Seven)
//                .SetBits((byte) ClassType.ContextSpecific);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.Class;

//            Assert.Equal(result, ClassType.ContextSpecific);
//        }

//        [Fact]
//        public void RandomByte_WithPrimitiveFlag_CreatesTagWithCorrectDataObjectType()
//        {
//            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedByte((byte) BitCount.Six);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.DataObject;

//            Assert.Equal(result, DataObjectType.Primitive);
//        }

//        [Fact]
//        public void RandomByte_WithPrivateClassFlag_CreatesTagWithCorrectClass()
//        {
//            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedByte(BitCount.Eight, BitCount.Seven)
//                .SetBits((byte) ClassType.ContextSpecific);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.Class;

//            Assert.Equal(result, ClassType.ContextSpecific);
//        }

//        [Fact]
//        public void RandomByte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
//        {
//            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedByte(BitCount.Eight, BitCount.Seven)
//                .SetBits((byte) ClassType.Universal);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.Class;

//            Assert.Equal(result, ClassType.Universal);
//        }

//        [Fact]
//        public void RandomByte_WithValueLessThan31_CreatesTagWithCorrectTagNumber()
//        {
//            var tagNumber = (byte) _Random.Next(0, Spec.ShortIdentifier.TagNumber.MaxLength);
//            var testValue = ((byte) _Random.Next(0, byte.MaxValue))
//                .GetMaskedByte(Spec.LeadingOctet.LongIdentifierFlag).SetBits(tagNumber);

//            var sut = new ShortIdentifier(testValue);

//            var result = sut.TagNumber;

//            Assert.Equal(result, tagNumber);
//        }

//        [Fact]
//        public void RandomShortIdentifierComponentParts_WhenInitializing_CreatesByteWithCorrectValue()
//        {
//            var expectedClassType = ShortIdentifierTestValueFactory.GetClassType(_Random);
//            var expectedDataObjectType = ShortIdentifierTestValueFactory.GetDataObjectType(_Random);
//            var expectedTagNumber = ShortIdentifierTestValueFactory.GetTagNumber(_Random);

//            var initializationValue = (byte) ((byte) expectedClassType | (byte) expectedDataObjectType |
//                                              expectedTagNumber);

//            var testThing = new ShortIdentifier(initializationValue);

//            Assert.Equal(expectedClassType, testThing.Class);
//            Assert.Equal(expectedDataObjectType, testThing.DataObject);
//            Assert.Equal(expectedTagNumber, testThing.TagNumber);
//        }
//    }
//}

