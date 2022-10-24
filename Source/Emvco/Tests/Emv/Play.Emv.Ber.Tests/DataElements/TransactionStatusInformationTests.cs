using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;
using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class TransactionStatusInformationTests
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
        TransactionStatusInformationTestTlv testData = new();
        TransactionStatusInformation testValue = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());
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
        TransactionStatusInformationTestTlv testData = new();
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());
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
        TransactionStatusInformationTestTlv testData = new();
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());
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
        TransactionStatusInformationTestTlv testData = new();
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(TransactionStatusInformation.Tag, testData.EncodeValue());
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
        TransactionStatusInformationTestTlv testData = new();
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());

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
        TransactionStatusInformationTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        TransactionStatusInformationTestTlv testData = new();
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());
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
        TransactionStatusInformationTestTlv testData = new();
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());
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
        TransactionStatusInformationTestTlv testData = new(new byte[] { 3, 216 });
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());
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
        TransactionStatusInformationTestTlv testData = new(new byte[] { 255, 255 });

        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region TransactionStatusInformation

    [Fact]
    public void TransactionStatusInformation_CardholderVerificationWasPerformed_ReturnsTrue()
    {
        TransactionStatusInformationTestTlv testData = new(new byte[] { 255, 0b1000000 });
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CardholderVerificationWasPerformed());
    }

    [Fact]
    public void TransactionStatusInformation_CardRiskManagementWasPerformed_ReturnsTrue()
    {
        TransactionStatusInformationTestTlv testData = new(new byte[] { 255, 0b100000 });
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CardRiskManagementWasPerformed());
    }

    [Fact]
    public void TransactionStatusInformation_IssuerAuthenticationWasPerformed_ReturnsTrue()
    {
        TransactionStatusInformationTestTlv testData = new(new byte[] { 255, 0b10000 });
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IssuerAuthenticationWasPerformed());
    }

    [Fact]
    public void TransactionStatusInformation_OfflineDataAuthenticationWasPerformed_ReturnsTrue()
    {
        TransactionStatusInformationTestTlv testData = new(new byte[] { 255, 0b10000000 });
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.OfflineDataAuthenticationWasPerformed());
    }

    [Fact]
    public void TransactionStatusInformation_ScriptProcessingWasPerformed_ReturnsTrue()
    {
        TransactionStatusInformationTestTlv testData = new(new byte[] { 255, 0b100 });
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.ScriptProcessingWasPerformed());
    }

    [Fact]
    public void TransactionStatusInformation_TerminalRiskManagementWasPerformed_ReturnsTrue()
    {
        TransactionStatusInformationTestTlv testData = new(new byte[] { 255, 0b1000 });
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.TerminalRiskManagementWasPerformed());
    }

    [Fact]
    public void TransactionStatusInformation_SetTransactionStatusInformationFlagsNotAvailable_NothingIsSet()
    {
        TransactionStatusInformationTestTlv testData = new(new byte[] { 255, 0b1000 });
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());

        sut.Set(Enums.TransactionStatusInformationFlags.NotAvailable);

        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void TransactionStatusInformation_SetTransactionStatusInformationFlags_FlagIsSet()
    {
        TransactionStatusInformationTestTlv testData = new(new byte[] { 0b0, 0b0 });
        TransactionStatusInformation sut = TransactionStatusInformation.Decode(testData.EncodeValue().AsSpan());

        sut = sut.Set(Enums.TransactionStatusInformationFlags.OfflineDataAuthenticationPerformed);

        byte[] expected = { 08, 0 };

        Assert.Equal(expected, sut.EncodeValue());
    }

    #endregion
}
