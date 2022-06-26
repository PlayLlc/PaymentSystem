using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Long;
using Play.Ber.Identifiers.Short;
using Play.Ber.Tests.TestData;
using Play.Core.Extensions;

using Xunit;

namespace Play.Core.Iso8825.Tests.Ber.TagTests.ShortIdentifiers
{
    public partial class TagFactoryTests
    {
        #region Instance Members

        [Fact]
        public void Byte_WithApplicationClassFlag_CreatesTagWithCorrectClass()
        {
            var testValue = ShortIdentifier.TagNumber.MaxValue.GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.Application);

            Tag sut = new(testValue);

            var result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Application);
        }

        [Fact]
        public void Byte_WithConstructedFlag_CreatesTagWithCorrectDataObjectType()
        {
            byte testValue = 0b_01111011;

            Tag sut = new(testValue);

            var result = sut.GetDataObject();

            Assert.Equal(result, DataObjectTypes.Constructed);
        }

        [Fact]
        public void Byte_WithContextSpecificClassFlag_CreatesTagWithCorrectClass()
        {
            var testValue = ShortIdentifier.TagNumber.MaxValue.GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.ContextSpecific);

            Tag sut = new(testValue);

            var result = sut.GetClass();

            Assert.Equal(result, ClassTypes.ContextSpecific);
        }

        [Fact]
        public void Byte_WithPrimitiveFlag_CreatesTagWithCorrectDataObjectType()
        {
            byte testValue = 0b_01011011;

            Tag sut = new(testValue);

            var result = sut.GetDataObject();

            Assert.Equal(result, DataObjectTypes.Primitive);
        }

        [Fact]
        public void Byte_WithPrivateClassFlag_CreatesTagWithCorrectClass()
        {
            var testValue = ShortIdentifier.TagNumber.MaxValue.SetBits((byte) ClassTypes.Private);

            Tag sut = new(testValue);

            var result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Private);
        }

        [Fact]
        public void Byte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
        {
            var testValue = ShortIdentifier.TagNumber.MaxValue;

            Tag sut = new(testValue);

            var result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Universal);
        }

        [Fact]
        public void Byte_WithValueLessThan31_CreatesTagWithCorrectTagNumber()
        {
            byte tagNumber = 0b_00000101;
            byte testValue = (byte) (0b_10100000 | tagNumber);

            Tag sut = new(testValue);

            var result = sut.GetTagNumber();

            Assert.Equal(result, tagNumber);
        }

        [Fact]
        public void RandomByte_WithApplicationClassFlag_CreatesTagWithCorrectClass()
        {
            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.Application);

            Tag sut = new(testValue);

            var result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Application);
        }

        [Fact]
        public void RandomByte_WithConstructedFlag_CreatesTagWithCorrectDataObjectType()
        {
            byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).SetBits((byte) DataObjectTypes.Constructed);

            Tag sut = new(testValue);

            var result = sut.GetDataObject();

            Assert.Equal(result, DataObjectTypes.Constructed);
        }

        [Fact]
        public void RandomByte_WithContextSpecificClassFlag_CreatesTagWithCorrectClass()
        {
            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.ContextSpecific);

            Tag sut = new(testValue);

            var result = sut.GetClass();

            Assert.Equal(result, ClassTypes.ContextSpecific);
        }

        [Fact]
        public void RandomByte_WithPrimitiveFlag_CreatesTagWithCorrectDataObjectType()
        {
            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue((byte) Bits.Six);

            Tag sut = new(testValue);

            var result = sut.GetDataObject();

            Assert.Equal(result, DataObjectTypes.Primitive);
        }

        [Fact]
        public void RandomByte_WithPrivateClassFlag_CreatesTagWithCorrectClass()
        {
            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.ContextSpecific);

            Tag sut = new(testValue);

            var result = sut.GetClass();

            Assert.Equal(result, ClassTypes.ContextSpecific);
        }

        [Fact]
        public void RandomByte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
        {
            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.Universal);

            Tag sut = new(testValue);

            var result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Universal);
        }

        [Fact]
        public void RandomByte_WithValueLessThan31_CreatesTagWithCorrectTagNumber()
        {
            byte tagNumber = (byte) _Random.Next(0, ShortIdentifier.TagNumber.MaxValue);
            var testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(LongIdentifier.LongIdentifierFlag).SetBits(tagNumber);

            Tag sut = new(testValue);

            Assert.Equal(tagNumber, sut.GetTagNumber());
        }

        [Fact]
        public void RandomShortIdentifierComponentParts_WhenInitializing_CreatesByteWithCorrectValue()
        {
            ClassTypes? expectedClassType = ShortIdentifierTestValueFactory.GetClassType(_Random);
            DataObjectTypes? expectedDataObjectType = ShortIdentifierTestValueFactory.GetDataObjectType(_Random);
            byte expectedTagNumber = ShortIdentifierTestValueFactory.GetTagNumber(_Random);

            byte initializationValue = (byte) ((byte) expectedClassType | (byte) expectedDataObjectType | expectedTagNumber);

            Tag sut = new(initializationValue);

            Assert.Equal(expectedClassType, sut.GetClass());
            Assert.Equal(expectedDataObjectType, sut.GetDataObject());
            Assert.Equal(expectedTagNumber, sut.GetTagNumber());
        }

        #endregion
    }
}