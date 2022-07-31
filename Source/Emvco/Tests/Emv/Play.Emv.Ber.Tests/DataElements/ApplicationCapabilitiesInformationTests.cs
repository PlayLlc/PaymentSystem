using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ApplicationCapabilitiesInformationTests
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
        ApplicationCapabilitiesInformationTestTlv testData = new();
        ApplicationCapabilitiesInformation testValue = ApplicationCapabilitiesInformation.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationCapabilitiesInformationTestTlv testData = new();
        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationCapabilitiesInformationTestTlv testData = new();
        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(testData.EncodeValue().AsSpan());
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
        ApplicationCapabilitiesInformationTestTlv testData = new();
        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ApplicationCapabilitiesInformation.Tag, testData.EncodeValue());
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
        ApplicationCapabilitiesInformationTestTlv testData = new();
        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? tlv = sut.AsTagLengthValue();
        TagLengthValue? expectedResultTlv = new(testData.GetTag(), testData.EncodeValue());

        Assert.Equal(expectedResultTlv.EncodeValue(), tlv.EncodeValue());
    }

    [Fact]
    public void ApplicationCapabilitiesInformation_CombinedDataAuthenticationIndicator_ReturnsTrue()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[] {13, 3, 17};
        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        Assert.True(sut.CombinedDataAuthenticationIndicator());
    }

    [Fact]
    public void ApplicationCapabilitiesInformation_CombinedDataAuthenticationIndicator_ReturnsFalse()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[] {0b1000_1000, 0b0001_1010, 0b01010_1110};
        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        Assert.False(sut.CombinedDataAuthenticationIndicator());
    }

    [Fact]
    public void ApplicationCapabilitiesInformation_GetSdsSchemeIndicatorUndefinedSDSconfiguration_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            13, 21,

            //undefined sds configuration
            0
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        SdsSchemeIndicators expected = SdsSchemeIndicators.UndefinedSDSconfiguration;
        SdsSchemeIndicators actual = sut.GetSdsSchemeIndicator();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ApplicationCapabilitiesInformation_GetSdsSchemeIndicatorAll10tags128bytes_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            13, 21,

            //undefined sds configuration
            5
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        SdsSchemeIndicators expected = SdsSchemeIndicators.All10tags128bytes;
        SdsSchemeIndicators actual = sut.GetSdsSchemeIndicator();

        Assert.Equal(expected, actual);
    }

    //all10tags160bytes

    [Fact]
    public void ApplicationCapabilitiesInformation_GetSdsSchemeIndicatorAll10tags160bytes_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            13, 21,

            //undefined sds configuration
            6
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        SdsSchemeIndicators expected = SdsSchemeIndicators.All10tags160bytes;
        SdsSchemeIndicators actual = sut.GetSdsSchemeIndicator();

        Assert.Equal(expected, actual);
    }

    //All10tags192bytes
    [Fact]
    public void ApplicationCapabilitiesInformation_GetSdsSchemeIndicatorAll10tags192bytes_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            13, 21,

            //undefined sds configuration
            7
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        SdsSchemeIndicators expected = SdsSchemeIndicators.All10tags192bytes;
        SdsSchemeIndicators actual = sut.GetSdsSchemeIndicator();

        Assert.Equal(expected, actual);
    }

    //All10tags32bytes
    [Fact]
    public void ApplicationCapabilitiesInformation_GetSdsSchemeIndicatorAll10tags32bytes_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            13, 21,

            //undefined sds configuration
            1
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        SdsSchemeIndicators expected = SdsSchemeIndicators.All10tags32bytes;
        SdsSchemeIndicators actual = sut.GetSdsSchemeIndicator();

        Assert.Equal(expected, actual);
    }

    //All10tags48bytes
    [Fact]
    public void ApplicationCapabilitiesInformation_GetSdsSchemeIndicatorAll10tags48bytes_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            13, 21,

            //undefined sds configuration
            2
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        SdsSchemeIndicators expected = SdsSchemeIndicators.All10tags48bytes;
        SdsSchemeIndicators actual = sut.GetSdsSchemeIndicator();

        Assert.Equal(expected, actual);
    }

    //All10tags64bytes
    [Fact]
    public void ApplicationCapabilitiesInformation_GetSdsSchemeIndicatorAll10tags64bytes_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            13, 21,

            //undefined sds configuration
            3
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        SdsSchemeIndicators expected = SdsSchemeIndicators.All10tags64bytes;
        SdsSchemeIndicators actual = sut.GetSdsSchemeIndicator();

        Assert.Equal(expected, actual);
    }

    //All10tags96bytes
    [Fact]
    public void ApplicationCapabilitiesInformation_GetSdsSchemeIndicatorAll10tags96bytes_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            13, 21,

            //undefined sds configuration
            4
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        SdsSchemeIndicators expected = SdsSchemeIndicators.All10tags96bytes;
        SdsSchemeIndicators actual = sut.GetSdsSchemeIndicator();

        Assert.Equal(expected, actual);
    }

    //AllSdstags32bytesexcept
    [Fact]
    public void ApplicationCapabilitiesInformation_GetSdsSchemeIndicatorAllSdstags32bytesexcept_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            13, 21,

            //undefined sds configuration
            8
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        SdsSchemeIndicators expected = SdsSchemeIndicators.AllSdstags32bytesexcept;
        SdsSchemeIndicators actual = sut.GetSdsSchemeIndicator();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ApplicationCapabilitiesInformation_GetDataStorageVersionNumberOtherValueRFU_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            7, 21,

            //undefined sds configuration
            8
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        DataStorageVersionNumber expected = new(7);
        DataStorageVersionNumber actual = sut.GetDataStorageVersionNumber();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ApplicationCapabilitiesInformation_GetDataStorageVersionNumberDataStorageNotSupported_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            192, 21,

            //undefined sds configuration
            8
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        DataStorageVersionNumber expected = new(0);
        DataStorageVersionNumber actual = sut.GetDataStorageVersionNumber();

        Assert.Equal(expected, actual);
        Assert.Equal((byte)expected, (byte)DataStorageVersionNumbers.NotSupported);
    }

    [Fact]
    public void ApplicationCapabilitiesInformation_GetDataStorageVersionNumberDataStorageVersion1_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            193, 21,

            //undefined sds configuration
            8
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        DataStorageVersionNumber expected = new(0b1);
        DataStorageVersionNumber actual = sut.GetDataStorageVersionNumber();

        Assert.Equal(expected, actual);
        Assert.Equal((byte)expected, (byte)DataStorageVersionNumbers.Version1);
    }

    [Fact]
    public void ApplicationCapabilitiesInformation_GetDataStorageVersionNumberDataStorageVersion2_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> contentOctets = stackalloc byte[]
        {
            194, 21,

            //undefined sds configuration
            8
        };

        ApplicationCapabilitiesInformation sut = ApplicationCapabilitiesInformation.Decode(contentOctets);

        DataStorageVersionNumber expected = new(0b10);
        DataStorageVersionNumber actual = sut.GetDataStorageVersionNumber();

        Assert.Equal(expected, actual);
        Assert.Equal((byte)expected, (byte)DataStorageVersionNumbers.Version2);
    }

    #endregion
}