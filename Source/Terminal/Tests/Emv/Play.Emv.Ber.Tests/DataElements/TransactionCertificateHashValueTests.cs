using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class TransactionCertificateHashValueTests
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
        TransactionCertificateHashValueTestTlv testData = new();
        TransactionCertificateHashValue testValue = TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan());
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
        TransactionCertificateHashValueTestTlv testData = new();
        TransactionCertificateHashValue sut = TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan());
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
        TransactionCertificateHashValueTestTlv testData = new();
        TransactionCertificateHashValue sut = TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan());
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
        TransactionCertificateHashValueTestTlv testData = new();
        TransactionCertificateHashValue sut = TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(TransactionCertificateHashValue.Tag, testData.EncodeValue());
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
        TransactionCertificateHashValueTestTlv testData = new();
        TransactionCertificateHashValue sut = TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan());

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
        TransactionCertificateHashValueTestTlv testData = new();
        TransactionCertificateHashValue sut = TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan());
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
        TransactionCertificateHashValueTestTlv testData = new();
        TransactionCertificateHashValue sut = TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan());
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
        TransactionCertificateHashValueTestTlv testData = new(new byte[]
        {
            0x4d, 0x3f, 0x07, 0x12, 0x32, 0x5d, 0xaa, 0x9b, 0x3c, 0xab,
            0xd4, 0xf3, 0x70, 0x21, 0x23, 0xd5, 0xaa, 0xb9, 0xc3, 0xba
        });

        TransactionCertificateHashValue sut = TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan());
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
        TransactionCertificateHashValueTestTlv testData = new(new byte[]
        {
            0x4d, 0x3f, 0x07, 0x12, 0x32, 0x5d, 0xaa, 0x9b, 0x3c, 0xab,
            0xd4, 0xf3, 0x70, 0x21, 0x23, 0xd5, 0xaa, 0xb9, 0xc3, 0xba
        });

        TransactionCertificateHashValue sut = TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void InvalidBerEncoding_EncodingToDataElement_Throws()
    {
        TransactionCertificateHashValueTestTlv testData = new(new byte[]
        {
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf
        });

        Assert.Throws<DataElementParsingException>(() => TransactionCertificateHashValue.Decode(testData.EncodeValue().AsSpan()));
    }

    #endregion
}