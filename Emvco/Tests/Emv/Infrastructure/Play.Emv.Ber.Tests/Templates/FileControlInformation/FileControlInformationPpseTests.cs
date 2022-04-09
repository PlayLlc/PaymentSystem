using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Infrastructure.Ber.Constructed;
using Play.Testing.Emv.Infrastructure.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.FileControlInformation;

public class FileControlInformationPpseTests
{
    #region Instance Members

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CreatesConstructedValue
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
    {
        FileControlInformationPpseTestTlv testData = new();

        DedicatedFileNameTestTlv? firstChild = new();
        FileControlInformationProprietaryPpseTestTlv? secondChild = new();

        byte[]? testValue = testData.EncodeTagLengthValue();

        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    /// <summary>
    ///     BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements()
    {
        FileControlInformationPpseTestTlv testData = new();
        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());
        byte[] testValue = sut.EncodeTagLengthValue();

        byte[]? expectedResult = testData.EncodeTagLengthValue();

        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationPpseTestTlv testData = new();
        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());

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
        FileControlInformationPpseTestTlv testData = new();
        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());

        Assert.True(sut.GetValueByteCount() == testData.EncodeValue().Length);
    }

    /// <summary>
    ///     Template_InvokingAsTagLengthValue_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        FileControlInformationPpseTestTlv testData = new();
        FileControlInformationPpse sut = FileControlInformationPpse.Decode(testData.EncodeTagLengthValue());

        TagLengthValue testValue = sut.AsTagLengthValue();

        //TagLengthValue expectedResult = testData.AsPrimitiveValue();
        //Assert.Equal(expectedResult, testValue);
    }

    #endregion
}