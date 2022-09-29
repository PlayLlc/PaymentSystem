using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;
using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class TerminalTypeTests
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
        TerminalTypeTestTlv testData = new();
        TerminalType testValue = TerminalType.Decode(testData.EncodeValue().AsSpan());
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
        TerminalTypeTestTlv testData = new();
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());
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
        TerminalTypeTestTlv testData = new();
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());
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
        TerminalTypeTestTlv testData = new();
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(TerminalType.Tag, testData.EncodeValue());
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
        TerminalTypeTestTlv testData = new();
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());

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
        TerminalTypeTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => TerminalType.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        TerminalTypeTestTlv testData = new();
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());
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
        TerminalTypeTestTlv testData = new();
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());
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
        TerminalTypeTestTlv testData = new(new byte[] { 74 });
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());
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
        TerminalTypeTestTlv testData = new(new byte[] { 97 });

        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region TerminalType - CommunicationType

    [Fact]
    public void TerminalType_GetCommunicationType_ReturnsExpectedResult()
    {
        TerminalTypeTestTlv testData = new(new byte[] { 43 });
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());

        Assert.Equal(TerminalType.CommunicationType.OfflineOnly, sut.GetCommunicationType());
    }

    [Fact]
    public void TerminalType_GetCommunicationType_ReturnsExpectedResult2()
    {
        TerminalTypeTestTlv testData = new(new byte[] { 42 });
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());

        Assert.Equal(TerminalType.CommunicationType.OnlineAndOfflineCapable, sut.GetCommunicationType());
    }

    [Fact]
    public void TerminalType_IsCommunicatioType_ReturnsTrue()
    {
        TerminalTypeTestTlv testData = new(new byte[] { 42 });
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsCommunicationType(TerminalType.CommunicationType.OnlineAndOfflineCapable));
    }

    #endregion

    #region TerminalType - EnvironmentType

    [Fact]
    public void TerminalType_GetEnvironment_ReturnsUnattended()
    {
        TerminalTypeTestTlv testData = new(new byte[] { 44 });
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());

        Assert.Equal(TerminalType.EnvironmentType.Unattended, sut.GetEnvironment());
    }

    [Fact]
    public void TerminalType_GetEnvironment_ReturnsAttended()
    {
        TerminalTypeTestTlv testData = new(new byte[] { 43 });
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());

        Assert.Equal(TerminalType.EnvironmentType.Attended, sut.GetEnvironment());
    }

    [Fact]
    public void TerminalType_IsEnvironmentType_ReturnsTrue()
    {
        TerminalTypeTestTlv testData = new(new byte[] { 43 });
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsEnvironmentType(TerminalType.EnvironmentType.Attended));
    }

    #endregion

    #region TerminalType - TerminalOperatorType

    [Fact]
    public void TerminalType_GetTerminalOperatorType_ReturnsExpectedResult()
    {
        TerminalTypeTestTlv testData = new(new byte[] { 23 });
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());

        Assert.Equal(TerminalType.TerminalOperatorType.Merchant, sut.GetTerminalOperatorType());
    }

    [Fact]
    public void TerminalType_IsOperatorType_ReturnsTrue()
    {
        TerminalTypeTestTlv testData = new(new byte[] { 23 });
        TerminalType sut = TerminalType.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsOperatorType(TerminalType.TerminalOperatorType.Merchant));
    }

    #endregion
}
