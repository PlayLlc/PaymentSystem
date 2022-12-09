﻿using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class CardRiskManagementDataObjectList1RelatedDataTests
{
    #region Instance Values

    private readonly BerCodec _Bercodec = new(EmvCodec.Configuration);

    #endregion

    #region Instance Members

    /// <summary>
    ///     BerEncoding_DeserializingDataElement_CreatesPrimitiveValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        CardRiskManagementDataObjectList1RelatedDataTestTlv testData = new();
        CardRiskManagementDataObjectList1RelatedData testValue = CardRiskManagementDataObjectList1RelatedData.Decode(testData.EncodeValue().AsSpan());

        Assert.NotNull(testValue);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElement_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        CardRiskManagementDataObjectList1RelatedDataTestTlv testData = new();
        CardRiskManagementDataObjectList1RelatedData sut = CardRiskManagementDataObjectList1RelatedData.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? actual = sut.EncodeValue();

        Assert.Equal(expectedResult, actual);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        CardRiskManagementDataObjectList1RelatedDataTestTlv testData = new();
        CardRiskManagementDataObjectList1RelatedData sut = CardRiskManagementDataObjectList1RelatedData.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? actual = sut.EncodeTagLengthValue();

        Assert.Equal(expectedResult, actual);
    }

    [Fact]
    public void BerEncoding_EncodeCardRiskManagementDataObjectList1_SerializesExpectedValue()
    {
        CardRiskManagementDataObjectList1RelatedDataTestTlv testData = new();

        CardRiskManagementDataObjectList1RelatedData expectedDol = CardRiskManagementDataObjectList1RelatedData.Decode(testData.EncodeValue().AsSpan());
        CardRiskManagementDataObjectList1RelatedData sut = CardRiskManagementDataObjectList1RelatedData.Decode(testData.EncodeValue().AsSpan());

        byte[] expected = expectedDol.EncodeValue();
        byte[] actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    /// <summary>
    ///     BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        CardRiskManagementDataObjectList1RelatedDataTestTlv testData = new();
        CardRiskManagementDataObjectList1RelatedData sut = CardRiskManagementDataObjectList1RelatedData.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(CardRiskManagementDataObjectList1RelatedData.Tag, testData.EncodeValue());
        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     TagLengthValue_SerializingToBer_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        CardRiskManagementDataObjectList1RelatedDataTestTlv testData = new();

        CardRiskManagementDataObjectList1RelatedData sut = CardRiskManagementDataObjectList1RelatedData.Decode(testData.EncodeValue().AsSpan());
        byte[]? encoded = sut.EncodeValue();
        TagLengthValue? tlv = sut.AsTagLengthValue();
        byte[]? tlvRaw = tlv.EncodeTagLengthValue();
        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        TagLengthValue? expectedResultTlv = new(testData.GetTag(), testData.EncodeValue());

        Assert.Equal(testValue, expectedResult);
    }

    #endregion
}