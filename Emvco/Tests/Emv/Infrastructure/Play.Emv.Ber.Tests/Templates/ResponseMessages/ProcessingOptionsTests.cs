using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.ResponseMessages;

public class ProcessingOptionsTests
{
    #region Instance Members

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CreatesConstructedValue
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
    {
        ProcessingOptionsTestTlv testData = new();
        ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    /// <summary>
    ///     BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements()
    {
        ProcessingOptionsTestTlv testData = new();
        ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
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
        ProcessingOptionsTestTlv testData = new();
        ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
        Assert.True(sut.GetTagLengthValueByteCount() == testData.GetTagLengthValueByteCount());
    }

    /// <summary>
    ///     Template_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        ProcessingOptionsTestTlv testData = new();
        ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
        Assert.True(sut.GetValueByteCount() == testData.GetValueByteCount());
    }

    /// <summary>
    ///     Template_InvokingAsTagLengthValue_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        ProcessingOptionsTestTlv testData = new();
        ProcessingOptions sut = ProcessingOptions.Decode(testData.EncodeTagLengthValue());
        TagLengthValue testValue = sut.AsTagLengthValue();

        //TagLengthValue expectedResult = testData.AsPrimitiveValue();
        //Assert.Equal(expectedResult, testValue);
    }

    #endregion
}