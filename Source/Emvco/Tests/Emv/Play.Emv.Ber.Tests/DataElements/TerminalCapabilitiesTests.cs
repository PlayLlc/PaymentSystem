using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class TerminalCapabilitiesTests
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
        TerminalCapabilitiesTestTlv testData = new();
        TerminalCapabilities testValue = TerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        TerminalCapabilitiesTestTlv testData = new();
        TerminalCapabilities sut = TerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        TerminalCapabilitiesTestTlv testData = new();
        TerminalCapabilities sut = TerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        TerminalCapabilitiesTestTlv testData = new();
        TerminalCapabilities sut = TerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(TerminalCapabilities.Tag, testData.EncodeValue());
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
        TerminalCapabilitiesTestTlv testData = new();
        TerminalCapabilities sut = TerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

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
        TerminalCapabilitiesTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => TerminalCapabilities.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        TerminalCapabilitiesTestTlv testData = new();
        TerminalCapabilities sut = TerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        TerminalCapabilitiesTestTlv testData = new();
        TerminalCapabilities sut = TerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        TerminalCapabilitiesTestTlv testData = new(new byte[] { 0x08, 0xEF, 0x47, });
        TerminalCapabilities sut = TerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        TerminalCapabilitiesTestTlv testData = new(new byte[] { 0xF, 0x8D, 0x3F });

        TerminalCapabilities sut = TerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region TerminalCapabilities

    [Fact]
    public void TerminalCapabilities_IsCardCaptureSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b100000, 0, 0 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsCardCaptureSupported());
    }

    [Fact]
    public void TerminalCapabilities_IsCombinedDataAuthenticationSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b10000, 0, 0 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsCombinedDataAuthenticationSupported());
    }

    [Fact]
    public void TerminalCapabilities_IsDynamicDataAuthenticationSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b1000000, 0, 0 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsDynamicDataAuthenticationSupported());
    }

    [Fact]
    public void TerminalCapabilities_IsEncipheredPinForOfflineVerificationSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b1000000, 0b10000, 0 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsEncipheredPinForOfflineVerificationSupported());
    }

    [Fact]
    public void TerminalCapabilities_IsEncipheredPinForOnlineVerificationSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b1000000, 0b1000000, 0 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsEncipheredPinForOnlineVerificationSupported());
    }

    [Fact]
    public void TerminalCapabilities_IsIcWithContactsSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b1000000, 0b1000000, 0b100000 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsIcWithContactsSupported());
    }

    [Fact]
    public void TerminalCapabilities_IsMagneticStripeSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b1000000, 0b1000000, 0b1000000 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsMagneticStripeSupported());
    }

    [Fact]
    public void TerminalCapabilities_IsManualKeyEntrySupported_ReturnsTrue()
    {
        byte[] testValue = { 0b1000000, 0b1000000, 0b10000000 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsManualKeyEntrySupported());
    }

    [Fact]
    public void TerminalCapabilities_IsNoCardVerificationMethodRequiredSet_ReturnsTrue()
    {
        byte[] testValue = { 0b1000000, 0b1000, 0b10000000 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsManualKeyEntrySupported());
    }

    [Fact]
    public void TerminalCapabilities_IsPlaintextPinForIccVerificationSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b1000000, 0b10000000, 0b10000000 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsPlaintextPinForIccVerificationSupported());
    }

    [Fact]
    public void TerminalCapabilities_IsSignaturePaperSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b1000000, 0b100000, 0b10000000 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsSignaturePaperSupported());
    }

    [Fact]
    public void TerminalCapabilities_IsStaticDataAuthenticationSupported_ReturnsTrue()
    {
        byte[] testValue = { 0b10000000, 0b100000, 0b10000000 };
        TerminalCapabilities sut = TerminalCapabilities.Decode(testValue.AsSpan());

        Assert.True(sut.IsStaticDataAuthenticationSupported());
    }

    #endregion

    #region Builder

    [Fact]
    public void TerminalCapabilitiesBuilder_Instantiate_BuilderInstantiated()
    {
        TerminalCapabilities.Builder builder = TerminalCapabilities.GetBuilder();

        Assert.NotNull(builder);
    }

    [Fact]
    public void TerminalCapabilitiesBuilder_Reset_ReturnsExpectedResult()
    {
        TerminalCapabilities.Builder builder = TerminalCapabilities.GetBuilder();

        TerminalCapabilitiesTestTlv testValue = new();
        TerminalCapabilities expected = TerminalCapabilities.Decode(testValue.EncodeValue().AsSpan());

        builder.Reset(expected);
        TerminalCapabilities actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TerminalCapabilitiesBuilder_SetCardVerificationMethodNotRequired_ReturnsExpectedResult()
    {
        TerminalCapabilities.Builder builder = TerminalCapabilities.GetBuilder();

        TerminalCapabilities expected = TerminalCapabilities.Decode(new byte[] { 0, 0b1000, 0}.AsSpan());

        builder.SetCardVerificationMethodNotRequired(true);

        TerminalCapabilities actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    #endregion
}
