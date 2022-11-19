using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Icc.Messaging.Apdu;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class ErrorIndicationTests
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
        ErrorIndicationTestTlv testData = new();
        ErrorIndication testValue = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
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
        ErrorIndicationTestTlv testData = new();
        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
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
        ErrorIndicationTestTlv testData = new();
        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
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
        ErrorIndicationTestTlv testData = new();
        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(ErrorIndication.Tag, testData.EncodeValue());
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
        ErrorIndicationTestTlv testData = new();
        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());

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
        ErrorIndicationTestTlv testData = new(new byte[] {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01});

        Assert.Throws<DataElementParsingException>(() => ErrorIndication.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        ErrorIndicationTestTlv testData = new();
        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
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
        ErrorIndicationTestTlv testData = new();
        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
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
        ErrorIndicationTestTlv testData = new(new byte[] {0x08, 0x12, 0x9F, 0x15, 0x28, 0x3E});
        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
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
        ErrorIndicationTestTlv testData = new(new byte[] {0x08, 0x12, 0x9F, 0x15, 0x28, 0x3E});

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region ErrorIndication

    [Fact]
    public void ErrorIndication_GetL1_ReturnsOk()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            0x00, //L1Ok
            0x12, 0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Level1Error actual = sut.GetL1();

        Assert.Equal(Level1Error.Ok, actual);
    }

    [Fact]
    public void ErrorIndication_GetL1_ReturnsEmpty()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            0x08, //L1Ok
            0x12, 0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Level1Error actual = sut.GetL1();

        Assert.Equal(0, actual);
    }

    [Fact]
    public void ErrorIndication_GetL2_ReturnsOk()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            0x00, //L2Ok
            0x12,
            0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Level2Error actual = sut.GetL2();

        Assert.Equal(Level2Error.Ok, actual);
    }

    [Fact]
    public void ErrorIndication_GetL2_ReturnsNoPpse()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            0x08, //L2 unkown
            0x12, 0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Level2Error actual = sut.GetL2();

        Assert.Equal(Level2Error.NoPpse, actual);
    }

    [Fact]
    public void ErrorIndication_GetL2_ReturnsEmpty()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            22, //L2 unkown
            0x12, 0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Level2Error actual = sut.GetL2();

        Assert.Equal(0, actual);
    }

    [Fact]
    public void ErrorIndication_GetL3_ReturnsOk()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            0, //L3 ok
            0x12, 0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Level3Error actual = sut.GetL3();

        Assert.Equal(Level3Error.Ok, actual);
    }

    [Fact]
    public void ErrorIndication_GetL3_ReturnsStop()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            2, //L3 ok
            0x12, 0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Level3Error actual = sut.GetL3();

        Assert.Equal(Level3Error.Stop, actual);
    }

    [Fact]
    public void ErrorIndication_GetL3_ReturnsEmpty()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            16, //L3 ok
            0x12, 0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Level3Error actual = sut.GetL3();

        Assert.Equal(0, actual);
    }

    [Fact]
    public void ErrorIndication_IsErrorPresent_ReturnsTrue()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            8, //L3 ok
            0x12, 0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Assert.True(sut.IsErrorPresent());
    }

    [Fact]
    public void ErrorIndication_IsErrorPresent_ReturnsFalse()
    {
        ErrorIndicationTestTlv testData = new(new byte[]
        {
            0, //L3 ok
            0x12, 0x9F, 0x15, 0x28, 0x3E
        });

        ErrorIndication sut = ErrorIndication.Decode(testData.EncodeValue().AsSpan());
        Assert.False(sut.IsErrorPresent());
    }

    #endregion

    #region Builder

    [Fact]
    public void ErrrorIndicationBuilder_Instantiate_BuilderInstantiated()
    {
        ErrorIndication.Builder builder = ErrorIndication.GetBuilder();

        Assert.NotNull(builder);
    }

    [Fact]
    public void ErrorIndicationBuilder_Reset_ReturnsExpectedResult()
    {
        ErrorIndication.Builder builder = ErrorIndication.GetBuilder();

        builder.Reset(ErrorIndication.Default);
        Assert.Equal(ErrorIndication.Default, builder.Complete());
    }

    [Fact]
    public void ErrorIndicationBuilder_SetLevel1Error_ReturnsExpectedResult()
    {
        ErrorIndication.Builder builder = ErrorIndication.GetBuilder();
        byte[] testValue = { 0b11, 0, 0, 0, 0, 0 };
        ErrorIndication expected = ErrorIndication.Decode(testValue.AsSpan());

        builder.Set(Level1Error.ProtocolError);
        Assert.Equal(expected, builder.Complete());
    }

    [Fact]
    public void ErrorIndicationBuilder_SetLevel2Error_ReturnsExpectedResult()
    {
        ErrorIndication.Builder builder = ErrorIndication.GetBuilder();
        byte[] testValue = { 0, 6, 0, 0, 0, 0 };
        ErrorIndication expected = ErrorIndication.Decode(testValue.AsSpan());

        builder.Set(Level2Error.CardDataError);
        Assert.Equal(expected, builder.Complete());
    }

    [Fact]
    public void ErrorIndicationBuilder_SetLevel3Error_ReturnsExpectedResult()
    {
        ErrorIndication.Builder builder = ErrorIndication.GetBuilder();
        byte[] testValue = { 0, 0, 0, 0, 0, 48 };
        ErrorIndication expected = ErrorIndication.Decode(testValue.AsSpan());

        builder.Set(Level3Error.AmountNotPresent);
        Assert.Equal(expected, builder.Complete());
    }

    [Fact]
    public void ErrorIndicationBuilder_SetStatusWords_ReturnsExpectedResult()
    {
        ErrorIndication.Builder builder = ErrorIndication.GetBuilder();
        byte[] testValue = { 0, 0, 0, 0x6A, 0x88, 0 };
        ErrorIndication expected = ErrorIndication.Decode(testValue.AsSpan());

        builder.Set(StatusWords._6A88);
        Assert.Equal(expected, builder.Complete());
    }


    #endregion
}