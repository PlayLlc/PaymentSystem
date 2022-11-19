using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class TerminalVerificationResultsTests
{
    #region Instance Members

    /// <summary>
    ///     BerEncoding_DeserializingDataElement_CreatesPrimitiveValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        TerminalVerificationResultsTestTlv testData = new();
        TerminalVerificationResults testValue = TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        TerminalVerificationResultsTestTlv testData = new();
        TerminalVerificationResults sut = TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        TerminalVerificationResultsTestTlv testData = new();
        TerminalVerificationResults sut = TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        TerminalVerificationResultsTestTlv testData = new();
        TerminalVerificationResults sut = TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(TerminalVerificationResults.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     TagLengthValue_SerializingToBer_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        TerminalVerificationResultsTestTlv testData = new();
        TerminalVerificationResults sut = TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan());

        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     InvalidBerEncoding_DeserializingDataElement_Throws
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void InvalidBerEncoding_DeserializingDataElement_Throws()
    {
        TerminalVerificationResultsTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        TerminalVerificationResultsTestTlv testData = new();
        TerminalVerificationResults sut = TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        ushort testResult = sut.GetValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    /// <summary>
    ///     DataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        TerminalVerificationResultsTestTlv testData = new();
        TerminalVerificationResults sut = TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    /// <summary>
    ///     CustomDataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void CustomDataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        TerminalVerificationResultsTestTlv testData = new(new byte[] { 0x08, 0x8c, 0xef, 0x13, 0x25 });
        TerminalVerificationResults sut = TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetValueByteCount();
        ushort testResult = sut.GetValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    /// <summary>
    ///     CustomDataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void CustomDataElement_InvokingGetTagLengthValueByteCount_ReturnsExpectedResult()
    {
        TerminalVerificationResultsTestTlv testData = new(new byte[] { 0x08, 0x8c, 0xef, 0x13, 0x25 });

        TerminalVerificationResults sut = TerminalVerificationResults.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region TerminalVerificationResults

    [Fact]
    public void TerminalVerificationResults_ApplicationNotYetEffective_ReturnsTrue()
    {
        TerminalVerificationResults sut = new TerminalVerificationResults(0b100000000000000000000000000000);

        Assert.True(sut.ApplicationNotYetEffective());
    }

    [Fact]
    public void TerminalVerificationResults_CardAppearsOnTerminalExceptionFile_ReturnsTrue()
    {
        TerminalVerificationResults sut = new TerminalVerificationResults(0b1000000000000000000000000000000000000);

        Assert.True(sut.CardAppearsOnTerminalExceptionFile());
    }

    [Fact]
    public void TerminalVerificationResults_CardholderVerificationWasNotSuccessful_ReturnsTrue()
    {
        TerminalVerificationResults sut = new TerminalVerificationResults(0b100000000000000000000000);

        Assert.True(sut.CardholderVerificationWasNotSuccessful());
    }

    [Fact]
    public void TerminalVerificationResults_CombinationDataAuthenticationFailed_ReturnsTrue()
    {
        TerminalVerificationResults sut = new TerminalVerificationResults(0b10000000000000000000000000000000000);

        Assert.True(sut.CombinationDataAuthenticationFailed());
    }

    [Fact]
    public void TerminalVerificationResults_DefaultTransactionCertificateDataObjectListUsed_ReturnsTrue()
    {
        TerminalVerificationResults sut = new TerminalVerificationResults(0b1000000000000000000000000000000000000000);

        Assert.True(sut.DefaultTransactionCertificateDataObjectListUsed());
    }

    [Fact]
    public void TerminalVerificationResults_DynamicDataAuthenticationFailed_ReturnsTrue()
    {
        TerminalVerificationResults sut = new TerminalVerificationResults(0b100000000000000000000000000000000000);

        Assert.True(sut.DynamicDataAuthenticationFailed());
    }

    #endregion


    #region Builder

    [Fact]
    public void TerminalVerificationResultsBuilder_Instantiate_BuilderInstantiated()
    {
        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();

        Assert.NotNull(builder);
    }

    [Fact]
    public void TerminalVerificationResultsBuilder_Reset_ReturnsExpectedResult()
    {
        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();

        TerminalVerificationResultsTestTlv testValue = new();
        TerminalVerificationResults expected = TerminalVerificationResults.Decode(testValue.EncodeValue().AsSpan());

        builder.Reset(expected);
        TerminalVerificationResults actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TerminalVerificationResultsBuilder_SetTerminalVerificationResult_ReturnsExpectedResult()
    {
        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();

        TerminalVerificationResults expected = new TerminalVerificationResults(223);

        TerminalVerificationResult tvr = new TerminalVerificationResult(223);

        builder.Set(tvr);
        TerminalVerificationResults actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TerminalVerificationResultsBuilder_ClearTerminalVerificationResult_ReturnsExpectedResult()
    {
        TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();

        TerminalVerificationResults expected = new TerminalVerificationResults(220);

        TerminalVerificationResult tvrToSet = new TerminalVerificationResult(223);
        TerminalVerificationResult tvrToClear = new TerminalVerificationResult(3);

        builder.Set(tvrToSet);
        builder.Clear(tvrToClear);

        TerminalVerificationResults actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    #endregion
}
