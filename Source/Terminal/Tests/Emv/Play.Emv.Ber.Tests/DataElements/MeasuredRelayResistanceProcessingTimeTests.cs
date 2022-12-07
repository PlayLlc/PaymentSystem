using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class MeasuredRelayResistanceProcessingTimeTests
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
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new();
        MeasuredRelayResistanceProcessingTime testValue = MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan());
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
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new();
        MeasuredRelayResistanceProcessingTime sut = MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan());
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
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new();
        MeasuredRelayResistanceProcessingTime sut = MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan());
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
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new();
        MeasuredRelayResistanceProcessingTime sut = MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(MeasuredRelayResistanceProcessingTime.Tag, testData.EncodeValue());
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
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new();
        MeasuredRelayResistanceProcessingTime sut = MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan());

        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new();
        MeasuredRelayResistanceProcessingTime sut = MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan());
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
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new();
        MeasuredRelayResistanceProcessingTime sut = MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan());
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
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new(new byte[] {0x3c, 0x4d});
        MeasuredRelayResistanceProcessingTime sut = MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan());
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
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new(new byte[] {0x3c, 0x4d});
        MeasuredRelayResistanceProcessingTime sut = MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void InvalidBerEncoding_EncodingToDataElement_Throws()
    {
        MeasuredRelayResistanceProcessingTimeTestTlv testData = new(new byte[]
        {
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf
        });

        Assert.Throws<DataElementParsingException>(() => MeasuredRelayResistanceProcessingTime.Decode(testData.EncodeValue().AsSpan()));
    }

    #endregion

    #region MeasuredRelayResistanceProcessingTime - Tests

    [Fact]
    public void MeasuredRelayResistanceProcessingTime_CreateMeasuredRelayResistanceProcessingTime_ReturnsZero()
    {
        Microseconds timeElapsed = new(20000);
        TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedCapduTransmissionTime = new(400);
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedRapduTransmissionTime = new(200);
        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceEstimatedTransmissionTime = new(300);

        MeasuredRelayResistanceProcessingTime expected = new(0);
        var actual = MeasuredRelayResistanceProcessingTime.Create(timeElapsed, terminalExpectedCapduTransmissionTime, terminalExpectedRapduTransmissionTime,
            deviceEstimatedTransmissionTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MeasuredRelayResistanceProcessingTime_CreateMeasuredRelayResistanceProcessingTime_Returns100RelaySeconds()
    {
        Microseconds timeElapsed = new(30000);
        TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedCapduTransmissionTime = new(400);
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedRapduTransmissionTime = new(200);
        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceEstimatedTransmissionTime = new(300);

        MeasuredRelayResistanceProcessingTime expected = new(100);
        var actual = MeasuredRelayResistanceProcessingTime.Create(timeElapsed, terminalExpectedCapduTransmissionTime, terminalExpectedRapduTransmissionTime,
            deviceEstimatedTransmissionTime);

        Assert.Equal(expected, actual);
    }

    #endregion
}