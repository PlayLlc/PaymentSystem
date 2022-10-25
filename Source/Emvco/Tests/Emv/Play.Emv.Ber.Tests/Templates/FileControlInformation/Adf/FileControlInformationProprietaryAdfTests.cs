using System;

using AutoFixture;

using Play.Ber.DataObjects;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.FileControlInformation.Adf;

public class FileControlInformationProprietaryAdfTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    #endregion

    #region Constructor

    public FileControlInformationProprietaryAdfTests()
    {
        _Fixture = new EmvFixture().Create();
    }

    #endregion

    #region Instance Members

    [Fact]
    public void PrimitiveValue_EncodingTagLengthValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationProprietaryAdfBuilder.GetDefaultEncodedTagLengthValue();
        FileControlInformationProprietaryAdf sut = _Fixture.Create<FileControlInformationProprietaryAdf>();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void PrimitiveValue_EncodingValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationProprietaryAdfBuilder.GetDefaultEncodedValue();
        FileControlInformationProprietaryAdf sut = _Fixture.Create<FileControlInformationProprietaryAdf>();
        byte[] actual = sut.EncodeValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CreatesConstructedValue
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
    {
        FileControlInformationProprietaryAdfTestTlv testData = new();
        FileControlInformationProprietaryAdf sut = FileControlInformationProprietaryAdf.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CorrectlyEncodesChildDataElements
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CorrectlyEncodesTlvWithChildDataElements()
    {
        FileControlInformationProprietaryAdfTestTlv testData = new();
        FileControlInformationProprietaryAdf sut = FileControlInformationProprietaryAdf.Decode(testData.EncodeTagLengthValue());
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
        FileControlInformationProprietaryAdfTestTlv testData = new();
        FileControlInformationProprietaryAdf sut = FileControlInformationProprietaryAdf.Decode(testData.EncodeTagLengthValue());
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
        FileControlInformationProprietaryAdfTestTlv testData = new();
        FileControlInformationProprietaryAdf sut = FileControlInformationProprietaryAdf.Decode(testData.EncodeTagLengthValue());
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
        FileControlInformationProprietaryAdfTestTlv testData = new();
        FileControlInformationProprietaryAdf sut = FileControlInformationProprietaryAdf.Decode(testData.EncodeTagLengthValue());
        Assert.True(sut.GetValueByteCount() == testData.EncodeValue().Length);
    }

    /// <summary>
    ///     Template_InvokingAsTagLengthValue_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        FileControlInformationProprietaryAdfTestTlv testData = new();
        TagLengthValue expected = testData.AsTagLengthValue();
        FileControlInformationProprietaryAdf sut = FileControlInformationProprietaryAdf.Decode(expected.EncodeTagLengthValue());
        TagLengthValue actual = sut.AsTagLengthValue();

        Assert.Equal(expected, actual);
    }

    #endregion
}
