using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class Track1DataTests
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
        Track1DataTestTlv testData = new();
        Track1Data testValue = Track1Data.Decode(testData.EncodeValue().AsSpan());
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
        Track1DataTestTlv testData = new();
        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());
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
        Track1DataTestTlv testData = new();
        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());
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
        Track1DataTestTlv testData = new();
        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(Track1Data.Tag, testData.EncodeValue());
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
        Track1DataTestTlv testData = new();
        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());

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
        Track1DataTestTlv testData = new(new byte[] {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01});

        Assert.Throws<DataElementParsingException>(() => Track1Data.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        Track1DataTestTlv testData = new();
        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());
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
        Track1DataTestTlv testData = new();
        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());
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
        Track1DataTestTlv testData = new(new byte[] {114, 120, 43, 54, 55});
        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());
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
        Track1DataTestTlv testData = new(new byte[] {33, 34, 44, 45, 64, 67});

        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region GetDiscretionaryData

    [Fact]
    public void Track1Data_GetTrack1DiscretionaryData_ReturnsExpectedTestTlvResult()
    {
        Track1DataTestTlv testData = new();

        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());

        ReadOnlySpan<byte> expectedDiscretionaryDecodedData = stackalloc byte[]
        {
            61, 84, 98, 62, (byte) '^', 44, 92, 42, 80, 64,
            62, 83, 99, 61, (byte) '^', 44, 92, 43, 80, 65
        };

        Track1DiscretionaryData actual = sut.GetTrack1DiscretionaryData();

        Assert.Equal(expectedDiscretionaryDecodedData.ToArray(), actual.EncodeValue());
    }

    [Fact]
    public void Track1Data_GetPrimaryAccountNumber_ReturnsExpectedTestTlvResult()
    {
        Track1DataTestTlv testData = new();

        Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());

        ReadOnlySpan<byte> expectedPrimaryAccountNumberDecodedData = stackalloc byte[] {84, 98, 62, 33, 44, 92, 42, 80, 64};

        TrackPrimaryAccountNumber actual = sut.GetPrimaryAccountNumber();

        Assert.Equal(expectedPrimaryAccountNumberDecodedData.ToArray(), actual.Encode());
    }

    //[Fact]
    //public void Track1Data_UpdateDiscretionaryData_ReturnsExpectedResult()
    //{
    //    NumberOfNonZeroBits nun = new NumberOfNonZeroBits(5);

    //    ReadOnlyMemory<byte> cvc3Bytes = new ReadOnlyMemory<byte>(new byte[] { 34, 48 });
    //    CardholderVerificationCode3Track1 cvc3 = CardholderVerificationCode3Track1.Decode(cvc3Bytes);

    //    ReadOnlySpan<byte> pcvc3Bytes = stackalloc byte[] { 12, 43, 31, 6, 7, 9 };
    //    PositionOfCardVerificationCode3Track1 pcvc3 = PositionOfCardVerificationCode3Track1.Decode(pcvc3Bytes);

    //    ReadOnlySpan<byte> punatcBytes = stackalloc byte[] { 11, 28, 33, 13, 6, 9 };
    //    PunatcTrack1 punatc = PunatcTrack1.Decode(punatcBytes);

    //    UnpredictableNumberNumeric unpredictableNumber = new UnpredictableNumberNumeric(1234);

    //    NumericApplicationTransactionCounterTrack1 natc = new NumericApplicationTransactionCounterTrack1(64);

    //    ApplicationTransactionCounter atc = new ApplicationTransactionCounter(18);

    //    Track1DataTestTlv testData = new();
    //    Track1Data sut = Track1Data.Decode(testData.EncodeValue().AsSpan());

    //    Track1Data actual = sut.UpdateDiscretionaryData(nun, cvc3, pcvc3, punatc, unpredictableNumber, natc, atc);
    //}

    #endregion
}