﻿using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ApplicationIdentifierTests
{
    #region Instance Members

    /// <summary>
    ///     BerEncoding_DeserializingDataElement_CreatesPrimitiveValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        ApplicationIdentifierTestTlv testData = new();
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(sut);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        ApplicationIdentifierTestTlv testData = new();
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        ApplicationIdentifierTestTlv testData = new();
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        ApplicationIdentifierTestTlv testData = new();
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ApplicationIdentifier.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     TagLengthValue_SerializingToBer_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        ApplicationIdentifierTestTlv testData = new();
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());

        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     InvalidBerEncoding_DeserializingDataElement_Throws
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void InvalidValueInput_DeserializingDataElement_Throws()
    {
        ApplicationIdentifierTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03 });

        Assert.Throws<DataElementParsingException>(() => ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        ApplicationIdentifierTestTlv testData = new();
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        ushort testResult = sut.GetValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    /// <summary>
    ///     DataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        ApplicationIdentifierTestTlv testData = new();
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    /// <summary>
    ///     CustomDataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void CustomDataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        ApplicationIdentifierTestTlv testData = new(new byte[] { 0x08, 0x32, 0x1C, 0x01, 0xC, 0x1E });
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        ushort testResult = sut.GetValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    /// <summary>
    ///     CustomDataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void CustomDataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        ApplicationIdentifierTestTlv testData = new(new byte[]
        {
            0x08, 0x32, 0x1C, 0x01, 0x1C, 0x14, 0x22, 0x10, 0x03, 0x05,
            0x01, 0x04
        });
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void ApplicationIdentifier_IsPartialMatch_ReturnsTrue()
    {
        ApplicationIdentifierTestTlv testData = new();
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());

        ApplicationIdentifierTestTlv testData2 = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01, 0x12 });
        ApplicationIdentifier sut2 = ApplicationIdentifier.Decode(testData2.EncodeValue().AsSpan());

        Assert.True(sut.IsPartialMatch(sut2));
    }

    [Fact]
    public void ApplicationIdentifier_IsPartialMatch_ReturnsFalse()
    {
        ApplicationIdentifierTestTlv testData = new();
        ApplicationIdentifier sut = ApplicationIdentifier.Decode(testData.EncodeValue().AsSpan());

        ApplicationIdentifierTestTlv testData2 = new(new byte[] { 0x08, 0x01, 0x03, 0x01, 0x01, 0x12 });
        ApplicationIdentifier sut2 = ApplicationIdentifier.Decode(testData2.EncodeValue().AsSpan());

        Assert.False(sut.IsPartialMatch(sut2));
    }

    #endregion
}