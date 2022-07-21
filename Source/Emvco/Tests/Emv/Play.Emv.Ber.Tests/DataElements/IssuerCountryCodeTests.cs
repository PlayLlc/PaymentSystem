using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Country;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class IssuerCountryCodeTests
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
        IssuerCountryCodeTestTlv testData = new();
        IssuerCountryCode testValue = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());
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
        IssuerCountryCodeTestTlv testData = new();
        IssuerCountryCode sut = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());
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
        IssuerCountryCodeTestTlv testData = new();
        IssuerCountryCode sut = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());
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
        IssuerCountryCodeTestTlv testData = new();
        IssuerCountryCode sut = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(IssuerCountryCode.Tag, testData.EncodeValue());
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
        IssuerCountryCodeTestTlv testData = new();
        IssuerCountryCode sut = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());

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
        IssuerCountryCodeTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => IssuerCountryCode.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        IssuerCountryCodeTestTlv testData = new();
        IssuerCountryCode sut = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());
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
        IssuerCountryCodeTestTlv testData = new();
        IssuerCountryCode sut = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());
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
        IssuerCountryCodeTestTlv testData = new(new byte[] { 7, 23 });
        IssuerCountryCode sut = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());
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
        IssuerCountryCodeTestTlv testData = new(new byte[]
        {
            6, 77
        });

        IssuerCountryCode sut = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    [Fact]
    public void IssuerContryCode_InvokeCountryCodeOperator_ReturnsExpectedResult()
    {
        IssuerCountryCodeTestTlv testData = new();
        IssuerCountryCode sut = IssuerCountryCode.Decode(testData.EncodeValue().AsSpan());

        NumericCountryCode expected = new NumericCountryCode(840);

        Assert.Equal(expected, (NumericCountryCode)sut);
    }
}
