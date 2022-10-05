using Play.Ber.DataObjects;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.FileControlInformation.Adf;

public class FileControlInformationProprietaryAdfTests : TestBase
{
    #region Instance Members

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
    ///     BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements()
    {
        FileControlInformationProprietaryAdfTestTlv testData = new();
        FileControlInformationProprietaryAdf sut = FileControlInformationProprietaryAdf.Decode(testData.EncodeTagLengthValue());
        byte[]? expected = testData.EncodeTagLengthValue();
        byte[] actual = sut.EncodeTagLengthValue();
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
