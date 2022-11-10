﻿using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates;

public class FileControlInformationIssuerDiscretionaryPpseTests
{
    #region Instance Members

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CreatesConstructedValue
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
    {
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse testValue = FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements()
    {
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut = FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());

        sut.GetTagLengthValueByteCount();

        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[] testValue = sut.EncodeTagLengthValue();

        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
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
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
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
        FileControlInformationIssuerDiscretionaryPpseTestTlv testData = new();
        FileControlInformationIssuerDiscretionaryDataPpse sut = FileControlInformationIssuerDiscretionaryDataPpse.Decode(testData.EncodeTagLengthValue());

        TagLengthValue testValue = sut.AsTagLengthValue();

        //TagLengthValue expectedResult = testData.AsPrimitiveValue();
        //Assert.Equal(expectedResult, testValue);
    }

    #endregion
}