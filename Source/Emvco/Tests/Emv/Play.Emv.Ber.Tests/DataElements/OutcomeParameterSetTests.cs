using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class OutcomeParameterSetTests
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
        OutcomeParameterSetTestTlv testData = new();
        OutcomeParameterSet testValue = OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan());
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
        OutcomeParameterSetTestTlv testData = new();
        OutcomeParameterSet sut = OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        OutcomeParameterSetTestTlv testData = new();
        OutcomeParameterSet sut = OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan());
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
        OutcomeParameterSetTestTlv testData = new();
        OutcomeParameterSet sut = OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(OutcomeParameterSet.Tag, testData.EncodeValue());
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
        OutcomeParameterSetTestTlv testData = new();
        OutcomeParameterSet sut = OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan());

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
        OutcomeParameterSetTestTlv testData = new(new byte[] {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01});

        Assert.Throws<DataElementParsingException>(() => OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        OutcomeParameterSetTestTlv testData = new();
        OutcomeParameterSet sut = OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan());
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
        OutcomeParameterSetTestTlv testData = new();
        OutcomeParameterSet sut = OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan());
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
        OutcomeParameterSetTestTlv testData = new(new byte[] {0x08, 0x12, 0x3E, 0x4C, 0x5A, 0x79, 0x34, 0x2D});
        OutcomeParameterSet sut = OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan());
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
        OutcomeParameterSetTestTlv testData = new(new byte[] {0x08, 0x12, 0x3E, 0x4C, 0x5A, 0x79, 0x34, 0x2D});

        OutcomeParameterSet sut = OutcomeParameterSet.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region OutcomeParameterSet

    [Fact]
    public void OutcomeParameterSet_IsDataRecordPresent_ReturnsTrue()
    {
        OutcomeParameterSet sut = new(0b0110_0110_0110_0110_0110_0110_0110_0110_0110_0110);

        Assert.True(sut.IsDataRecordPresent());
    }

    [Fact]
    public void OutcomeParameterSet_IsDataRecordPresent_ReturnsFalse()
    {
        OutcomeParameterSet sut = new(0b1000_0000_0000_0110_0110_0110_0110_0110_0110_0110);

        Assert.False(sut.IsDataRecordPresent());
    }

    [Fact]
    public void OutcomeParameterSet_IsDiscretionaryDataPresent_ReturnsTrue()
    {
        OutcomeParameterSet sut = new(0b0111_0110_0110_0110_0110_0110_0110_0110_0110_0110);

        Assert.True(sut.IsDiscretionaryDataPresent());
    }

    [Fact]
    public void OutcomeParameterSet_IsDiscretionaryDataPresent_ReturnsFalse()
    {
        OutcomeParameterSet sut = new(0b1000_0000_0000_0110_0110_0110_0110_0110_0110_0110);

        Assert.False(sut.IsDiscretionaryDataPresent());
    }

    [Fact]
    public void OutcomeParameterSet_IsReceiptPresent_ReturnsTrue()
    {
        OutcomeParameterSet sut = new(0b0111_1110_0110_0110_0110_0110_0110_0110_0110_0110);

        Assert.True(sut.IsReceiptPresent());
    }

    [Fact]
    public void OutcomeParameterSet_IsReceiptPresent_ReturnsFalse()
    {
        OutcomeParameterSet sut = new(0b1000_0000_0000_0110_0110_0110_0110_0110_0110_0110);

        Assert.False(sut.IsReceiptPresent());
    }

    [Fact]
    public void OutcomeParameterSet_IsUiRequestOnOutcomePresent_ReturnsTrue()
    {
        OutcomeParameterSet sut = new(0b1111_1110_0110_0110_0110_0110_0110_0110_0110_0110);

        Assert.True(sut.IsUiRequestOnOutcomePresent());
    }

    [Fact]
    public void OutcomeParameterSet_IsUiRequestOnOutcomePresent_ReturnsFalse()
    {
        OutcomeParameterSet sut = new(0b0110_0000_0000_0110_0110_0110_0110_0110_0110_0110);

        Assert.False(sut.IsUiRequestOnOutcomePresent());
    }

    [Fact]
    public void OutcomeParameterSet_IsUiRequestOnRestartPresent_ReturnsTrue()
    {
        OutcomeParameterSet sut = new(0b1111_1110_0110_0110_0110_0110_0110_0110_0110_0110);

        Assert.True(sut.IsUiRequestOnRestartPresent());
    }

    [Fact]
    public void OutcomeParameterSet_IsUiRequestOnRestartPresent_ReturnsFalse()
    {
        OutcomeParameterSet sut = new(0b1010_0000_0000_0110_0110_0110_0110_0110_0110_0110);

        Assert.False(sut.IsUiRequestOnRestartPresent());
    }

    [Fact]
    public void OutcomeParameterSet_GetCvmPerformed_ReturnsExpectedResult()
    {
        OutcomeParameterSet sut = new(0b0010_0000_0000_0110_0110_0110_0110_0110_0110_0110);

        Assert.Equal(CvmPerformedOutcome.OnlinePin, sut.GetCvmPerformed());
    }

    [Fact]
    public void OutcomeParameterSet_GetFieldOffRequestOutcome_ReturnsExpectedResult()
    {
        OutcomeParameterSet sut = new(0b0010_0000_0000_0110_0110_0110_0110_0110_0110_0110);
        FieldOffRequestOutcome expected = new(0b0110_0110);

        Assert.Equal(expected, sut.GetFieldOffRequestOutcome());
    }

    [Fact]
    public void OutcomeParameterSet_GetOnlineResponseOutcome_ReturnsExpectedResult()
    {
        OutcomeParameterSet sut = new(0b0010_0000_0000_0110_0110_0110_0110_0110_0110_0110);

        Assert.Equal(OnlineResponseOutcome.NotAvailable, sut.GetOnlineResponseOutcome());
    }

    [Fact]
    public void OutcomeParameterSet_GetStartOutcome_ReturnsExpectedResult()
    {
        OutcomeParameterSet sut = new(0b0010_0000_0010_0000_0010_0000_0000_0110_0110_0110_0110_0110_0110_0110);

        Assert.Equal(StartOutcomes.C, sut.GetStartOutcome());
    }

    [Fact]
    public void OutcomeParameterSet_GetStatusOutcome_ReturnsExpectedResult()
    {
        OutcomeParameterSet sut = new(0b11_0000_0010_0000_0010_0000_0010_0000_0000_0110_0110_0110_0110_0110_0110_0110);

        Assert.Equal(StatusOutcomes.OnlineRequest, sut.GetStatusOutcome());
    }

    #endregion
}