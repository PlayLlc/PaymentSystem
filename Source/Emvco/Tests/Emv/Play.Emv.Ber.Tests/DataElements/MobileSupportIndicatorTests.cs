using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class MobileSupportIndicatorTests
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
        MobileSupportIndicatorTestTlv testData = new();
        MobileSupportIndicator testValue = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());
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
        MobileSupportIndicatorTestTlv testData = new();
        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());
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
        MobileSupportIndicatorTestTlv testData = new();
        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());
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
        MobileSupportIndicatorTestTlv testData = new();
        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(MobileSupportIndicator.Tag, testData.EncodeValue());
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
        MobileSupportIndicatorTestTlv testData = new();
        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());

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
        MobileSupportIndicatorTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        MobileSupportIndicatorTestTlv testData = new();
        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());
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
        MobileSupportIndicatorTestTlv testData = new();
        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());
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
        MobileSupportIndicatorTestTlv testData = new(new byte[] { 0x32 });
        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());
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
        MobileSupportIndicatorTestTlv testData = new(new byte[] { 0xEF });

        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region MobileSupportIndicator

    [Fact]
    public void MobileSupportIndicator_IsOnDeviceCvmRequired_ReturnsTrue()
    {
        MobileSupportIndicatorTestTlv testData = new(new byte[] { 0b10 });

        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsOnDeviceCvmRequired());
    }

    [Fact]
    public void MobileSupportIndicator_IsMobileSupported_ReturnsTrue()
    {
        MobileSupportIndicatorTestTlv testData = new(new byte[] { 0b1 });

        MobileSupportIndicator sut = MobileSupportIndicator.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsMobileSupported());
    }

    #endregion

    #region Builder

    [Fact]
    public void MobileSupportIndicatorBuilder_Instantiate_BuilderInstantiated()
    {
        MobileSupportIndicator.Builder builder = MobileSupportIndicator.GetBuilder();

        Assert.NotNull(builder);
    }

    [Fact]
    public void MobileSupportIndicatorBuilder_Reset_ReturnsExpectedResult()
    {
        MobileSupportIndicator.Builder builder = MobileSupportIndicator.GetBuilder();

        builder.Reset(MobileSupportIndicator.Default);
        Assert.Equal(MobileSupportIndicator.Default, builder.Complete());
    }

    [Fact]
    public void MobileSupportIndicatorBuilder_SetOnDeviceCvmRequired_ReturnsExpectedResult()
    {
        MobileSupportIndicator.Builder builder = MobileSupportIndicator.GetBuilder();
        MobileSupportIndicator expected = new MobileSupportIndicator(0b10);

        builder.SetOnDeviceCvmRequired(true);
        Assert.Equal(expected, builder.Complete());
    }

    [Fact]
    public void MobileSupportIndicatorBuilder_SetMobileSupported_ReturnsExpectedResult()
    {
        MobileSupportIndicator.Builder builder = MobileSupportIndicator.GetBuilder();
        MobileSupportIndicator expected = new MobileSupportIndicator(0b1);

        builder.SetMobileSupported(true);
        Assert.Equal(expected, builder.Complete());
    }

    #endregion
}
