using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class AdditionalTerminalCapabilitiesTests
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
        AdditionalTerminalCapabilitiesTestTlv testData = new();
        AdditionalTerminalCapabilities testValue = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        AdditionalTerminalCapabilitiesTestTlv testData = new();
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        AdditionalTerminalCapabilitiesTestTlv testData = new();
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        AdditionalTerminalCapabilitiesTestTlv testData = new();
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(AdditionalTerminalCapabilities.Tag, testData.EncodeValue());
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
        AdditionalTerminalCapabilitiesTestTlv testData = new();
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

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
        AdditionalTerminalCapabilitiesTestTlv testData = new();
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        AdditionalTerminalCapabilitiesTestTlv testData = new();
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0x4d, 0x3c, 0x2b, 0x1a, 0x9b });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
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
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[]
        {
            0x4d, 0x3c, 0x2b, 0x1a, 0x4f
        });

        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void InvalidBerEncoding_EncodingToDataElement_Throws()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[]
        {
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
        });

        Assert.Throws<DataElementParsingException>(() => AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan()));
    }

    #endregion

    #region AdditionalTerminalCapabilities

    //Administrative
    [Fact]
    public void AdditionalTerminalCapabilities_Administrative_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 1, 2, 3, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.Administrative());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Administrative_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 2, 2, 3, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.Administrative());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_AlphabeticalAndSpecialCharactersKeys_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 1, 2, 0b0110_0000, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.AlphabeticalAndSpecialCharactersKeys());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_AlphabeticalAndSpecialCharactersKeys_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 2, 2, 0b1001_0000, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.AlphabeticalAndSpecialCharactersKeys());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Cash_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1000_0000, 2, 0b0110_0000, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.Cash());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Cash_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 2, 0b1001_0000, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.Cash());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Cashback_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 2, 0b0110_0000, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.Cashback());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Cashback_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 2, 0b1001_0000, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.Cashback());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CashDeposit_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CashDeposit());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CashDeposit_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 4, 5 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CashDeposit());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable1_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 4, 1 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable1());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable1_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 4, 2 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable1());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable10_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0110, 1 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable10());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable10_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 2 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable10());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable2_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0110, 2 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable2());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable2_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 1 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable2());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable3_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0110, 4 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable3());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable3_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable3());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable4_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0110, 0b1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable4());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable4_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable4());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable5_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0110, 0b1_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable5());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable5_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable5());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable6_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0110, 0b11_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable6());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable6_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable6());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable7_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0110, 0b111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable7());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable7_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable7());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable8_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0110, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable8());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable8_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable8());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable9_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CodeTable9());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CodeTable9_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CodeTable9());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CommandKeys_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.CommandKeys());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_CommandKeys_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.CommandKeys());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_DisplayAttendant_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b1010_0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.DisplayAttendant());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_DisplayAttendant_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.DisplayAttendant());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_DisplayCardholder_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0110_0000, 0b1011_0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.DisplayCardholder());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_DisplayCardholder_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1001_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.DisplayCardholder());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_FunctionKeys_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1001_0000, 0b1000_0000, 0b0111_0000, 0b1011_0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.FunctionKeys());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_FunctionKeys_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b0100_0000, 0b0100_0000, 0b1100_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.FunctionKeys());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Goods_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1101_0000, 0b1000_0000, 0b0111_0000, 0b1011_0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.Goods());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Goods_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1000_0000, 0b0100_0000, 0b1100_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.Goods());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Inquiry_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1101_1000, 0b1000_0000, 0b0111_0000, 0b1011_0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.Inquiry());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Inquiry_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1000_0000, 0b0100_0000, 0b1100_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.Inquiry());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_NumericKeys_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1101_1000, 0b1000_0000, 0b1111_0000, 0b1011_0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.NumericKeys());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_NumericKeys_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1000_0000, 0b0100_0000, 0b0011_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.NumericKeys());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Payment_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1101_1010, 0b1000_0000, 0b1111_0000, 0b1011_0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.Payment());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_Payment_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1000_0000, 0b0100_0000, 0b0011_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.Payment());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_PrintAttendant_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1101_1010, 0b1000_0000, 0b1111_0000, 0b1011_0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.PrintAttendant());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_PrintAttendant_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1000_0000, 0b0100_0000, 0b0011_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.PrintAttendant());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_PrintCardholder_ReturnsTrue()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1101_1010, 0b1000_0000, 0b1111_0000, 0b1111_0111, 0b1111_1000 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.PrintCardholder());
    }

    [Fact]
    public void AdditionalTerminalCapabilities_PrintCardholder_ReturnsFalse()
    {
        AdditionalTerminalCapabilitiesTestTlv testData = new(new byte[] { 0b1000_0000, 0b0100_0000, 0b0011_0000, 0b0100, 3 });
        AdditionalTerminalCapabilities sut = AdditionalTerminalCapabilities.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.PrintCardholder());
    }

    #endregion
}
