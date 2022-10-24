using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class UnpredictableNumberTests
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
        UnpredictableNumberTestTlv testData = new();
        UnpredictableNumber testValue = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());
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
        UnpredictableNumberTestTlv testData = new();
        UnpredictableNumber sut = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());
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
        UnpredictableNumberTestTlv testData = new();
        UnpredictableNumber sut = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());
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
        UnpredictableNumberTestTlv testData = new();
        UnpredictableNumber sut = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(UnpredictableNumber.Tag, testData.EncodeValue());
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
        UnpredictableNumberTestTlv testData = new();
        UnpredictableNumber sut = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());

        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        UnpredictableNumberTestTlv testData = new();
        UnpredictableNumber sut = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());
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
        UnpredictableNumberTestTlv testData = new();
        UnpredictableNumber sut = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());
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
        UnpredictableNumberTestTlv testData = new(new byte[] { 0x4d, 0x3c, 0x2b, 0x1a });
        UnpredictableNumber sut = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());
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
        UnpredictableNumberTestTlv testData = new(new byte[]
        {
            0x4d, 0x3c, 0x2b, 0x1a
        });

        UnpredictableNumber sut = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void InvalidBerEncoding_EncodingToDataElement_Throws()
    {
        UnpredictableNumberTestTlv testData = new(new byte[]
        {
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
        });

        Assert.Throws<DataElementParsingException>(() => UnpredictableNumber.Decode(testData.EncodeValue().AsSpan()));
    }

    #endregion

    #region UnpredictableNumber

    [Fact]
    public void UnpredictableNumber_GetSetBitCount_ReturnsExpectedResult()
    {
        UnpredictableNumberTestTlv testData = new(new byte[]
        {
            0x4d, 0x3c, 0x2b, 0x1a
        });

        UnpredictableNumber sut = UnpredictableNumber.Decode(testData.EncodeValue().AsSpan());

        int expected = 15;
        Assert.Equal(expected, sut.GetSetBitCount());
    }

    #endregion
}
