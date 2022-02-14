using System;

using Play.Ber.DataObjects;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Emv.DataElements.Tests;

public class ApplicationFileLocatorTests
{
    #region Instance Members

    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        ApplicationFileLocatorTestTlv testData = new();
        ApplicationFileLocator testValue = ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        ApplicationFileLocatorTestTlv testData = new();
        ApplicationFileLocator sut = ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        ApplicationFileLocatorTestTlv testData = new();
        ApplicationFileLocator sut = ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        ApplicationFileLocatorTestTlv testData = new();
        ApplicationFileLocator sut = ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ApplicationFileLocator.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        ApplicationFileLocatorTestTlv testData = new();
        ApplicationFileLocator sut = ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan());

        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void InvalidBerEncoding_DeserializingDataElement_Throws()
    {
        ApplicationFileLocatorTestTlv testData = new(new byte[] {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01});

        Assert.Throws<ArgumentOutOfRangeException>(() => ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan()));
    }

    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        ApplicationFileLocatorTestTlv testData = new();
        ApplicationFileLocator sut = ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        ushort testResult = sut.GetValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void DataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        ApplicationFileLocatorTestTlv testData = new();
        ApplicationFileLocator sut = ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void CustomDataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        ApplicationFileLocatorTestTlv testData = new(new byte[] {0x08, 0x32, 0x1C, 0x01});
        ApplicationFileLocator sut = ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        ushort testResult = sut.GetValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void CustomDataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        ApplicationFileLocatorTestTlv testData = new(new byte[]
        {
            0x08, 0x32, 0x1C, 0x01, 0x1C, 0x14, 0x22, 0x10,
            0x03, 0x05, 0x01, 0x04
        });
        ApplicationFileLocator sut = ApplicationFileLocator.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion
}