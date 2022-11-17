using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class TagsToWriteBeforeGeneratingApplicationCryptogramTests
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
        TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv testData = new();
        TagsToWriteBeforeGeneratingApplicationCryptogram testValue = TagsToWriteBeforeGeneratingApplicationCryptogram.Decode(testData.EncodeValue().AsSpan());
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
        TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv testData = new();
        TagsToWriteBeforeGeneratingApplicationCryptogram sut = TagsToWriteBeforeGeneratingApplicationCryptogram.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv testData = new();
        TagsToWriteBeforeGeneratingApplicationCryptogram sut = TagsToWriteBeforeGeneratingApplicationCryptogram.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     InvalidBerEncoding_DeserializingDataElement_Throws
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void InvalidBerEncoding_DeserializingDataElement_Throws()
    {
        TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv testData = new(new byte[] {2, 12, 1});

        Assert.Throws<ArgumentOutOfRangeException>(() => TagsToWriteBeforeGeneratingApplicationCryptogram.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv testData = new();
        TagsToWriteBeforeGeneratingApplicationCryptogram sut = TagsToWriteBeforeGeneratingApplicationCryptogram.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(TagsToWriteBeforeGeneratingApplicationCryptogram.Tag, testData.EncodeValue());
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
        TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv testData = new();
        TagsToWriteBeforeGeneratingApplicationCryptogram sut = TagsToWriteBeforeGeneratingApplicationCryptogram.Decode(testData.EncodeValue().AsSpan());

        byte[] testValue = sut.EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv testData = new();
        TagsToWriteBeforeGeneratingApplicationCryptogram sut = TagsToWriteBeforeGeneratingApplicationCryptogram.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        int testResult = sut.GetValueByteCount();

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
        TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv testData = new();
        TagsToWriteBeforeGeneratingApplicationCryptogram sut = TagsToWriteBeforeGeneratingApplicationCryptogram.Decode(testData.EncodeValue().AsSpan());
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
        TagsToWriteBeforeGeneratingApplicationCryptogramTestTlv testData = new(new byte[] {0x9F, 0x76, 3, 1, 1, 1});

        TagsToWriteBeforeGeneratingApplicationCryptogram sut = TagsToWriteBeforeGeneratingApplicationCryptogram.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        int testResult = sut.GetValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion
}