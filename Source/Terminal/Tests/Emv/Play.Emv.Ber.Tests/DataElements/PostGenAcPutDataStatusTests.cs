using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class PostGenAcPutDataStatusTests
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
        PostGenAcPutDataStatusTestTlv testData = new();
        PostGenAcPutDataStatus testValue = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());
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
        PostGenAcPutDataStatusTestTlv testData = new();
        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());
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
        PostGenAcPutDataStatusTestTlv testData = new();
        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());
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
        PostGenAcPutDataStatusTestTlv testData = new();
        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(PostGenAcPutDataStatus.Tag, testData.EncodeValue());
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
        PostGenAcPutDataStatusTestTlv testData = new();
        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());

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
        PostGenAcPutDataStatusTestTlv testData = new();
        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());
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
        PostGenAcPutDataStatusTestTlv testData = new();
        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());
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
        PostGenAcPutDataStatusTestTlv testData = new(new byte[] {0x8f});
        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());
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
        PostGenAcPutDataStatusTestTlv testData = new(new byte[] {0x4d});

        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void InvalidBerEncoding_EncodingDataElement_ThrowsException()
    {
        PostGenAcPutDataStatusTestTlv testData = new(new byte[] {0x4d, 0x7A});

        Assert.Throws<DataElementParsingException>(() => PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan()));
    }

    #endregion

    #region PostGenAcPutDataStatus

    [Fact]
    public void PostGenAcPutDataStatus_IsCompleted_ReturnsTrue()
    {
        PostGenAcPutDataStatusTestTlv testData = new(new byte[] {0b1000_0000});

        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsCompleted());
    }

    [Fact]
    public void PostGenAcPutDataStatus_IsCompleted_ReturnsFalse()
    {
        PostGenAcPutDataStatusTestTlv testData = new(new byte[] {0b0110_0000});

        PostGenAcPutDataStatus sut = PostGenAcPutDataStatus.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsCompleted());
    }

    #endregion
}