using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class DataStorageSummaryStatusTests
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
        DataStorageSummaryStatusTestTlv testData = new();
        DataStorageSummaryStatus testValue = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSummaryStatusTestTlv testData = new();
        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSummaryStatusTestTlv testData = new();
        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSummaryStatusTestTlv testData = new();
        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(DataStorageSummaryStatus.Tag, testData.EncodeValue());
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
        DataStorageSummaryStatusTestTlv testData = new();
        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());

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
        DataStorageSummaryStatusTestTlv testData = new(new byte[] {1, 2, 3, 4, 5, 6, 7});

        Assert.Throws<DataElementParsingException>(() => DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        DataStorageSummaryStatusTestTlv testData = new();
        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSummaryStatusTestTlv testData = new();
        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSummaryStatusTestTlv testData = new(new byte[] {0x7d});
        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());
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
        DataStorageSummaryStatusTestTlv testData = new(new byte[] {0x17});

        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region DataStorageSummaryStatus

    [Fact]
    public void DataStorageSummaryStatus_InvokingIsReadSuccessful_ReturnsTrue()
    {
        DataStorageSummaryStatusTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsReadSuccessful());
    }

    [Fact]
    public void DataStorageSummaryStatus_InvokingIsReadSuccessful_ReturnsFalse()
    {
        DataStorageSummaryStatusTestTlv testData = new(new byte[] {0b0111_1111});

        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsReadSuccessful());
    }

    [Fact]
    public void DataStorageSummaryStatus_InvokingIsSuccessfulWrite_ReturnsTrue()
    {
        DataStorageSummaryStatusTestTlv testData = new(new byte[] {0b1111_1111});

        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());

        Assert.True(sut.IsSuccessfulWrite());
    }

    [Fact]
    public void DataStorageSummaryStatus_InvokingIsSuccessfulWrite_ReturnsFalse()
    {
        DataStorageSummaryStatusTestTlv testData = new(new byte[] {0b1011_1111});

        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsSuccessfulWrite());
    }

    [Fact]
    public void DataStorageSummaryStatus_GetBuilder_ReturnsExpectedResult()
    {
        DataStorageSummaryStatusTestTlv testData = new(new byte[] {0b1011_1111});

        DataStorageSummaryStatus sut = DataStorageSummaryStatus.Decode(testData.EncodeValue().AsSpan());

        DataStorageSummaryStatus.Builder builder = DataStorageSummaryStatus.GetBuilder();

        builder.Reset(sut);

        Assert.NotNull(builder);
        Assert.Equal(builder.Complete(), sut);
    }

    #endregion
}