using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class DataStorageSlotManagementControlTests
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
        DataStorageSlotManagementControlTestTlv testData = new();
        DataStorageSlotManagementControl testValue = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotManagementControlTestTlv testData = new();
        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotManagementControlTestTlv testData = new();
        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotManagementControlTestTlv testData = new();
        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(DataStorageSlotManagementControl.Tag, testData.EncodeValue());
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
        DataStorageSlotManagementControlTestTlv testData = new();
        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

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
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });

        Assert.Throws<DataElementParsingException>(() => DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        DataStorageSlotManagementControlTestTlv testData = new();
        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotManagementControlTestTlv testData = new();
        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0x7d });
        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0xFF });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region DataStorageSlotManagementControl

    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsPermanent_ReturnsTrue()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b1111_1111 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsPermanent());
    }

    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsPermanent_ReturnsFalse()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b0111_1111 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsPermanent());
    }

    //IsVolatile
    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsVolatile_ReturnsTrue()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b1111_1111 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsVolatile());
    }

    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsVolatile_ReturnsFalse()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b1011_1111 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsVolatile());
    }

    //IsLowVolatility
    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsLowVolatility_ReturnsTrue()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b1111_1111 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsLowVolatility());
    }

    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsLowVolatility_ReturnsFalse()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b1101_1111 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsLowVolatility());
    }

    //IsLocked
    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsLocked_ReturnsTrue()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b1111_1111 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsLocked());
    }

    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsLocked_ReturnsFalse()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b1110_1111 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsLocked());
    }

    //IsDeactivated
    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsDeactivated_ReturnsTrue()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b1111_1111 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsDeactivated());
    }

    [Fact]
    public void DataStorageSlotManagementControl_InvokingIsDeactivated_ReturnsFalse()
    {
        DataStorageSlotManagementControlTestTlv testData = new(new byte[] { 0b1111_1110 });

        DataStorageSlotManagementControl sut = DataStorageSlotManagementControl.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsDeactivated());
    }

    #endregion
}
