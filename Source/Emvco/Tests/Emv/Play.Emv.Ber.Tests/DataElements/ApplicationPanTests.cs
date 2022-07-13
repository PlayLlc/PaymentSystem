using System;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ApplicationPanTests : TestBase
{
    #region Instance Members

    private readonly BerCodec _BerCodec = new(EmvCodec.Configuration);

    #endregion

    [Fact]
    public void BerEncoding_DeserializingDataElement_CreatesPrimitiveValue()
    {
        ApplicationPanTestTlv testData = new();
        ApplicationPan testValue = ApplicationPan.Decode(testData.EncodeValue().AsSpan());
        Assert.NotNull(testValue);
    }

    [Fact]
    public void BerEncoding_EncodingDataElement_SerializesExpectedValue()
    {
        ApplicationPanTestTlv testData = new();
        ApplicationPan sut = ApplicationPan.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeValue();
        byte[]? testValue = sut.EncodeValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingDataElementTlv_SerializesExpectedValue()
    {
        ApplicationPanTestTlv testData = new();
        ApplicationPan sut = ApplicationPan.Decode(testData.EncodeValue().AsSpan());
        byte[] expectedResult = testData.EncodeTagLengthValue();
        byte[]? testValue = sut.EncodeTagLengthValue();

        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        ApplicationPanTestTlv testData = new();
        ApplicationPan sut = ApplicationPan.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ApplicationPan.Tag, testData.EncodeValue());
        Assert.Equal(testValue, expectedResult);
    }

    [Fact]
    public void ApplicationPan_AsDataStorageId_ReturnsExpectedResult()
    {
        ApplicationPanTestTlv testData = new();
        ApplicationPan sut = ApplicationPan.Decode(testData.EncodeValue().AsSpan());

        Span<byte> expectedDataStorageContentOctets = new byte[8];
        ApplicationPanTestTlv._DefaultContent.Encode().CopyTo(expectedDataStorageContentOctets);

        DataStorageId expected = new DataStorageId(new BigInteger(expectedDataStorageContentOctets));
        DataStorageId actual = sut.AsDataStorageId(null);

        Assert.Equal(expected.EncodeValue(), actual.EncodeValue());
    }

    [Fact]
    public void ApplicationPan_AsDataStorageIdWithGivenApplicationPanSequenceNumber_ReturnsExpectedResult()
    {
        ApplicationPanSequenceNumber sequenceNumber = new ApplicationPanSequenceNumber(1);
        ApplicationPanTestTlv testData = new();
        ApplicationPan sut = ApplicationPan.Decode(testData.EncodeValue().AsSpan());

        Span<byte> expectedDataStorageContentOctets = new byte[8];
        ApplicationPanTestTlv._DefaultContent.Encode().CopyTo(expectedDataStorageContentOctets);
        expectedDataStorageContentOctets[^1] = (byte)1;

        DataStorageId expected = new DataStorageId(new BigInteger(expectedDataStorageContentOctets));
        DataStorageId actual = sut.AsDataStorageId(sequenceNumber);

        Assert.Equal(expected.EncodeValue(), actual.EncodeValue());
    }

    [Fact]
    public void ApplicationPan_AsDataStorageIdWithLeftPaddingRemovalOfInputAndPANSequenceNumber_ReturnsExpectedResult()
    {
        ApplicationPanSequenceNumber sequenceNumber = new ApplicationPanSequenceNumber(3);

        Nibble[] testData = new Nibble[] {0xF, 0xF, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x0, 0x1, 0x2, 0x3 };
        ApplicationPan sut = new ApplicationPan(new TrackPrimaryAccountNumber(testData));

        byte[] expectedPANByteContentsForDataStorageId = new byte[] { 18, 52, 86, 120, 144, 18, 48, 3 };
        DataStorageId expected = new DataStorageId(new BigInteger(expectedPANByteContentsForDataStorageId));
        DataStorageId actual = sut.AsDataStorageId(sequenceNumber);

        Assert.Equal(expected.EncodeValue(), actual.EncodeValue());
    }

    [Fact]
    public void ApplicationPan_IsIssuerIdentifierMatching_ReturnsTrue()
    {
        byte[] contentOctets = { 18, 52, 86, 120, 144, 18, 48, 3 };
        ApplicationPan sut = ApplicationPan.Decode(contentOctets.AsSpan());

        IssuerIdentificationNumber identificationNumber = new IssuerIdentificationNumber(184803);

        bool actual = sut.IsIssuerIdentifierMatching(identificationNumber);

        Assert.True(actual);
    }

    [Fact]
    public void ApplicationPan_IsIssuerIdentifierMatching_ReturnsFalse()
    {
        byte[] contentOctets = { 18, 52, 86, 120, 144, 18, 48, 3 };
        ApplicationPan sut = ApplicationPan.Decode(contentOctets.AsSpan());

        IssuerIdentificationNumber identificationNumber = new IssuerIdentificationNumber(141848);

        bool actual = sut.IsIssuerIdentifierMatching(identificationNumber);

        Assert.False(actual);
    }
}
