using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class MagstripeCvmCapabilityNoCvmRequiredTests
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
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new();
        MagstripeCvmCapabilityNoCvmRequired testValue = MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan());
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
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new();
        MagstripeCvmCapabilityNoCvmRequired sut = MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan());
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
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new();
        MagstripeCvmCapabilityNoCvmRequired sut = MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan());
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
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new();
        MagstripeCvmCapabilityNoCvmRequired sut = MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(MagstripeCvmCapabilityNoCvmRequired.Tag, testData.EncodeValue());
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
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new();
        MagstripeCvmCapabilityNoCvmRequired sut = MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan());

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
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new();
        MagstripeCvmCapabilityNoCvmRequired sut = MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan());
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
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new();
        MagstripeCvmCapabilityNoCvmRequired sut = MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan());
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
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new(new byte[] { 0x3D });
        MagstripeCvmCapabilityNoCvmRequired sut = MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan());
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
        MagstripeCvmCapabilityNoCvmRequiredTestTlv testData = new(new byte[]
        {
            0x89
        });

        MagstripeCvmCapabilityNoCvmRequired sut = MagstripeCvmCapabilityNoCvmRequired.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion
}
