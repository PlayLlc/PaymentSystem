using System;

using Play.Ber.DataObjects;
using Play.Emv.DataElements.Emv;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Emv.DataElements.Tests;

public class KernelIdentifierTests
{
    #region Instance Members

    [Fact]
    public void Test()
    {
        ApplicationDedicatedFileNameTestTlv testData = new(new byte[] {0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40});
        ApplicationDedicatedFileName testValue = ApplicationDedicatedFileName.Decode(testData.EncodeValue().AsSpan());
        byte[] testResult = testValue.EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void Test2()
    {
        ApplicationPriorityIndicatorTestTlv testData = new(new byte[] {0x02});
        ApplicationPriorityIndicator testValue = ApplicationPriorityIndicator.Decode(testData.EncodeValue().AsSpan());
        byte[] testResult = testValue.EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void Test3()
    {
        KernelIdentifierTestTlv testData = new(new byte[] {0x03});
        KernelIdentifier testValue = KernelIdentifier.Decode(testData.EncodeValue().AsSpan());
        byte[] testResult = testValue.EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        KernelIdentifierTestTlv testData = new();
        KernelIdentifier testValue = KernelIdentifier.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        KernelIdentifierTestTlv testData = new();
        KernelIdentifier sut = KernelIdentifier.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        KernelIdentifierTestTlv testData = new();
        KernelIdentifier sut = KernelIdentifier.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        KernelIdentifierTestTlv testData = new();
        KernelIdentifier sut = KernelIdentifier.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(KernelIdentifier.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        KernelIdentifierTestTlv testData = new();

        KernelIdentifier sut = KernelIdentifier.Decode(testData.EncodeValue().AsSpan());
        byte[]? encoded = sut.EncodeValue();
        TagLengthValue? tlv = sut.AsTagLengthValue();
        byte[]? tlvRaw = tlv.EncodeTagLengthValue();
        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        TagLengthValue? expectedResultTlv = new(testData.GetTag(), testData.EncodeValue());

        Assert.Equal(testValue, expectedResult);
    }

    #endregion
}