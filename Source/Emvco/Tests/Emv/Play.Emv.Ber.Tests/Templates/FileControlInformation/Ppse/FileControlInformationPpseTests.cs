using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.FileControlInformation.Ppse;

public class FileControlInformationPpseTests : TestBase
{
    #region Instance Members

    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
    {
        FileControlInformationPpseTestTlv testData = new();

        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements()
    {
        FileControlInformationPpseTestTlv testData = new();
        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());
        byte[]? expected = testData.EncodeTagLengthValue();
        byte[] actual = sut.EncodeTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationPpseTestTlv testData = new();
        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());

        Assert.True(sut.GetTagLengthValueByteCount() == testData.GetTagLengthValueByteCount());
        Assert.NotNull(sut);
    }

    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationPpseTestTlv testData = new();
        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());

        Assert.True(sut.GetValueByteCount() == testData.EncodeValue().Length);
    }

    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        FileControlInformationPpseTestTlv testData = new();
        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());
        TagLengthValue expected = new(FileControlInformationTemplate.Tag, testData.EncodeValue());
        TagLengthValue actual = sut.AsTagLengthValue();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}