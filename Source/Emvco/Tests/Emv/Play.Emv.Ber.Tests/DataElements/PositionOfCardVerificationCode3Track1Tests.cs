using System;

using Play.Ber.DataObjects;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class PositionOfCardVerificationCode3Track1Tests
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
        PositionOfCardVerificationCode3Track1TestTlv testData = new();
        PositionOfCardVerificationCode3Track1 testValue = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
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
        PositionOfCardVerificationCode3Track1TestTlv testData = new();
        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
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
        PositionOfCardVerificationCode3Track1TestTlv testData = new();
        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
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
        PositionOfCardVerificationCode3Track1TestTlv testData = new();
        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(PositionOfCardVerificationCode3Track1.Tag, testData.EncodeValue());
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
        PositionOfCardVerificationCode3Track1TestTlv testData = new();
        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());

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
    public void InvalidBerEncoding_DeserializingDataElement_Throws()
    {
        PositionOfCardVerificationCode3Track1TestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        PositionOfCardVerificationCode3Track1TestTlv testData = new();
        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
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
        PositionOfCardVerificationCode3Track1TestTlv testData = new();
        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
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
        PositionOfCardVerificationCode3Track1TestTlv testData = new(new byte[] { 0x32, 0x8e, 0x12, 0x7f, 0x18, 0x8b });
        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
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
        PositionOfCardVerificationCode3Track1TestTlv testData = new(new byte[]
        {
            0x08, 0x13, 0x9c, 0x0A, 0x16, 0xc3
        });

        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void DataElement_GetSetBitCount_ReturnsExpectedResult()
    {
        PositionOfCardVerificationCode3Track1TestTlv testData = new();
        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
        int expected = 24;
        int actual = sut.GetSetBitCount();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CustomDataElement_GetSetBitCount_ReturnsExpectedResult()
    {
        PositionOfCardVerificationCode3Track1TestTlv testData = new(new byte[]
        {
            0b1010_1010,
            0b1010_1010,
            0b1010_1010,
            0b1010_1010,
            0b1010_1010,
            0b1010_1010,
        });
        PositionOfCardVerificationCode3Track1 sut = PositionOfCardVerificationCode3Track1.Decode(testData.EncodeValue().AsSpan());
        int expected = 24;
        int actual = sut.GetSetBitCount();

        Assert.Equal(expected, actual);
    }

    #endregion
}
