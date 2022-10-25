using System;

using AutoFixture;

using Play.Ber.DataObjects;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.FileControlInformation.Ddf;

public class FileControlInformationProprietaryDdfTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    #endregion

    #region Constructor

    public FileControlInformationProprietaryDdfTests()
    {
        _Fixture = new EmvFixture().Create();
    }

    #endregion

    #region Instance Members

    [Fact]
    public void PrimitiveValue_EncodingTagLengthValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationProprietaryDdfBuilder.GetDefaultEncodedTagLengthValue();
        FileControlInformationProprietaryDdf sut = _Fixture.Create<FileControlInformationProprietaryDdf>();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_EncodingValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationProprietaryDdfBuilder.GetDefaultEncodedValue();
        FileControlInformationProprietaryDdf sut = _Fixture.Create<FileControlInformationProprietaryDdf>();
        byte[] actual = sut.EncodeValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_DecodingValue_ReturnsExpectedResult()
    {
        FileControlInformationProprietaryDdf expected = _Fixture.Create<FileControlInformationProprietaryDdf>();
        FileControlInformationProprietaryDdf actual =
            FileControlInformationProprietaryDdf.Decode(EmvFixture.FileControlInformationProprietaryDdfBuilder
                .GetDefaultEncodedTagLengthValue().AsMemory());

        Assertion(() => Assert.Equal(expected, actual));
    }

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CreatesConstructedValue
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
    {
        FileControlInformationProprietaryDdfTestTlv testData = new();
        FileControlInformationProprietaryDdf sut = FileControlInformationProprietaryDdf.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CorrectlyEncodesChildDataElements
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CorrectlyEncodesTlvWithChildDataElements()
    {
        FileControlInformationProprietaryDdfTestTlv testData = new();
        FileControlInformationProprietaryDdf sut = FileControlInformationProprietaryDdf.Decode(testData.EncodeTagLengthValue());
        byte[]? expected = testData.EncodeTagLengthValue();
        byte[] actual = sut.EncodeTagLengthValue();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CorrectlyCreatesChildDataElements2
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CorrectlyEncodesChildDataElements()
    {
        FileControlInformationProprietaryDdfTestTlv testData = new();
        FileControlInformationProprietaryDdf sut = FileControlInformationProprietaryDdf.Decode(testData.EncodeTagLengthValue());
        byte[]? expected = testData.EncodeValue();
        byte[] actual = sut.EncodeValue();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationProprietaryDdfTestTlv testData = new();
        FileControlInformationProprietaryDdf sut = FileControlInformationProprietaryDdf.Decode(testData.EncodeTagLengthValue());
        Assert.True(sut.GetTagLengthValueByteCount() == testData.GetTagLengthValueByteCount());
        Assert.NotNull(sut);
    }

    /// <summary>
    ///     Template_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationProprietaryDdfTestTlv testData = new();
        FileControlInformationProprietaryDdf sut = FileControlInformationProprietaryDdf.Decode(testData.EncodeTagLengthValue());
        Assert.True(sut.GetValueByteCount() == testData.EncodeValue().Length);
    }

    /// <summary>
    ///     Template_InvokingAsTagLengthValue_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        FileControlInformationProprietaryDdfTestTlv testData = new();
        TagLengthValue expected = testData.AsTagLengthValue();
        FileControlInformationProprietaryDdf sut = FileControlInformationProprietaryDdf.Decode(expected.EncodeTagLengthValue());
        TagLengthValue actual = sut.AsTagLengthValue();

        Assert.Equal(expected, actual);
    }

    #endregion
}
