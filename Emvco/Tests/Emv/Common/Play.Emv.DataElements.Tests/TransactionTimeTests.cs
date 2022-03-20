//using System;

//using Play.Ber.Codecs;
//using Play.Ber.DataObjects;
//using Play.Emv.Ber;
//using Play.Emv.Ber.DataElements;

//using Xunit;

//namespace Play.Emv.DataElements.Tests;

//public class TransactionTimeTests
//{
//    #region Instance Values

//    private readonly BerCodec _Codec = new(EmvCodec.Configuration);

//    #endregion

//    #region Tests

//    [Fact]
//    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
//    {
//        ReadOnlySpan<byte> contentOctets = BerTestData.Primitives.TransactionTime.ValueBytes.AsSpan();
//        var testValue = TransactionTime.Decode(contentOctets);
//        Assert.NotNull(testValue);
//    }

//    [Fact]
//    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
//    {
//        ReadOnlySpan<byte> contentOctets = BerTestData.Primitives.TransactionTime.ValueBytes.AsSpan();
//        var dataElement = TransactionTime.Decode(contentOctets);
//        var testValue = dataElement.Decode();

//        Assert.Equal(testValue, BerTestData.Primitives.TransactionTime.ValueBytes);
//    }

//    [Fact]
//    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
//    {
//        ReadOnlySpan<byte> contentOctets = BerTestData.Primitives.TransactionTime.ValueBytes.AsSpan();
//        var dataElement = TransactionTime.Decode(contentOctets);
//        var testValue = dataElement.AsTagLengthValue();
//        var expectedResult = new TagLengthValue(TransactionTime.Tag, contentOctets);
//        Assert.Equal(testValue, expectedResult);
//    }

//    [Fact]
//    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
//    {
//        ReadOnlySpan<byte> contentOctets = BerTestData.Primitives.TransactionTime.ValueBytes.AsSpan();
//        var dataElement = TransactionTime.Decode(contentOctets);
//        var testValue = dataElement.AsTagLengthValue().AsRawTlv();
//        var expectedResult = BerTestData.Primitives.TransactionTime.TlvBytes;

//        Assert.Equal(testValue, expectedResult);
//    }

//    #endregion
//}

