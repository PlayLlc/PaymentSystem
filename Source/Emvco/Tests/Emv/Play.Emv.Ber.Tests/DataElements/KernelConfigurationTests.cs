using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class KernelConfigurationTests
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
        KernelConfigurationTestTlv testData = new();
        KernelConfiguration testValue = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());
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
        KernelConfigurationTestTlv testData = new();
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());
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
        KernelConfigurationTestTlv testData = new();
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());
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
        KernelConfigurationTestTlv testData = new();
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(KernelConfiguration.Tag, testData.EncodeValue());
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
        KernelConfigurationTestTlv testData = new();
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

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
        KernelConfigurationTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => KernelConfiguration.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        KernelConfigurationTestTlv testData = new();
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());
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
        KernelConfigurationTestTlv testData = new();
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());
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
        KernelConfigurationTestTlv testData = new(new byte[] { 0x32 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());
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
        KernelConfigurationTestTlv testData = new(new byte[]
        {
            0x08
        });

        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region Kernel DataElement

    [Fact]
    public void KernelConfiguration_IsEmvModeSupported_ReturnsTrue()
    { 
        KernelConfigurationTestTlv testData = new(new byte[] {0b1000_1000});
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsEmvModeSupported());
    }

    [Fact]
    public void KernelConfiguration_IsEmvModeSupported_ReturnsFalse()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b1100_1000 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsEmvModeSupported());
    }

    [Fact]
    public void KernelConfiguration_IsMagstripeModeSupported_ReturnsTrue()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b0100_1000 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsMagstripeModeSupported());
    }

    [Fact]
    public void KernelConfiguration_IsMagstripeModeSupported_ReturnsFalse()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b1100_1000 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsMagstripeModeSupported());
    }

    [Fact]
    public void KernelConfiguration_IsOnDeviceCardholderVerificationSupported_ReturnsTrue()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b0110_1000 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsOnDeviceCardholderVerificationSupported());
    }

    [Fact]
    public void KernelConfiguration_IsOnDeviceCardholderVerificationSupported_ReturnsFalse()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b1100_1000 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsOnDeviceCardholderVerificationSupported());
    }

    [Fact]
    public void KernelConfiguration_IsReadAllRecordsActivated_ReturnsTrue()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b0110_1100 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsReadAllRecordsActivated());
    }

    [Fact]
    public void KernelConfiguration_IsReadAllRecordsActivated_ReturnsFalse()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b1100_1000 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsReadAllRecordsActivated());
    }

    [Fact]
    public void KernelConfiguration_IsRelayResistanceProtocolSupported_ReturnsTrue()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b0111_1100 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsRelayResistanceProtocolSupported());
    }

    [Fact]
    public void KernelConfiguration_IsRelayResistanceProtocolSupported_ReturnsFalse()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b1100_1000 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsRelayResistanceProtocolSupported());
    }

    [Fact]
    public void KernelConfiguration_IsReservedForPaymentSystem_ReturnsTrue()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b0111_1100 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsReservedForPaymentSystem());
    }

    [Fact]
    public void KernelConfiguration_IsReservedForPaymentSystem_ReturnsFalse()
    {
        KernelConfigurationTestTlv testData = new(new byte[] { 0b1100_0110 });
        KernelConfiguration sut = KernelConfiguration.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsReservedForPaymentSystem());
    }
    #endregion
}
