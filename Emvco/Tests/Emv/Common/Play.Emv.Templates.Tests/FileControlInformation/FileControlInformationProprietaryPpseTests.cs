using Play.Ber.DataObjects;
using Play.Emv.Templates.FileControlInformation;
using Play.Emv.TestData.Ber.Constructed;

using Xunit;

namespace Play.Emv.Templates.Tests.FileControlInformation;

public class FileControlInformationProprietaryPpseTests
{
    #region Instance Members

    [Fact]
    public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
    {
        FileControlInformationProprietaryPpseTestTlv testData = new();
        FileControlInformationProprietaryPpse sut = FileControlInformationProprietaryPpse.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    [Fact]
    public void BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements()
    {
        FileControlInformationProprietaryPpseTestTlv testData = new();
        FileControlInformationProprietaryPpse sut = FileControlInformationProprietaryPpse.Decode(testData.EncodeTagLengthValue());
        byte[] testValue = sut.EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();

        Assert.Equal(expectedResult, testValue);
    }

    [Fact]
    public void Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationProprietaryPpseTestTlv testData = new();
        FileControlInformationProprietaryPpse sut = FileControlInformationProprietaryPpse.Decode(testData.EncodeTagLengthValue());

        Assert.True(sut.GetTagLengthValueByteCount() == testData.GetTagLengthValueByteCount());
        Assert.NotNull(sut);
    }

    [Fact]
    public void Template_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationProprietaryPpseTestTlv testData = new();
        FileControlInformationProprietaryPpse sut = FileControlInformationProprietaryPpse.Decode(testData.EncodeTagLengthValue());

        Assert.True(sut.GetValueByteCount() == testData.EncodeValue().Length);
    }

    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        ConstructedTlv testData = new FileControlInformationProprietaryPpseTestTlv();
        FileControlInformationProprietaryPpse sut = FileControlInformationProprietaryPpse.Decode(testData.EncodeTagLengthValue());

        TagLengthValue testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = testData.AsTagLengthValue();
        Assert.Equal(expectedResult, testValue);
    }

    #endregion
}