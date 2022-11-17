using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class DataStorageOperatorDataSetInfoTests
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
        DataStorageOperatorDataSetInfoTestTlv testData = new();
        DataStorageOperatorDataSetInfo testValue = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoTestTlv testData = new();
        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoTestTlv testData = new();
        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoTestTlv testData = new();
        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(DataStorageOperatorDataSetInfo.Tag, testData.EncodeValue());
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
        DataStorageOperatorDataSetInfoTestTlv testData = new();
        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());

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
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0x13, 0x28});

        Assert.Throws<DataElementParsingException>(() => DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        DataStorageOperatorDataSetInfoTestTlv testData = new();
        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoTestTlv testData = new();
        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0x7d});
        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0xe});

        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region DataStorageOperatorDataSetInfo

    [Fact]
    public void DataStorageOperatorDataSetInfo_IsVolatile_ReturnsTrue()
    {
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsVolatile());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfo_IsVolatile_ReturnsFalse()
    {
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0b1011_1111});

        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsVolatile());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfo_IsPermanent_ReturnsTrue()
    {
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsPermanent());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfo_IsPermanent_ReturnsFalse()
    {
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0b0111_1111});

        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsPermanent());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfo_IsLowVolatility_ReturnsTrue()
    {
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsLowVolatility());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfo_IsLowVolatility_ReturnsFalse()
    {
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0b1101_1111});

        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsLowVolatility());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfo_IsDeclineOnDataStorageErrorSet_ReturnsTrue()
    {
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsDeclineOnDataStorageErrorSet());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfo_IsDeclineOnDataStorageErrorSet_ReturnsFalse()
    {
        DataStorageOperatorDataSetInfoTestTlv testData = new(new byte[] {0b1101_0111});

        DataStorageOperatorDataSetInfo sut = DataStorageOperatorDataSetInfo.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsDeclineOnDataStorageErrorSet());
    }

    #endregion
}