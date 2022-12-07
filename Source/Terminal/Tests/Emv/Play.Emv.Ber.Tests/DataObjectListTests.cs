using System;
using System.Linq;

using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;
using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Tests.TestDoubles;

using Xunit;

namespace Play.Emv.Ber.Tests;

public class DataObjectListTests
{
    #region Instance Members

    /// <summary>
    ///     BerEncodingTagLengths_Deserializing_CreatesDataObjectList
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    [Fact]
    public void BerEncodingTagLengths_Deserializing_CreatesDataObjectList()
    {
        Tag[] expectedResult = {ApplicationExpirationDate.Tag, CardholderName.Tag, KernelIdentifier.Tag};
        TagLength[] testData = MockDataObjectListTestFactory.GetEmptyTagLengths(expectedResult).ToArray();
        MockDataObjectList sut = MockDataObjectListTestFactory.Create(testData);

        Assert.NotNull(sut);
    }

    /// <summary>
    ///     BerEncodingTagLengths_InvokingGetRequestedItems_ReturnsExpectedTags
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    ///// <summary>
    /////     BerEncodingNullTagLengths_InvokingGetRequestedItems_ReturnsExpectedTagLengthPairs
    ///// </summary>
    ///// <exception cref="BerParsingException"></exception>
    ///// <exception cref="InvalidOperationException"></exception>
    //[Fact]
    //public void BerEncodingNullTagLengths_InvokingGetRequestedItems_ReturnsExpectedTagLengthPairs()
    //{
    //    TagLength[] expectedResult =
    //    {
    //        new(ApplicationExpirationDate.Tag, new ApplicationExpirationDateTestTlv().AsPrimitiveValue().EncodeValue(EmvCodec.GetBerCodec())),
    //        new(KernelIdentifier.Tag, new KernelIdentifierTestTlv().AsPrimitiveValue().EncodeValue(EmvCodec.GetBerCodec())),
    //        new(CardholderName.Tag, new CardholderNameTestTlv().AsPrimitiveValue().EncodeValue(EmvCodec.GetBerCodec()))

    //        //new(CardholderName.Tag, new CardholderNameTestTlv().AsTagLengthValue().GetLength()),
    //        //new(KernelIdentifier.Tag, new KernelIdentifierTestTlv().AsTagLengthValue().GetLength())
    //    };

    //    MockDataObjectList sut = MockDataObjectListTestFactory.Create(expectedResult);
    //    TagLength[] testValue = sut.GetRequestedItems();

    //    foreach (TagLength item in expectedResult)
    //    {
    //        TagLength result = testValue.FirstOrDefault(a => a == item);
    //        Assert.Equal(result, item);
    //    }
    //}

    /// <summary>
    ///     BerEncodingTagLengths_InvokingGetRequestedItems_ReturnsExpectedLength
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    [Fact]
    public void BerEncodingTagLengths_InvokingGetRequestedItems_ReturnsExpectedLength()
    {
        Length expectedResult = new((uint) new byte[] {0x01}.Length);

        TagLength[] testData = {new(CardholderName.Tag, expectedResult)};

        MockDataObjectList sut = MockDataObjectListTestFactory.Create(testData);

        TagLength[] testValue = sut.GetRequestedItems();

        foreach (TagLength item in testValue)
        {
            Length result = item.GetLength();
            Assert.Equal(expectedResult, item.GetLength());
        }
    }

    #endregion

    //[Fact]
    //public void BerEncodingTagLengths_InvokingAsCommandTemplate_ReturnsCommandTemplate()
    //{
    //    PrimitiveValue[] testData = {new ApplicationExpirationDateTestTlv().AsPrimitiveValue()};

    //    MockDataObjectList sut =
    //        MockDataObjectListTestFactory.Create(testData.Select(a => new TagLength(a.GetTag(), a.EncodeValue(EmvCodec.GetBerCodec()))).ToArray());

    //    Mock<IReadTlvDatabase> databaseMock = new();
    //    databaseMock.Setup(a => a.IsPresentAndNotEmpty(It.IsAny<Tag>())).Returns(true);
    //    databaseMock.Setup(a => a.Get(ApplicationExpirationDate.Tag)).Returns(new ApplicationExpirationDateTestTlv().AsPrimitiveValue());

    //    CommandTemplate commandTemplate = sut.AsCommandTemplate(databaseMock.Object);
    //    Assert.NotNull(commandTemplate);
    //}

    ///// <summary>
    /////     BerEncodingTagLengths_InvokingAsCommandTemplate_ReturnsExpectedCommandTemplate
    ///// </summary>
    ///// <exception cref="BerParsingException"></exception>
    ///// <exception cref="InvalidOperationException"></exception>
    //[Fact]
    //public void BerEncodingTagLengths_InvokingAsCommandTemplate_ReturnsExpectedCommandTemplate()
    //{
    //    PrimitiveValue[] testData = {new ApplicationExpirationDateTestTlv().AsPrimitiveValue()};

    //    MockDataObjectList sut = MockDataObjectListTestFactory.Create(testData.Select(a => new TagLength(a.GetTag(), a.EncodeValue(EmvCodec.GetBerCodec())))
    //        .ToArray());

    //    Mock<IReadTlvDatabase> databaseMock = new();
    //    databaseMock.Setup(a => a.IsPresentAndNotEmpty(It.IsAny<Tag>())).Returns(true);
    //    databaseMock.Setup(a => a.Get(ApplicationExpirationDate.Tag)).Returns(new ApplicationExpirationDateTestTlv().AsPrimitiveValue());

    //    CommandTemplate commandTemplate = sut.AsCommandTemplate(databaseMock.Object);
    //    byte[] expectedResult = testData.SelectMany(a => a.EncodeTagLengthValue(EmvCodec.GetBerCodec())).ToArray();
    //    byte[] testValue = commandTemplate.EncodeValue();

    //    Assert.Equal(expectedResult, testValue);
    //}
}