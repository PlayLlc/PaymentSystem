using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Country;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ThirdPartyDataTests
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
        ThirdPartyDataTestTlv testData = new();
        ThirdPartyData testValue = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());
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
        ThirdPartyDataTestTlv testData = new();
        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());
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
        ThirdPartyDataTestTlv testData = new();
        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());
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
        ThirdPartyDataTestTlv testData = new();
        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ThirdPartyData.Tag, testData.EncodeValue());
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
        ThirdPartyDataTestTlv testData = new();
        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());

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
    public void InvalidBerEncoding_DeserializingDataElementInvalidByteLength_Throws()
    {
        ThirdPartyDataTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03 });

        Assert.Throws<DataElementParsingException>(() => ThirdPartyData.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        ThirdPartyDataTestTlv testData = new();
        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());
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
        ThirdPartyDataTestTlv testData = new();
        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());
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
        ThirdPartyDataTestTlv testData = new(new byte[]
        {
            0x08, 0x64, 0x4c, 0x3e, 0x0f, 0x12, 0x15
        });
        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());
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
        ThirdPartyDataTestTlv testData = new(new byte[]
        {
            0x08, 0x64, 0x4c, 0x3e, 0x0f, 0x12, 0x15
        });

        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void ThirdPartyData_InvokesGetCountryCode_ReturnsExpectedResult()
    {
        //US Country Code.
        ThirdPartyDataTestTlv testData = new(new byte[]
        {
            85, 83, 0x4c, 0x3e, 0x0f, 0x12, 0x15
        });

        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());

        ReadOnlySpan<char> input = stackalloc char[] { 'U', 'S' };

        Alpha2CountryCode expected = new Alpha2CountryCode(input);
        Alpha2CountryCode countryCode = sut.GetCountryCode();

        Assert.Equal(expected, countryCode);
    }

    [Fact]
    public void ThirdPartyData_InvokesGetUniqueIdentifier_ReturnsExpectedResult()
    {
        //US Country Code.
        ThirdPartyDataTestTlv testData = new(new byte[]
        {
            85, 83, 0x4c, 0x3e, 0x0f, 0x12, 0x15
        });

        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());

        ushort expected = 19518;
        ushort actual = sut.GetUniqueIdentifier();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ThirdPartyData_InvokesTryGetDeviceTypeDeviceIsPresent_ReturnsExpectedResult()
    {
        //US Country Code.
        ThirdPartyDataTestTlv testData = new(new byte[]
        {
            85, 83, 0b1000_1000, 0b1010_1010, 12, 13,
        });

        ThirdPartyData sut = ThirdPartyData.Decode(testData.EncodeValue().AsSpan());

        ushort expected = 3085;
        sut.TryGetDeviceType(out ushort? actual);

        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    #endregion
}

