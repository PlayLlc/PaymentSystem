using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class TornRecordTests
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
        TornRecordTestTlv testData = new();
        TornRecord testValue = TornRecord.Decode(testData.EncodeValue().AsMemory());
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
        TornRecordTestTlv testData = new();
        TornRecord sut = TornRecord.Decode(testData.EncodeValue().AsMemory());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     InvalidBerEncoding_DeserializingDataElement_Throws
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void InvalidBerEncoding_DeserializingDataElement_Throws()
    {
        TornRecordTestTlv testData = new(new byte[] {2, 12, 1});

        Assert.Throws<TerminalDataException>(() => TornRecord.Decode(testData.EncodeValue().AsMemory()));
    }

    /// <summary>
    ///     BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        TornRecordTestTlv testData = new();
        TornRecord sut = TornRecord.Decode(testData.EncodeValue().AsMemory());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(TornRecord.Tag, testData.EncodeValue());
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
        TornRecordTestTlv testData = new();
        TornRecord sut = TornRecord.Decode(testData.EncodeValue().AsMemory());

        byte[] testValue = sut.EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        TornRecordTestTlv testData = new();
        TornRecord sut = TornRecord.Decode(testData.EncodeValue().AsMemory());
        int expectedResult = testData.GetValueByteCount();
        int testResult = sut.GetValueByteCount();

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
        TornRecordTestTlv testData = new();
        TornRecord sut = TornRecord.Decode(testData.EncodeValue().AsMemory());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region TornRecord Tests

    [Fact]
    public void TornRecord_TryGetRecordItem_NoItemFound()
    {
        TornRecordTestTlv testData = new();
        TornRecord testValue = TornRecord.Decode(testData.EncodeValue().AsMemory());

        bool result = testValue.TryGetRecordItem(ApplicationCapabilitiesInformation.Tag, out PrimitiveValue? primitive);

        Assert.False(result);
        Assert.Null(primitive);
    }

    [Fact]
    public void TornRecord_TryGetRecordItem_ItemFound()
    {
        TornRecordTestTlv testData = new();
        TornRecord testValue = TornRecord.Decode(testData.EncodeValue().AsMemory());

        bool result = testValue.TryGetRecordItem(new Tag(0x5A), out PrimitiveValue? primitive);

        Assert.True(result);
        Assert.NotNull(primitive);
    }

    [Fact]
    public void TornRecord_TryGetOldestRecord_ItemFound()
    {
        TornRecordTestTlv testData = new();
        TornRecord testValue = TornRecord.Decode(testData.EncodeValue().AsMemory());

        bool found = TornRecord.TryGetOldestRecord(new TornRecord[] {testValue}, out TornRecord? result);

        Assert.True(found);
        Assert.NotNull(result);
        Assert.Equal(result, testValue);
    }

    [Fact]
    public void TornRecord_TryGetOldestRecord_ReturnsOldestRecord()
    {
        TornRecordTestTlv anotherTestData = new(new byte[]
        {
            0x5F, 0x34, //Pan Sequence Number
            1, 12, 0x5A, // Tag
            9, // Length
            12, 23, 33, 13, 15, 12, 23, 33, 13 //Value,
        });
        TornRecord anotherTornRecord = TornRecord.Decode(anotherTestData.EncodeValue().AsMemory());

        TornRecordTestTlv testData = new();
        TornRecord tornRecord = TornRecord.Decode(testData.EncodeValue().AsMemory());

        bool found = TornRecord.TryGetOldestRecord(new TornRecord[] {anotherTornRecord, tornRecord}, out TornRecord? result);

        Assert.True(found);
        Assert.NotNull(result);
        Assert.Equal(result, anotherTornRecord);
    }

    [Fact]
    public void TornRecord_CreateEmptyTornRecord_CreatesExpectedItem()
    {
        TornRecord sut = TornRecord.CreateEmptyTornRecord();

        Assert.NotNull(sut);

        sut.TryGetRecordItem(ApplicationPan.Tag, out PrimitiveValue? applicationPan);
        Assert.NotNull(applicationPan);

        sut.TryGetRecordItem(ApplicationPanSequenceNumber.Tag, out PrimitiveValue? applicationPanSequenceNumber);
        Assert.NotNull(applicationPanSequenceNumber);
    }

    #endregion
}