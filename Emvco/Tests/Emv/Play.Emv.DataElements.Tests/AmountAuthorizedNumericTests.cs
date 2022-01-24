using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.TestData.Ber.Primitive;

using Xunit;

namespace Play.Emv.DataElements.Tests;

public class AmountAuthorizedNumericTests
{
    #region Instance Values

    private readonly BerCodec _BerCodec = new(EmvCodec.Configuration);

    #endregion

    #region Instance Members

    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric testValue = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric sut = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric sut = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric sut = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(AmountAuthorizedNumeric.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    // TODO: WARNING - this test is failing indeterminately. There is a bug here
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        AmountAuthorizedNumeric sut = AmountAuthorizedNumeric.Decode(testData.EncodeValue().AsSpan());
        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(testValue, expectedResult);
    }

    #endregion
}