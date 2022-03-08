using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.DataElements.Emv;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Emv.DataElements.Tests;

public class CardholderNameTests
{
    #region Instance Values

    private readonly BerCodec _BerCodec = new(EmvCodec.Configuration);

    #endregion

    #region Instance Members

    /// <summary>
    /// BerEncoding_DeserializingDataElement_CreatesPrimitiveValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        CardholderNameTestTlv testData = new();
        CardholderName testValue = CardholderName.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    /// <summary>
    /// BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        CardholderNameTestTlv testData = new();
        CardholderName sut = CardholderName.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    /// BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        CardholderNameTestTlv testData = new();
        CardholderName sut = CardholderName.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    /// BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        CardholderNameTestTlv testData = new();
        CardholderName sut = CardholderName.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(CardholderName.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    /// TagLengthValue_SerializingToBer_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        CardholderNameTestTlv testData = new();

        CardholderName sut = CardholderName.Decode(testData.EncodeValue().AsSpan());
        byte[]? encoded = sut.EncodeValue();
        TagLengthValue? tlv = sut.AsTagLengthValue();
        byte[]? tlvRaw = tlv.EncodeTagLengthValue();
        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        TagLengthValue? expectedResultTlv = new(testData.GetTag(), testData.EncodeValue());

        Assert.Equal(testValue, expectedResult);
    }

    #endregion
}