using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Emv.DataElements.Tests;

public class ProcessingOptionsDataObjectListTests
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
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList testValue = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
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
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
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
        ProcessingOptionsDataObjectListTestTlv testData = new();
        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ProcessingOptionsDataObjectList.Tag, testData.EncodeValue());
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
        ProcessingOptionsDataObjectListTestTlv testData = new();

        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());
        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     TagLengthValue_CreatingDataObjectListResult_IsNotNull
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void TagLengthValue_CreatingDataObjectListResult_IsNotNull()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();

        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());

        DataObjectListResult testValue = sut.AsDataObjectListResult(testData.GetTerminalValues());

        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     TagLengthValue_CreatingDataObjectListResult_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void TagLengthValue_CreatingDataObjectListResult_ReturnsExpectedResult()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();

        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());

        DataObjectListResult expectedResult = new(testData.GetTerminalValues());
        DataObjectListResult testValue = sut.AsDataObjectListResult(testData.GetTerminalValues());

        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     TagLengthValue_CreatingCommandTemplate_IsNotNull
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void TagLengthValue_CreatingCommandTemplate_IsNotNull()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();

        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());

        CommandTemplate? testValue = sut.AsCommandTemplate(testData.GetTerminalValues());

        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     TagLengthValue_CreatingCommandTemplate_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void TagLengthValue_CreatingCommandTemplate_ReturnsExpectedResult()
    {
        ProcessingOptionsDataObjectListTestTlv testData = new();

        ProcessingOptionsDataObjectList sut = ProcessingOptionsDataObjectList.Decode(testData.EncodeValue().AsSpan());

        CommandTemplate expectedResult = CommandTemplate.Decode(testData.GetTerminalValues()
                                                                    .SelectMany(a => a.EncodeTagLengthValue(EmvCodec.GetBerCodec()))
                                                                    .ToArray().AsSpan());

        CommandTemplate testValue = sut.AsCommandTemplate(testData.GetTerminalValues());

        Assert.Equal(expectedResult, testValue);
    }

    #endregion

    // TODO: Need to test the creation of a DataObjectListResult
    // ProcessingOptionsDataObjectList.AsDataObjectListResult(TagLengthValue[] terminalValues);
}