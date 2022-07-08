using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ApplicationEffectiveDateTests
{
    /// <summary>
    ///     BerEncoding_DeserializingDataElement_CreatesPrimitiveValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        ApplicationEffectiveDateTestTlv testData = new();
        ApplicationEffectiveDate testValue = ApplicationEffectiveDate.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    [Fact]
    public void BerEncoding_DeserializingDataElementInvalidValue_ThrowsException()
    {
        Assert.Throws<DataElementParsingException>(() =>
        {
            ReadOnlySpan<byte> value = stackalloc byte[] { 20, 11, 26, 13 };
            ApplicationEffectiveDate testValue = ApplicationEffectiveDate.Decode(value);
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
        ApplicationEffectiveDateTestTlv testData = new();
        ApplicationEffectiveDate sut = ApplicationEffectiveDate.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationEffectiveDateTestTlv testData = new();
        ApplicationEffectiveDate sut = ApplicationEffectiveDate.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationEffectiveDateTestTlv testData = new();
        ApplicationEffectiveDate sut = ApplicationEffectiveDate.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ApplicationEffectiveDate.Tag, testData.EncodeValue());
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
        ApplicationEffectiveDateTestTlv testData = new();

        ApplicationEffectiveDate sut = ApplicationEffectiveDate.Decode(testData.EncodeValue().AsSpan());
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
    public void ApplicationEffectiveDate_CastToUint_OperatorReturnsExpectedResult()
    {
        ApplicationEffectiveDateTestTlv testData = new();

        ApplicationEffectiveDate sut = ApplicationEffectiveDate.Decode(testData.EncodeValue().AsSpan());

        uint expected = 220707;
        Assert.Equal(expected, (uint)sut);
    }

    #endregion
}
