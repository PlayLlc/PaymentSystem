using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ApplicationDedicatedFileNameTests : TestBase
{
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
        ApplicationDedicatedFileNameTestTlv testData = new();
        ApplicationDedicatedFileName testValue = ApplicationDedicatedFileName.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
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
        ApplicationDedicatedFileNameTestTlv testData = new();
        ApplicationDedicatedFileName sut = ApplicationDedicatedFileName.Decode(testData.EncodeValue().AsSpan());
        byte[] expected = testData.EncodeValue();
        byte[]? actual = sut.EncodeValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
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
        ApplicationDedicatedFileNameTestTlv testData = new();
        ApplicationDedicatedFileName sut = ApplicationDedicatedFileName.Decode(testData.EncodeValue().AsSpan());
        byte[] expected = testData.EncodeTagLengthValue();
        byte[]? actual = sut.EncodeTagLengthValue();

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
        ApplicationDedicatedFileNameTestTlv testData = new();
        ApplicationDedicatedFileName sut = ApplicationDedicatedFileName.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? actual = sut.AsTagLengthValue();
        TagLengthValue expected = new(ApplicationDedicatedFileName.Tag, testData.EncodeValue());

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     TagLengthValue_SerializingToBer_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        ApplicationDedicatedFileNameTestTlv testData = new();

        ApplicationDedicatedFileName sut = ApplicationDedicatedFileName.Decode(testData.EncodeValue().AsSpan());
        byte[] actual = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expected = testData.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}