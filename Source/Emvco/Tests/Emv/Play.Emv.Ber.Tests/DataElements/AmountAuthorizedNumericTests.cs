using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class AmountAuthorizedNumericTests : TestBase
{
    #region Instance Values

    private readonly BerCodec _BerCodec = new(EmvCodec.Configuration);

    #endregion

    #region Instance Members

    /// <summary>
    ///     BerEncoding_DeserializingDataElement_CreatesPrimitiveValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric testValue = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        Assertion(() => Assert.NotNull(testValue));
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric sut = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assertion(() => Assert.Equal(testValue, expectedResult));
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric sut = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        byte[] expected = testData.EncodeTagLengthValue();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric sut = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue expected = new(AmountAuthorizedNumeric.Tag, testData.EncodeValue());
        TagLengthValue? actual = sut.AsTagLengthValue();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    // TODO: WARNING - this test is failing indeterminately. There is a bug here
    /// <summary>
    ///     TagLengthValue_SerializingToBer_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric sut = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        byte[] expected = testData.EncodeTagLengthValue();
        byte[] actual = sut.AsTagLengthValue().EncodeTagLengthValue();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}