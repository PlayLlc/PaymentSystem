using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class Track1DiscretionaryDataTests
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
        Track1DiscretionaryDataTestTlv testData = new();
        Track1DiscretionaryData testValue = Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan());
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
        Track1DiscretionaryDataTestTlv testData = new();
        Track1DiscretionaryData sut = Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan());
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
        Track1DiscretionaryDataTestTlv testData = new();
        Track1DiscretionaryData sut = Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan());
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
        Track1DiscretionaryDataTestTlv testData = new();
        Track1DiscretionaryData sut = Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(Track1DiscretionaryData.Tag, testData.EncodeValue());
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
        Track1DiscretionaryDataTestTlv testData = new();
        Track1DiscretionaryData sut = Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan());

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
    public void InvalidBerEncoding_DeserializingDataElementWithBytesNotPresentInCharMap_ThrowsKeyNotFoundException()
    {
        Track1DiscretionaryDataTestTlv testData = new(new byte[] {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01});

        Assert.Throws<KeyNotFoundException>(() => Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan()));
    }

    [Fact]
    public void InvalidBerEncoding_DeserializingData_Throws()
    {
        Track1DiscretionaryDataTestTlv testData = new(new byte[]
        {
            61, 84, 98, 62, 33, 44, 92, 42, 80, 64,
            62, 83, 99, 61, 34, 44, 92, 43, 80, 65,
            63, 82, 100, 60, 35, 44, 92, 44, 80, 66,
            61, 84, 98, 62, 33, 44, 92, 42, 80, 64,
            62, 83, 99, 61, 34, 44, 92, 43, 80, 65,
            63, 82, 100, 60, 35, 44, 92, 44, 80, 66
        });

        Assert.Throws<DataElementParsingException>(() => Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        Track1DiscretionaryDataTestTlv testData = new();
        Track1DiscretionaryData sut = Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan());
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
        Track1DiscretionaryDataTestTlv testData = new();
        Track1DiscretionaryData sut = Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan());
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
        Track1DiscretionaryDataTestTlv testData = new(new byte[]
        {
            62, 83, 99, 61, 34, 44, 92, 43, 80, 65,
            72, 73, 99, 101, 102, 103, 111, 112, 113, 114
        });
        Track1DiscretionaryData sut = Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan());
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
        Track1DiscretionaryDataTestTlv testData = new(new byte[] {62, 83, 99, 61, 34, 44, 92, 43, 80, 65});

        Track1DiscretionaryData sut = Track1DiscretionaryData.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion
}