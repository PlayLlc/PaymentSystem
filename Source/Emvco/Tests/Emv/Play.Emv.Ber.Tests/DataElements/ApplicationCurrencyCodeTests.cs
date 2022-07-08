using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ApplicationCurrencyCodeTests
{
    /// <summary>
    ///     BerEncoding_DeserializingDataElement_CreatesPrimitiveValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        ApplicationCurrencyCodeTestTlv testData = new();
        ApplicationCurrencyCode testValue = ApplicationCurrencyCode.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    [Fact]
    public void BerEncoding_DeserializingDataElementGivesValidationException_ExceptionIsThrown()
    {
        Assert.Throws<DataElementParsingException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] { 12, 14 };
            ApplicationCurrencyCode testValue = ApplicationCurrencyCode.Decode(input);
        });
    }

    [Fact]
    public void BerEncoding_DeserializingDataElementFromInvalidCurrencyCode_ExceptionIsThrown()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            ReadOnlySpan<byte> input = stackalloc byte[] { 7, 12 };
                ApplicationCurrencyCode testValue = ApplicationCurrencyCode.Decode(input);
        });
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        ApplicationCurrencyCodeTestTlv testData = new();
        ApplicationCurrencyCode sut = ApplicationCurrencyCode.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationCurrencyCodeTestTlv testData = new();
        ApplicationCurrencyCode sut = ApplicationCurrencyCode.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationCurrencyCodeTestTlv testData = new();
        ApplicationCurrencyCode sut = ApplicationCurrencyCode.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ApplicationCurrencyCode.Tag, testData.EncodeValue());
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
        ApplicationCurrencyCodeTestTlv testData = new();
        ApplicationCurrencyCode sut = ApplicationCurrencyCode.Decode(testData.EncodeValue().AsSpan());

        byte[]? encoded = sut.EncodeValue();
        TagLengthValue? tlv = sut.AsTagLengthValue();
        byte[]? tlvRaw = tlv.EncodeTagLengthValue();
        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        TagLengthValue? expectedResultTlv = new(testData.GetTag(), testData.EncodeValue());

        Assert.Equal(testValue, expectedResult);
    }

    #region Operators

    [Fact]
    public void ApplicationCurrencyCode_InstantiateNewNumericCurrencyCode_ReturnsExpectedResult()
    {
        ApplicationCurrencyCodeTestTlv testData = new();
        ApplicationCurrencyCode sut = ApplicationCurrencyCode.Decode(testData.EncodeValue().AsSpan());

        NumericCurrencyCode expected = new NumericCurrencyCode(840);
        NumericCurrencyCode actual = (NumericCurrencyCode)(sut);

        Assert.Equal(expected, actual);
    }

    #endregion
}
