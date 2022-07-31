using System;
using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Globalization.Currency;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class CvmListTests
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
        CvmListTestTlv testData = new();
        CvmList testValue = CvmList.Decode(testData.EncodeValue().AsSpan());
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
        CvmListTestTlv testData = new();
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());
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
        CvmListTestTlv testData = new();
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());
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
        CvmListTestTlv testData = new();
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(CvmList.Tag, testData.EncodeValue());
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
        CvmListTestTlv testData = new();
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());

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
        CvmListTestTlv testData = new(new byte[] {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01});

        Assert.Throws<DataElementParsingException>(() => CvmList.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        CvmListTestTlv testData = new();
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());
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
        CvmListTestTlv testData = new();
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());
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
        CvmListTestTlv testData = new(new byte[] {0x32, 0x3D, 0x2c, 0x1a, 0x9, 0x58, 0x33, 0xF, 0x15, 0x25});
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());
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
        CvmListTestTlv testData = new(new byte[]
        {
            0x32, 0x3D, 0x2c, 0x1a, 0x9, 0x58, 0x33, 0xF, 0x15, 0x25,
            0x68, 0x89
        });

        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region CvmList

    [Fact]
    public void InstantiatedCvmListWithSmallerLengthThen8_InvokingAreCardholderVerificationRulesPresent_ExceptionIsThrown()
    {
        Assert.Throws<DataElementParsingException>(() =>
        {
            CvmList sut = new(new BigInteger(new byte[] {0x32, 0x3D, 0x2c, 0x1a, 0x9, 0x58}));

            bool cvmRulesPresent = sut.AreCardholderVerificationRulesPresent();
        });
    }

    [Fact]
    public void InstantiatedCvmListWithOddNumberOfBytes_InvokingAreCardholderVerificationRulesPresent_ReturnsFalse()
    {
        CvmList sut = new(new BigInteger(new byte[]
        {
            0x32, 0x3D, 0x2c, 0x1a, 0x9, 0x58, 0x17, 0x34, 0x10, 0x20,
            0x30
        }));

        bool cvmRulesPresent = sut.AreCardholderVerificationRulesPresent();

        Assert.False(cvmRulesPresent);
    }

    [Fact]
    public void InstantiatedCvmList_InvokingAreCardholderVerificationRulesPresent_ReturnsTrue()
    {
        CvmList sut = new(new BigInteger(new byte[] {0x32, 0x3D, 0x2c, 0x1a, 0x9, 0x58, 0x17, 0x34, 0x10, 0x20}));

        bool cvmRulesPresent = sut.AreCardholderVerificationRulesPresent();

        Assert.True(cvmRulesPresent);
    }

    [Fact]
    public void InvalidCvmList_InvokingTryGetCardholderVerificationRules_ReturnsFalseWithEmptyOutResult()
    {
        CvmList sut = new(new BigInteger(new byte[]
        {
            0x32, 0x3D, 0x2c, 0x1a, 0x9, 0x58, 0x17, 0x34, 0x10, 0x20,
            0x30
        }));

        bool result = sut.TryGetCardholderVerificationRules(out CvmRule[]? output);

        Assert.False(result);
        Assert.Equal(output, Array.Empty<CvmRule>());
    }

    [Fact]
    public void CvmList_InvokingTryGetCardholderVerificationRules_ReturnsExpectedResult()
    {
        CvmListTestTlv testData = new();
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());

        CvmRule[] expected = {new(new byte[] {0x34, 0x8F}), new(new byte[] {0x45, 0x7C})};
        bool result = sut.TryGetCardholderVerificationRules(out CvmRule[]? output);

        Assert.True(result);
        Assert.Equal(expected, output);
    }

    [Fact]
    public void CvmList_GetXAmount_ReturnsExpectedResult()
    {
        CvmListTestTlv testData = new();
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());

        NumericCurrencyCode currencyCode = new(840);

        ulong expectedAmount = PlayCodec.BinaryCodec.DecodeToUInt64(new byte[] {0x08, 0x32, 0x3c, 0x4d});
        Money expected = new(expectedAmount, currencyCode);

        Money xAmount = sut.GetXAmount(currencyCode);

        Assert.Equal((ulong) expected, (ulong) xAmount);
    }

    [Fact]
    public void CvmList_GetYAmount_ReturnsExpectedResult()
    {
        CvmListTestTlv testData = new();
        CvmList sut = CvmList.Decode(testData.EncodeValue().AsSpan());

        NumericCurrencyCode currencyCode = new(840);

        ulong expectedAmount = PlayCodec.BinaryCodec.DecodeToUInt64(new byte[] {0x16, 0x10, 0x8, 0x2});
        Money expected = new(expectedAmount, currencyCode);

        Money yAmount = sut.GetYAmount(currencyCode);

        Assert.Equal((ulong) expected, (ulong) yAmount);
    }

    #endregion
}