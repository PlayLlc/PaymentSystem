using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.TestData.Ber.Constructed;

using Xunit;

namespace Play.Emv.Templates.Tests.FileControlInformation;

public class DirectoryEntryTests
{
    #region Instance Members

    /// <summary>
    ///     BerEncoding_DeserializingTemplate_CreatesConstructedValue
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
    {
        DirectoryEntryTestTlv testData = new();
        DirectoryEntry sut = DirectoryEntry.Decode(testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    /// <summary>
    ///     BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDTemplate_CorrectlyCreatesChildDataElements()
    {
        DirectoryEntryTestTlv testData = new();
        DirectoryEntry sut = DirectoryEntry.Decode(testData.EncodeTagLengthValue());
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
        DirectoryEntryTestTlv testData = new();
        DirectoryEntry sut = DirectoryEntry.Decode(testData.EncodeTagLengthValue());
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
        DirectoryEntryTestTlv testData = new();
        DirectoryEntry sut = DirectoryEntry.Decode(testData.EncodeTagLengthValue());
        Assert.True(sut.GetValueByteCount() == testData.EncodeValue().Length);
    }

    /// <summary>
    ///     Template_InvokingAsTagLengthValue_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void Template_InvokingAsTagLengthValue_ReturnsExpectedResult()
    {
        DirectoryEntryTestTlv testData = new();
        DirectoryEntry sut = DirectoryEntry.Decode(testData.EncodeTagLengthValue());
        TagLengthValue testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = testData.AsTagLengthValue();
        Assert.Equal(expectedResult, testValue);
    }

    #endregion
}