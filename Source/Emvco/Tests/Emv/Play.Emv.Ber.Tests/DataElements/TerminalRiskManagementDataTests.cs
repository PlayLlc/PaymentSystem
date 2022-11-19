using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class TerminalRiskManagementDataTests
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
        TerminalRiskManagementDataTestTlv testData = new();
        TerminalRiskManagementData testValue = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());
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
        TerminalRiskManagementDataTestTlv testData = new();
        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());
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
        TerminalRiskManagementDataTestTlv testData = new();
        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());
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
        TerminalRiskManagementDataTestTlv testData = new();
        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(TerminalRiskManagementData.Tag, testData.EncodeValue());
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
        TerminalRiskManagementDataTestTlv testData = new();
        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

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
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01});

        Assert.Throws<DataElementParsingException>(() => TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        TerminalRiskManagementDataTestTlv testData = new();
        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());
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
        TerminalRiskManagementDataTestTlv testData = new();
        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());
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
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x3f, 0x8c, 0x08, 0xEF, 0x3f, 0x8c});
        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());
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
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0x32, 0x1A});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region TerminalRiskManagementData Tests

    #region #region Byte 1

    [Fact]
    public void TerminalRiskManagementData_PresentAndHoldSupported_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0x32, 0b1});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.PresentAndHoldSupported());
    }

    [Fact]
    public void TerminalRiskManagementData_PlaintextPinVerificationPerformedByIccForContactless_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0x32, 0b10});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.PlaintextPinVerificationPerformedByIccForContactless());
    }

    [Fact]
    public void TerminalRiskManagementData_CdCvmForContactless_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0x32, 0b100});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CdCvmForContactless());
    }

    [Fact]
    public void TerminalRiskManagementData_NoCardVerificationMethodRequiredForContactless_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0x32, 0b1000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.NoCardVerificationMethodRequiredForContactless());
    }

    [Fact]
    public void TerminalRiskManagementData_EncipheredPinVerificationPerformedByIccForContactless_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0x32, 0b10000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.EncipheredPinVerificationPerformedByIccForContactless());
    }

    [Fact]
    public void TerminalRiskManagementData_SignaturePaperForContactless_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0x32, 0b100000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.SignaturePaperForContactless());
    }

    [Fact]
    public void TerminalRiskManagementData_EncipheredPinVerifiedOnlineForContactless_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0x32, 0b1000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.EncipheredPinVerifiedOnlineForContactless());
    }

    [Fact]
    public void TerminalRiskManagementData_IsRestartSupported_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0x32, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsRestartSupported());
    }

    #endregion

    #region Byte 2

    [Fact]
    public void TerminalRiskManagementData_PlaintextPinVerificationPerformedByIccForContact_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0b10, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.PlaintextPinVerificationPerformedByIccForContact());
    }

    [Fact]
    public void TerminalRiskManagementData_CdCvmForContact_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0b100, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CdCvmForContact());
    }

    [Fact]
    public void TerminalRiskManagementData_NoCardholderVerificationMethodRequiredForContact_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0b1000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.NoCardholderVerificationMethodRequiredForContact());
    }

    [Fact]
    public void TerminalRiskManagementData_EncipheredPinVerificationPerformedByIccForContact_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0b10000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.EncipheredPinVerificationPerformedByIccForContact());
    }

    [Fact]
    public void TerminalRiskManagementData_SignaturePaperForContact_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0b100000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.SignaturePaperForContact());
    }

    [Fact]
    public void TerminalRiskManagementData_EncipheredPinVerifiedOnlineForContact_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0b1000000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.EncipheredPinVerifiedOnlineForContact());
    }

    [Fact]
    public void TerminalRiskManagementData_CvmLimitExceeded_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0xEF, 0b10000000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CvmLimitExceeded());
    }

    #endregion

    #region Byte 3

    [Fact]
    public void TerminalRiskManagementData_CdCvmWithoutCdaSupported_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0b10_0000, 0b10000000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CdCvmWithoutCdaSupported());
    }

    [Fact]
    public void TerminalRiskManagementData_EmvModeContactlessTransactionsNotSupported_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0b100_0000, 0b10000000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.EmvModeContactlessTransactionsNotSupported());
    }

    [Fact]
    public void TerminalRiskManagementData_MagstripeModeContactlessTransactionsNotSupported_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0x08, 0b1000_0000, 0b10000000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.MagstripeModeContactlessTransactionsNotSupported());
    }

    #endregion

    #region Byte 4

    [Fact]
    public void TerminalRiskManagementData_ScaExempt_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0b100_0000, 0b1000_0000, 0b10000000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.ScaExempt());
    }

    [Fact]
    public void TerminalRiskManagementData_CdCvmBypassRequested_ReturnsTrue()
    {
        TerminalRiskManagementDataTestTlv testData = new(new byte[] {0x08, 0xEF, 0x32, 0x1A, 0b1000_0000, 0b1000_0000, 0b10000000, 0b10000000});

        TerminalRiskManagementData sut = TerminalRiskManagementData.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CdCvmBypassRequested());
    }

    #endregion

    #endregion
}