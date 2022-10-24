using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ReferenceControlParameterTests
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
        ReferenceControlParameterTestTlv testData = new();
        ReferenceControlParameter testValue = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());
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
        ReferenceControlParameterTestTlv testData = new();
        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());
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
        ReferenceControlParameterTestTlv testData = new();
        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());
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
        ReferenceControlParameterTestTlv testData = new();
        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ReferenceControlParameter.Tag, testData.EncodeValue());
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
        ReferenceControlParameterTestTlv testData = new();
        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());

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
        ReferenceControlParameterTestTlv testData = new();
        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());
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
        ReferenceControlParameterTestTlv testData = new();
        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());
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
        ReferenceControlParameterTestTlv testData = new(new byte[] { 0x8f });
        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());
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
        ReferenceControlParameterTestTlv testData = new(new byte[]
        {
            0x4d
        });

        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void InvalidBerEncoding_EncodingDataElement_ThrowsException()
    {
        ReferenceControlParameterTestTlv testData = new(new byte[]
        {
            0x4d, 0x7A
        });

        Assert.Throws<DataElementParsingException>(() => ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan()));
    }

    #endregion

    #region ReferenceControlParameter

    [Fact]
    public void ReferenceControlParameter_IsCdaSignatureRequested_ReturnsTrue()
    {
        ReferenceControlParameterTestTlv testData = new(new byte[]
        {
            0b1001_1001
        });

        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsCdaSignatureRequested());
    }

    [Fact]
    public void ReferenceControlParameter_IsCdaSignatureRequested_ReturnsFalse()
    {
        ReferenceControlParameterTestTlv testData = new(new byte[]
        {
            0b1010_1001
        });

        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsCdaSignatureRequested());
    }

    [Fact]
    public void ReferenceControlParameter_GetCryptogramType_ReturnsExpectedResult()
    {
        ReferenceControlParameterTestTlv testData = new(new byte[]
        {
            0x80
        });

        ReferenceControlParameter sut = ReferenceControlParameter.Decode(testData.EncodeValue().AsSpan());

        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, sut.GetCryptogramType());
    }

    #endregion
}
