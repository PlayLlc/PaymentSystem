using System;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Constructed;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.ResponseMessages;

public class ResponseMessageTemplateFormat2Tests
{
    #region Instance values

    private readonly EmvCodec _Codec = EmvCodec.GetCodec();

    #endregion

    [Fact]
    public void ConstructedValue_Deserializing_CreatesResponseMessageTemplate()
    {
        AmountAuthorizedNumericTestTlv testData = new();
        ResponseMessageTemplateFormat2 sut = ResponseMessageTemplateFormat2.Decode(_Codec, testData.EncodeTagLengthValue());
        Assert.NotNull(sut);
    }

    [Fact]
    public void ConstructedValue_DeserializingMultipleTlvsInput_CreatesResponseMessageTemplate()
    {
        ResponseMessageTemplateFormat2TestTlv testData = new();
        ResponseMessageTemplateFormat2 sut = ResponseMessageTemplateFormat2.Decode(_Codec, testData.EncodeValue().AsMemory());
        byte[] expected = testData.EncodeValue();
        byte[] actual = sut.EncodeValue(_Codec);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConstructedValue_DeserializingTagLengthValues_ReturnsExpectedResult()
    {
        ResponseMessageTemplateFormat2TestTlv testData = new();

        TagLengthValue[] expected = new TagLengthValue[]
        {
            new AdditionalTerminalCapabilitiesTestTlv().AsTagLengthValue(),
            new ApplicationCryptogramTestTlv().AsTagLengthValue(),
            new CvmResultsTestTlv().AsTagLengthValue()
        };

        TagLengthValue[] actual = ResponseMessageTemplateFormat2.DecodeValue(_Codec, testData.EncodeValue());
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void InvalidTlv_DeserializingConstructedValue_ThrowsArgumentOutOfRangeExceptionWhenTryingToDeserialize()
    {
        byte[] invalidTlv = { 0x01, 0x07, 0x02 };

        Assert.Throws<ArgumentOutOfRangeException>(() => ResponseMessageTemplateFormat2.Decode(_Codec, invalidTlv.AsMemory()));
    }

    [Fact]
    public void InitializedConstructedValue_GetValueByteCount_ReturnsExpectedResult()
    {
        ResponseMessageTemplateFormat2TestTlv testData = new();
        ResponseMessageTemplateFormat2 sut = ResponseMessageTemplateFormat2.Decode(_Codec, testData.EncodeValue().AsMemory());

        int expected = testData.GetValueByteCount();
        int actual = sut.GetValueByteCount(_Codec);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConstructedValue_AsTagLengthValue_ReturnsExpectedResult()
    {
        ResponseMessageTemplateFormat2TestTlv testData = new();
        ResponseMessageTemplateFormat2 sut = ResponseMessageTemplateFormat2.Decode(_Codec, testData.EncodeValue().AsMemory());

        TagLengthValue expected = testData.AsTagLengthValue();
        TagLengthValue actual = sut.AsTagLengthValue(_Codec);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CustomConstructedValue_EncodeValue_ReturnsExpectedResult()
    {
        TestTlv[] children =
        {
            new AdditionalTerminalCapabilitiesTestTlv(),
            new ApplicationCryptogramTestTlv(),
            new CvmResultsTestTlv(),
            new ErrorIndicationTestTlv(),
            new CardholderNameTestTlv(),
        };

        Tag[] childrenIndex = { AdditionalTerminalCapabilities.Tag, ApplicationCryptogram.Tag, CvmResults.Tag, ErrorIndication.Tag, CardholderName.Tag };

        ResponseMessageTemplateFormat2TestTlv testData = new(childrenIndex, children);
        ResponseMessageTemplateFormat2 sut = ResponseMessageTemplateFormat2.Decode(_Codec, testData.EncodeValue().AsMemory());
        byte[] expected = testData.EncodeValue();
        byte[] actual = sut.EncodeValue(_Codec);

        Assert.Equal(expected, actual);
    }
}
