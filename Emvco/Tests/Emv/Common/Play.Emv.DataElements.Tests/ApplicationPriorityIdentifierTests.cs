﻿using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Emv.DataElements.Tests;

public class ApplicationPriorityIdentifierTests
{
    #region Instance Values

    private readonly BerCodec _BerCodec = new(EmvCodec.Configuration);

    #endregion

    #region Instance Members

    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        ApplicationPriorityIndicatorTestTlv testData = new();
        ApplicationPriorityIndicator testValue = ApplicationPriorityIndicator.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        ApplicationPriorityIndicatorTestTlv testData = new();
        ApplicationPriorityIndicator sut = ApplicationPriorityIndicator.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        ApplicationPriorityIndicatorTestTlv testData = new();
        ApplicationPriorityIndicator sut = ApplicationPriorityIndicator.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        ApplicationPriorityIndicatorTestTlv testData = new();
        ApplicationPriorityIndicator sut = ApplicationPriorityIndicator.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ApplicationPriorityIndicator.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        ApplicationPriorityIndicatorTestTlv testData = new();

        ApplicationPriorityIndicator sut = ApplicationPriorityIndicator.Decode(testData.EncodeValue().AsSpan());
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