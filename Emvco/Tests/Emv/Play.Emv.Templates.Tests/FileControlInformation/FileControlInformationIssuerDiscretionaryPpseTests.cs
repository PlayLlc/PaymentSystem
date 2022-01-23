using Play.Ber.DataObjects;
using Play.Emv.Templates.FileControlInformation;
using Play.Emv.TestData.Ber.Constructed;

using Xunit;

namespace Play.Emv.Templates.Tests.FileControlInformation;

public class FileControlInformationIssuerDiscretionaryPpseTests
{
    #region Instance Members

    [Fact]
    public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
    {
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse testValue =
            FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(testValue);
    }

    [Fact]
    public void BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements()
    {
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut =
            FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());

        sut.GetTagLengthValueByteCount();

        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[] testValue = sut.EncodeTagLengthValue();

        Assert.Equal(expectedResult, testValue);
    }

    [Fact]
    public void Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut =
            FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());

        Assert.True(sut.GetTagLengthValueByteCount() == testData.GetTagLengthValueByteCount());
        Assert.NotNull(sut);
    }

    [Fact]
    public void Template_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut =
            FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());

        Assert.True(sut.GetValueByteCount() == testData.EncodeValue().Length);
    }

    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut =
            FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());

        TagLengthValue testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = testData.AsTagLengthValue();
        Assert.Equal(expectedResult, testValue);
    }

    #endregion
}