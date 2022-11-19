﻿using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Encryption.Certificates;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class IccPublicKeyExponentTests
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
        IccPublicKeyExponentTestTlv testData = new();
        IccPublicKeyExponent testValue = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());
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
        IccPublicKeyExponentTestTlv testData = new();
        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(expectedResult, testValue);
    }

    /// <summary>
    ///     BerEncoding_EncodingDataElementTlv_SerializesExpectedValue
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        IccPublicKeyExponentTestTlv testData = new();
        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());
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
        IccPublicKeyExponentTestTlv testData = new();
        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(IccPublicKeyExponent.Tag, testData.EncodeValue());
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
        IccPublicKeyExponentTestTlv testData = new();
        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());

        byte[] testValue = sut.AsTagLengthValue().EncodeTagLengthValue();
        byte[] expectedResult = testData.EncodeTagLengthValue();
        Assert.Equal(testValue, expectedResult);
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        IccPublicKeyExponentTestTlv testData = new();
        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());
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
        IccPublicKeyExponentTestTlv testData = new();
        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());
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
        IccPublicKeyExponentTestTlv testData = new(new byte[] {0xe3, 0x8f});
        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());
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
        IccPublicKeyExponentTestTlv testData = new(new byte[] {0x4d, 0x2c});

        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    [Fact]
    public void InvalidBerEncoding_Decoding_ThrowsException()
    {
        IccPublicKeyExponentTestTlv testData = new(new byte[] {13, 55, 77, 84, 134});

        Assert.Throws<DataElementParsingException>(() => IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan()));
    }

    #endregion

    #region IccPublicKeyExponent

    [Fact]
    public void IccPublicKeyExponent_AsPublicKeyExponent_ReturnsExpectedResult()
    {
        IccPublicKeyExponentTestTlv testData = new(new byte[] {3});

        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());

        PublicKeyExponents actual = sut.AsPublicKeyExponent();

        Assert.Equal(PublicKeyExponents._3, actual);
    }

    [Fact]
    public void IccPublicKeyExponent_AsPublicKeyExponent_Throws()
    {
        IccPublicKeyExponentTestTlv testData = new(new byte[] {3, 33});

        IccPublicKeyExponent sut = IccPublicKeyExponent.Decode(testData.EncodeValue().AsSpan());

        Assert.Throws<InvalidOperationException>(() => sut.AsPublicKeyExponent());
    }

    #endregion
}