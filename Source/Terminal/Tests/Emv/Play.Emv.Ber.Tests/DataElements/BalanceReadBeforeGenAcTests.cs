using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class BalanceReadBeforeGenAcTests : TestBase
{
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        BalanceReadBeforeGenAcTestTlv testData = new();
        BalanceReadBeforeGenAc testValue = BalanceReadBeforeGenAc.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        BalanceReadBeforeGenAcTestTlv testData = new();
        BalanceReadBeforeGenAc sut = BalanceReadBeforeGenAc.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        BalanceReadBeforeGenAcTestTlv testData = new();
        BalanceReadBeforeGenAc sut = BalanceReadBeforeGenAc.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        BalanceReadBeforeGenAcTestTlv testData = new();
        BalanceReadBeforeGenAc sut = BalanceReadBeforeGenAc.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(BalanceReadBeforeGenAc.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        BalanceReadBeforeGenAcTestTlv testData = new();
        BalanceReadBeforeGenAc sut = BalanceReadBeforeGenAc.Decode(testData.EncodeValue().AsSpan());
        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }
}
