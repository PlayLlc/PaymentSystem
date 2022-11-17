using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ApplicationVersionNumberReaderTests
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
        ApplicationVersionNumberReaderTestTlv testData = new();
        ApplicationVersionNumberReader testValue = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationVersionNumberReaderTestTlv testData = new();
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationVersionNumberReaderTestTlv testData = new();
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationVersionNumberReaderTestTlv testData = new();
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ApplicationVersionNumberReader.Tag, testData.EncodeValue());
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
        ApplicationVersionNumberReaderTestTlv testData = new();
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());

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
        ApplicationVersionNumberReaderTestTlv testData = new(new byte[] {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01});

        Assert.Throws<DataElementParsingException>(() => ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        ApplicationVersionNumberReaderTestTlv testData = new();
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationVersionNumberReaderTestTlv testData = new();
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationVersionNumberReaderTestTlv testData = new(new byte[] {0x08, 0x32});
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationVersionNumberReaderTestTlv testData = new(new byte[] {0x79, 0xEF});

        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region ApplicationVersionNumberReader

    [Fact]
    public void ApplicationVersionNumberReader_IsCardholderVerificationIsSupported_ReturnsTrue()
    {
        ApplicationVersionNumberReaderTestTlv testData = new(new byte[] {0b0, 0b0011_0000});
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsCardholderVerificationIsSupported());
    }

    [Fact]
    public void ApplicationVersionNumberReader_IsCdaSupported_ReturnsTrue()
    {
        ApplicationVersionNumberReaderTestTlv testData = new(new byte[] {0b0, 0b0011_0001});
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsCdaSupported());
    }

    [Fact]
    public void ApplicationVersionNumberReader_IsDdaSupported_ReturnsTrue()
    {
        ApplicationVersionNumberReaderTestTlv testData = new(new byte[] {0b0, 0b0011_0001});
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsDdaSupported());
    }

    [Fact]
    public void ApplicationVersionNumberReader_IsIssuerAuthenticationIsSupported19_ReturnsTrue()
    {
        ApplicationVersionNumberReaderTestTlv testData = new(new byte[] {0b0, 0b101});
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsIssuerAuthenticationIsSupported19());
    }

    [Fact]
    public void ApplicationVersionNumberReader_IsSdaSupported_ReturnsTrue()
    {
        ApplicationVersionNumberReaderTestTlv testData = new(new byte[] {0b0, 0b110_0101});
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsSdaSupported());
    }

    [Fact]
    public void ApplicationVersionNumberReader_TerminalRiskManagementIsToBePerformed_ReturnsTrue()
    {
        ApplicationVersionNumberReaderTestTlv testData = new(new byte[] {0b0, 0b1101});
        ApplicationVersionNumberReader sut = ApplicationVersionNumberReader.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.TerminalRiskManagementIsToBePerformed());
    }

    #endregion
}