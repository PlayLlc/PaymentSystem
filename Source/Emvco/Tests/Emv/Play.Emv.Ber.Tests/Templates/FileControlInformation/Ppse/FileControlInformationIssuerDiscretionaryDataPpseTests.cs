using System;

using AutoFixture;

using Play.Ber.DataObjects;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.FileControlInformation.Ppse;

public class FileControlInformationIssuerDiscretionaryDataPpseTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    #endregion

    #region Constructor

    public FileControlInformationIssuerDiscretionaryDataPpseTests()
    {
        _Fixture = new EmvFixture().Create();
    }

    #endregion

    #region Instance Members

    [Fact]
    public void ConstructedValue_EncodingTagLengthValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationIssuerDiscretionaryDataPpseBuilder.GetDefaultEncodedTagLengthValue();
        FileControlInformationIssuerDiscretionaryDataPpse sut = _Fixture.Create<FileControlInformationIssuerDiscretionaryDataPpse>();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ConstructedValue_EncodingValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationIssuerDiscretionaryDataPpseBuilder.GetDefaultEncodedValue();
        FileControlInformationIssuerDiscretionaryDataPpse sut = _Fixture.Create<FileControlInformationIssuerDiscretionaryDataPpse>();
        byte[] actual = sut.EncodeValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ConstructedValue_DecodingValue_ReturnsExpectedResult()
    {
        FileControlInformationIssuerDiscretionaryDataPpse expected = _Fixture.Create<FileControlInformationIssuerDiscretionaryDataPpse>();
        FileControlInformationIssuerDiscretionaryDataPpse actual =
            FileControlInformationIssuerDiscretionaryDataPpse.Decode(EmvFixture.FileControlInformationIssuerDiscretionaryDataPpseBuilder
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
        FileControlInformationIssuerDiscretionaryDataPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut = FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CorrectlyEncodesChildDataElements
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CorrectlyEncodesTlvWithChildDataElements()
    {
        FileControlInformationIssuerDiscretionaryDataPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut = FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());
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
        FileControlInformationIssuerDiscretionaryDataPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut = FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());
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
        FileControlInformationIssuerDiscretionaryDataPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut = FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());
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
        FileControlInformationIssuerDiscretionaryDataPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut = FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());
        Assert.True(sut.GetValueByteCount() == testData.EncodeValue().Length);
    }

    /// <summary>
    ///     Template_InvokingAsTagLengthValue_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        FileControlInformationIssuerDiscretionaryDataPpseTestTlv testData = new();
        TagLengthValue expected = testData.AsTagLengthValue();
        FileControlInformationIssuerDiscretionaryDataPpse sut = FileControlInformationIssuerDiscretionaryDataPpse.Decode(expected.EncodeTagLengthValue());
        TagLengthValue actual = sut.AsTagLengthValue();

        Assert.Equal(expected, actual);
    }

    #endregion

}
