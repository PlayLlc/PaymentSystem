using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class DataStorageSlotAvailabilityTests
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
        DataStorageSlotAvailabilityTestTlv testData = new();
        DataStorageSlotAvailability testValue = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotAvailabilityTestTlv testData = new();
        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotAvailabilityTestTlv testData = new();
        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotAvailabilityTestTlv testData = new();
        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(DataStorageSlotAvailability.Tag, testData.EncodeValue());
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
        DataStorageSlotAvailabilityTestTlv testData = new();
        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());

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
        DataStorageSlotAvailabilityTestTlv testData = new(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });

        Assert.Throws<DataElementParsingException>(() => DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        DataStorageSlotAvailabilityTestTlv testData = new();
        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotAvailabilityTestTlv testData = new();
        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotAvailabilityTestTlv testData = new(new byte[] { 0x7d });
        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotAvailabilityTestTlv testData = new(new byte[] { 0xFF });

        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region DataStorageSlotAvailability

    [Fact]
    public void DataStorageSlotAvailability_InvokingIsPermanentSlotTypeSet_ReturnsTrue()
    {
        DataStorageSlotAvailabilityTestTlv testData = new(new byte[] { 0b1111_1111 });

        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsPermanentSlotTypeSet());
    }

    [Fact]
    public void DataStorageSlotAvailability_InvokingIsPermanentSlotTypeSet_ReturnsFalse()
    {
        DataStorageSlotAvailabilityTestTlv testData = new(new byte[] { 0b0111_1111 });

        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsPermanentSlotTypeSet());
    }

    [Fact]
    public void DataStorageSlotAvailability_InvokingIsVolatileSlotTypeSet_ReturnsTrue()
    {
        DataStorageSlotAvailabilityTestTlv testData = new(new byte[] { 0b1111_1111 });

        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsVolatileSlotTypeSet());
    }

    [Fact]
    public void DataStorageSlotAvailability_InvokingIsVolatileSlotTypeSet_ReturnsFalse()
    {
        DataStorageSlotAvailabilityTestTlv testData = new(new byte[] { 0b1011_1111 });

        DataStorageSlotAvailability sut = DataStorageSlotAvailability.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsVolatileSlotTypeSet());
    }

    #endregion
}
