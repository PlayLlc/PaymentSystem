using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ApplicationUsageControlTests
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
        ApplicationUsageControlTestTlv testData = new();
        ApplicationUsageControl testValue = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationUsageControlTestTlv testData = new();
        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationUsageControlTestTlv testData = new();
        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationUsageControlTestTlv testData = new();
        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ApplicationUsageControl.Tag, testData.EncodeValue());
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
        ApplicationUsageControlTestTlv testData = new();
        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());

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
        ApplicationUsageControlTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        ApplicationUsageControlTestTlv testData = new();
        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationUsageControlTestTlv testData = new();
        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationUsageControlTestTlv testData = new(new byte[] { 0x08, 0x32 });
        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0x08, 0x32
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region Byte 1

    [Fact]
    public void ApplicationUsageControlDataElement_IsValidForDomesticCashTransactions_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0b10000000, 0x08
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsValidForDomesticCashTransactions());
    }

    [Fact]
    public void ApplicationUsageControlDataElement_IsValidForInternationalCashTransactions_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0b11000000, 0x08
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsValidForInternationalCashTransactions());
    }

    [Fact]
    public void ApplicationUsageControlDataElement_IsValidForDomesticGoods_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0b11100000, 0x08
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsValidForDomesticGoods());
    }

    [Fact]
    public void ApplicationUsageControlDataElement_IsValidForInternationalGoods_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0b11110000, 0x08
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsValidForInternationalGoods());
    }

    [Fact]
    public void ApplicationUsageControlDataElement_IsValidForDomesticServices_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0b11111000, 0x08
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsValidForDomesticServices());
    }

    [Fact]
    public void ApplicationUsageControlDataElement_IsValidForInternationalServices_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0b11111100, 0x08
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsValidForInternationalServices());
    }

    [Fact]
    public void ApplicationUsageControlDataElement_IsValidAtAtms_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0b00111110, 0x08
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsValidAtAtms());
    }

    [Fact]
    public void ApplicationUsageControlDataElement_IsValidAtTerminalsOtherThanAtms_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0b00110011, 0x08
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsValidAtTerminalsOtherThanAtms());
    }

    #endregion

    #region Byte 2

    [Fact]
    public void ApplicationUsageControlDataElement_IsDomesticCashbackAllowed_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0x08, 0b10110011
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsDomesticCashbackAllowed());
    }

    [Fact]
    public void ApplicationUsageControlDataElement_IsInternationalCashbackAllowed_ReturnsTrue()
    {
        ApplicationUsageControlTestTlv testData = new(new byte[]
        {
            0x08, 0b01110011
        });

        ApplicationUsageControl sut = ApplicationUsageControl.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsInternationalCashbackAllowed());
    }

    #endregion
}
