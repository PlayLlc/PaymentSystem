using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class DataStorageOperatorDataSetInfoForReaderTests
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
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new();
        DataStorageOperatorDataSetInfoForReader testValue = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new();
        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new();
        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new();
        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(DataStorageOperatorDataSetInfoForReader.Tag, testData.EncodeValue());
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
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new();
        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

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
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0x13, 0x28});

        Assert.Throws<DataElementParsingException>(() => DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new();
        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new();
        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0x7d});
        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0xe});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region DataStorageOperatorDataSetInfoForReader

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsUsableForTransactionCryptogram_ReturnsTrue()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsUsableForTransactionCryptogram());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsUsableForTransactionCryptogram_ReturnsFalse()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b0111_1111});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsUsableForTransactionCryptogram());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsUsableForAuthorizationRequestCryptogram_ReturnsTrue()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsUsableForAuthorizationRequestCryptogram());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsUsableForAuthorizationRequestCryptogram_ReturnsFalse()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b1011_1111});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsUsableForAuthorizationRequestCryptogram());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsUsableForApplicationCryptogram_ReturnsTrue()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsUsableForApplicationCryptogram());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsUsableForApplicationCryptogram_ReturnsFalse()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b1101_1111});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsUsableForApplicationCryptogram());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsStopIfNoDataStorageOperatorSetTerminalSet_ReturnsTrue()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsStopIfNoDataStorageOperatorSetTerminalSet());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsStopIfNoDataStorageOperatorSetTerminalSet_ReturnsFalse()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b1111_1011});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsStopIfNoDataStorageOperatorSetTerminalSet());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsStopIfWriteFailedSet_ReturnsTrue()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsStopIfWriteFailedSet());
    }

    [Fact]
    public void DataStorageOperatorDataSetInfoForReader_InvokingIsStopIfWriteFailedSet_ReturnsFalse()
    {
        DataStorageOperatorDataSetInfoForReaderTestTlv testData = new(new byte[] {0b1111_1101});

        DataStorageOperatorDataSetInfoForReader sut = DataStorageOperatorDataSetInfoForReader.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsStopIfWriteFailedSet());
    }

    #endregion
}