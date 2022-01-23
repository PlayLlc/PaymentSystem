using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Long;
using Play.Ber.Identifiers.Short;
using Play.Ber.Tests.TestData;
using Play.Core.Extensions;

using Xunit;

namespace Play.Ber.Tests.TagTests.Tags.TagFactories;

public partial class TagTests
{
    #region Instance Members

    [Fact]
    public void Byte_WithApplicationClassFlag_CreatesTagWithCorrectClass()
    {
        byte testValue = ShortIdentifier.TagNumber.MaxValue.GetMaskedValue(0b11000000).SetBits((byte) ClassType.Application);

        Tag sut = new(testValue);

        ClassType? result = sut.GetClass();

        Assert.Equal(result, ClassType.Application);
    }

    [Fact]
    public void Byte_WithConstructedFlag_CreatesTagWithCorrectDataObjectType()
    {
        byte testValue = 0b_01111011;

        Tag sut = new(testValue);

        DataObjectType? result = sut.GetDataObject();

        Assert.Equal(result, DataObjectType.Constructed);
    }

    [Fact]
    public void Byte_WithContextSpecificClassFlag_CreatesTagWithCorrectClass()
    {
        byte testValue = ShortIdentifier.TagNumber.MaxValue.GetMaskedValue(Bits.Eight, Bits.Seven)
            .SetBits((byte) ClassType.ContextSpecific);

        Tag sut = new(testValue);

        ClassType? result = sut.GetClass();

        Assert.Equal(result, ClassType.ContextSpecific);
    }

    [Fact]
    public void Byte_WithPrimitiveFlag_CreatesTagWithCorrectDataObjectType()
    {
        byte testValue = 0b_01011011;

        Tag sut = new(testValue);

        DataObjectType? result = sut.GetDataObject();

        Assert.Equal(result, DataObjectType.Primitive);
    }

    [Fact]
    public void Byte_WithPrivateClassFlag_CreatesTagWithCorrectClass()
    {
        byte testValue = ShortIdentifier.TagNumber.MaxValue.GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassType.Private);

        Tag sut = new(testValue);

        ClassType? result = sut.GetClass();

        Assert.Equal(result, ClassType.Private);
    }

    [Fact]
    public void Byte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
    {
        byte testValue = ShortIdentifier.TagNumber.MaxValue.GetMaskedValue(Bits.Eight, Bits.Seven);
        Tag sut = new(testValue);

        ClassType? result = sut.GetClass();

        Assert.Equal(result, ClassType.Universal);
    }

    [Fact]
    public void Byte_WithValueLessThan31_CreatesTagWithCorrectTagNumber()
    {
        byte tagNumber = 0b_00000101;
        byte testValue = (byte) (0b_10100000 | tagNumber);

        Tag sut = new(testValue);

        ushort result = sut.GetTagNumber();

        Assert.Equal(result, tagNumber);
    }

    [Fact]
    public void RandomByte_WithApplicationClassFlag_CreatesTagWithCorrectClass()
    {
        byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven)
            .SetBits((byte) ClassType.Application);

        Tag sut = new(testValue);

        ClassType? result = sut.GetClass();

        Assert.Equal(result, ClassType.Application);
    }

    [Fact]
    public void RandomByte_WithConstructedFlag_CreatesTagWithCorrectDataObjectType()
    {
        byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).SetBits((byte) DataObjectType.Constructed);

        Tag sut = new(testValue);

        DataObjectType? result = sut.GetDataObject();

        Assert.Equal(result, DataObjectType.Constructed);
    }

    [Fact]
    public void RandomByte_WithContextSpecificClassFlag_CreatesTagWithCorrectClass()
    {
        byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven)
            .SetBits((byte) ClassType.ContextSpecific);

        Tag sut = new(testValue);

        ClassType? result = sut.GetClass();

        Assert.Equal(result, ClassType.ContextSpecific);
    }

    [Fact]
    public void RandomByte_WithPrimitiveFlag_CreatesTagWithCorrectDataObjectType()
    {
        byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue((byte) Bits.Six);

        Tag sut = new(testValue);

        DataObjectType? result = sut.GetDataObject();

        Assert.Equal(result, DataObjectType.Primitive);
    }

    [Fact]
    public void RandomByte_WithPrivateClassFlag_CreatesTagWithCorrectClass()
    {
        byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven)
            .SetBits((byte) ClassType.ContextSpecific);

        Tag sut = new(testValue);

        ClassType? result = sut.GetClass();

        Assert.Equal(result, ClassType.ContextSpecific);
    }

    // TODO: WARNING - This test is failing indeterminately. That means there is a bug
    [Fact]
    public void RandomByte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
    {
        byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassType.Universal);
        testValue = (testValue & 0b00011111) == 0 ? (byte) (testValue - 1) : testValue;

        Tag sut = new(testValue);

        ClassType? result = sut.GetClass();

        Assert.Equal(result, ClassType.Universal);
    }

    [Fact]
    public void RandomByte_WithValueLessThan31_CreatesTagWithCorrectTagNumber()
    {
        byte tagNumber = (byte) _Random.Next(0, ShortIdentifier.TagNumber.MaxValue);
        byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(LongIdentifier.LongIdentifierFlag).SetBits(tagNumber);

        Tag sut = new(testValue);

        ushort result = sut.GetTagNumber();

        Assert.Equal(result, tagNumber);
    }

    [Fact]
    public void RandomShortIdentifierComponentParts_WhenInitializing_CreatesByteWithCorrectValue()
    {
        ClassType? expectedClassType = ShortIdentifierTestValueFactory.GetClassType(_Random);
        DataObjectType? expectedDataObjectType = ShortIdentifierTestValueFactory.GetDataObjectType(_Random);
        byte expectedTagNumber = ShortIdentifierTestValueFactory.GetTagNumber(_Random);

        byte initializationValue = (byte) ((byte) expectedClassType | (byte) expectedDataObjectType | expectedTagNumber);

        Tag sut = new(initializationValue.AsReadOnlySpan());

        Assert.Equal(expectedClassType, sut.GetClass());
        Assert.Equal(expectedDataObjectType, sut.GetDataObject());
        Assert.Equal(expectedTagNumber, sut.GetTagNumber());
    }

    #endregion
}