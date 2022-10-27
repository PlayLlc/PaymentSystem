using System;

using AutoFixture;

using Play.Ber.DataObjects;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.FileControlInformation.Ddf;

public class FileControlInformationDdfTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    #endregion

    #region Constructor

    public FileControlInformationDdfTests()
    {
        _Fixture = new EmvFixture().Create();
    }

    #endregion

    #region Instance Members

    [Fact]
    public void ConstructedValue_EncodingTagLengthValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationDdfBuilder.GetDefaultEncodedTagLengthValue();
        FileControlInformationDdf sut = _Fixture.Create<FileControlInformationDdf>();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ConstructedValue_EncodingValue_ReturnsExpectedResult()
    {
        byte[] expected = EmvFixture.FileControlInformationDdfBuilder.GetDefaultEncodedValue();
        FileControlInformationDdf sut = _Fixture.Create<FileControlInformationDdf>();
        byte[] actual = sut.EncodeValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ConstructedValue_DecodingValue_ReturnsExpectedResult()
    {
        FileControlInformationDdf expected = _Fixture.Create<FileControlInformationDdf>();
        FileControlInformationDdf actual =
            FileControlInformationDdf.Decode(EmvFixture.FileControlInformationDdfBuilder
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
        FileControlInformationDdfTestTlv testData = new();
        FileControlInformationDdf sut = FileControlInformationDdf.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CorrectlyEncodesChildDataElements
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CorrectlyEncodesTlvWithChildDataElements()
    {
        FileControlInformationDdfTestTlv testData = new();
        FileControlInformationDdf sut = FileControlInformationDdf.Decode(testData.EncodeTagLengthValue());
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
        FileControlInformationDdfTestTlv testData = new();
        FileControlInformationDdf sut = FileControlInformationDdf.Decode(testData.EncodeTagLengthValue());
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
        FileControlInformationDdfTestTlv testData = new();
        FileControlInformationDdf sut = FileControlInformationDdf.Decode(testData.EncodeTagLengthValue());
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
        FileControlInformationDdfTestTlv testData = new();
        FileControlInformationDdf sut = FileControlInformationDdf.Decode(testData.EncodeTagLengthValue());
        Assert.True(sut.GetValueByteCount() == testData.EncodeValue().Length);
    }

    /// <summary>
    ///     Template_InvokingAsTagLengthValue_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        FileControlInformationDdfTestTlv testData = new();
        TagLengthValue expected = testData.AsTagLengthValue();
        FileControlInformationDdf sut = FileControlInformationDdf.Decode(expected.EncodeTagLengthValue());
        TagLengthValue actual = sut.AsTagLengthValue();

        Assert.Equal(expected, actual);
    }

    #endregion
}
