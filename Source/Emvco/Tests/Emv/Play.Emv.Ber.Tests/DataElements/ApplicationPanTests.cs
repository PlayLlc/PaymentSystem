using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
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
}
