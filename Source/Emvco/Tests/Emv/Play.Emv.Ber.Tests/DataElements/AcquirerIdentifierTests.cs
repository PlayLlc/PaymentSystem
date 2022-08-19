using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Primitive.Acquirer;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class AcquirerIdentifierTests : TestBase
{
    #region Instance Values

    #region Instance Members

    private readonly BerCodec _BerCodec = new(EmvCodec.Configuration);

    #endregion

    #endregion

    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        AcquirerIdentifierTestTlv testData = new();
        AcquirerIdentifier sut = AcquirerIdentifier.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(sut);
    }

    [Fact]
    public void BerEncoding_DeserializingDataElementConsistingOfOnly3Bytes_ThrowsDataElementParsingException()
    {
        AcquirerIdentifierTestTlv testData = new(new byte[] {11, 12, 3});

        Assert.Throws<DataElementParsingException>(() =>
        {
            AcquirerIdentifier sut = AcquirerIdentifier.Decode(testData.EncodeValue().AsSpan());
        });
    }

    [Fact]
    public void BerEncoding_DeserializeDataElementConsistingOf12Digits_ThrowsDataElementParsingException()
    {
        AcquirerIdentifierTestTlv testData = new(new byte[] {11, 12, 13, 14, 15, 16});

        Assert.Throws<DataElementParsingException>(() =>
        {
            AcquirerIdentifier sut = AcquirerIdentifier.Decode(testData.EncodeValue().AsSpan());
        });
    }

    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        AcquirerIdentifierTestTlv testData = new();
        AcquirerIdentifier sut = AcquirerIdentifier.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        AcquirerIdentifierTestTlv testData = new();
        AcquirerIdentifier sut = AcquirerIdentifier.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        AcquirerIdentifierTestTlv testData = new();
        AcquirerIdentifier sut = AcquirerIdentifier.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(AcquirerIdentifier.Tag, testData.EncodeValue());

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        AcquirerIdentifierTestTlv testData = new();
        AcquirerIdentifier sut = AcquirerIdentifier.Decode(testData.EncodeValue().AsSpan());

        byte[] expected = testData.EncodeTagLengthValue();
        byte[] actual = sut.AsTagLengthValue().EncodeTagLengthValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_GetValueByteCount_ReturnsExpectedResult()
    {
        ulong value = 61105100823;
        AcquirerIdentifier sut = new(value);

        ushort expected = 8;
        ushort actual = sut.GetValueByteCount(_BerCodec);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AcquirerIdentifier_InitializedFromSameContentOctets_IsEqual()
    {
        byte[] contentOctets = new byte[] {6, 11, 5, 10, 8, 23};
        ulong value = 61105100823;

        AcquirerIdentifier sut = new(value);
        AcquirerIdentifier sut2 = AcquirerIdentifier.Decode(contentOctets.AsSpan());

        Assert.Equal(sut, sut2);
    }
}