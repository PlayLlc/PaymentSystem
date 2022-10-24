using System;

using Play.Ber.DataObjects;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Primitive.Acquirer;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class AuthorizationResponseCodeTests : TestBase
{
    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        AuthorizationResponseCodeTestTlv testData = new();
        AuthorizationResponseCode sut = AuthorizationResponseCode.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(sut);
    }

    [Fact]
    public void BerEncoding_DeserializingDataElementWithInvalidLength_ThrowsDataElementParsingExceptionError()
    {
        AuthorizationResponseCodeTestTlv testData = new(new byte[] { 49, 50, 51 });

        Assert.Throws<DataElementParsingException>(() =>
        {
            AuthorizationResponseCode sut = AuthorizationResponseCode.Decode(testData.EncodeValue().AsSpan());
        });
    }

    [Fact]
    public void BerEncoding_DeserializingDataElementWithInvalidChars_ThrowsCodecExceptionError()
    {
        AuthorizationResponseCodeTestTlv testData = new(new byte[] { 21, 22 });

        Assert.Throws<CodecParsingException>(() =>
        {
            AuthorizationResponseCode sut = AuthorizationResponseCode.Decode(testData.EncodeValue().AsSpan());
        });
    }

    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        AuthorizationResponseCodeTestTlv testData = new();
        AuthorizationResponseCode sut = AuthorizationResponseCode.Decode(testData.EncodeValue().AsSpan());
        byte[] expected = testData.EncodeValue();
        byte[]? actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        AuthorizationResponseCodeTestTlv testData = new();
        AuthorizationResponseCode sut = AuthorizationResponseCode.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        AuthorizationResponseCodeTestTlv testData = new();
        AuthorizationResponseCode sut = AuthorizationResponseCode.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(AuthorizationResponseCode.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void AuthorizationResponseCode_InitializedFromSameContentOctets_IsEqual()
    {
        char[] contentOctetsChars = new char[] { '1', '2' };
        byte[] contentOctets = new byte[] { 49, 50 };

        AuthorizationResponseCode sut = new AuthorizationResponseCode(contentOctetsChars);
        AuthorizationResponseCode sut2 = AuthorizationResponseCode.Decode(contentOctets.AsSpan());

        byte[] actual = sut.EncodeValue();

        Assert.Equal(contentOctets, actual);
    }
}
