using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Ber.Tests.TestDoubles;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests;

public class DataObjectListTests
{
    #region Instance Members

    [Fact]
    public void BerEncodingTagLengths_Deserializing_CreatesDataObjectList()
    {
        Tag[] expectedResult = {ApplicationExpirationDate.Tag, CardholderName.Tag, KernelIdentifier.Tag};
        TagLength[] testData = MockDataObjectListTestFactory.GetEmptyTagLengths(expectedResult).ToArray();
        MockDataObjectList sut = MockDataObjectListTestFactory.Create(testData);

        Assert.NotNull(sut);
    }

    [Fact]
    public void BerEncodingTagLengths_InvokingGetRequestedItems_ReturnsExpectedTags()
    {
        Tag[] expectedResult = {ApplicationExpirationDate.Tag, CardholderName.Tag, KernelIdentifier.Tag};
        TagLength[] testData = MockDataObjectListTestFactory.GetEmptyTagLengths(expectedResult).ToArray();
        MockDataObjectList sut = MockDataObjectListTestFactory.Create(testData);
        TagLength[] testValue = sut.GetRequestedItems();

        foreach (Tag item in expectedResult)
        {
            Tag? result = testValue.FirstOrDefault(a => a.GetTag() == item).GetTag();
            Assert.Equal(result, item);
        }
    }

    [Fact]
    public void BerEncodingNullTagLengths_InvokingGetRequestedItems_ReturnsExpectedTagLengthPairs()
    {
        TagLength[] expectedResult =
        {
            new(ApplicationExpirationDate.Tag, new ApplicationExpirationDateTestTlv().AsTagLengthValue().GetLength()),
            new(KernelIdentifier.Tag, new KernelIdentifierTestTlv().AsTagLengthValue().GetLength()),
            new(CardholderName.Tag, new CardholderNameTestTlv().AsTagLengthValue().GetLength())

            //new(CardholderName.Tag, new CardholderNameTestTlv().AsTagLengthValue().GetLength()),
            //new(KernelIdentifier.Tag, new KernelIdentifierTestTlv().AsTagLengthValue().GetLength())
        };

        MockDataObjectList sut = MockDataObjectListTestFactory.Create(expectedResult);
        TagLength[] testValue = sut.GetRequestedItems();

        foreach (TagLength item in expectedResult)
        {
            TagLength result = testValue.FirstOrDefault(a => a == item);
            Assert.Equal(result, item);
        }
    }

    [Fact]
    public void BerEncodingTagLengths_InvokingGetRequestedItems_ReturnsExpectedLength()
    {
        Length expectedResult = new(new byte[] {0x01}.AsSpan());

        TagLength[] testData = {new(CardholderName.Tag, expectedResult)};

        MockDataObjectList sut = MockDataObjectListTestFactory.Create(testData);

        TagLength[] testValue = sut.GetRequestedItems();

        foreach (TagLength item in testValue)
        {
            Length result = item.GetLength();
            Assert.Equal(expectedResult, item.GetLength());
        }
    }

    [Fact]
    public void BerEncodingTagLengths_InvokingAsCommandTemplate_ReturnsCommandTemplate()
    {
        TagLengthValue[] testData = {new ApplicationExpirationDateTestTlv().AsTagLengthValue()};

        MockDataObjectList sut =
            MockDataObjectListTestFactory.Create(testData.Select(a => new TagLength(a.GetTag(), a.EncodeValue())).ToArray());
        CommandTemplate commandTemplate = sut.AsCommandTemplate(testData);
        Assert.NotNull(commandTemplate);
    }

    [Fact]
    public void BerEncodingTagLengths_InvokingAsCommandTemplate_ReturnsExpectedCommandTemplate()
    {
        TagLengthValue[] testData = {new ApplicationExpirationDateTestTlv().AsTagLengthValue()};

        MockDataObjectList sut =
            MockDataObjectListTestFactory.Create(testData.Select(a => new TagLength(a.GetTag(), a.EncodeValue())).ToArray());
        CommandTemplate commandTemplate = sut.AsCommandTemplate(testData);
        byte[] expectedResult = testData.SelectMany(a => a.EncodeTagLengthValue()).ToArray();
        byte[] testValue = commandTemplate.EncodeValue();

        Assert.Equal(expectedResult, testValue);
    }

    #endregion
}