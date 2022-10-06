using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class UserInterfaceRequestDataTests
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
        UserInterfaceRequestDataTestTlv testData = new();
        UserInterfaceRequestData testValue = UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan());
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
        UserInterfaceRequestDataTestTlv testData = new();
        UserInterfaceRequestData sut = UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan());
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
        UserInterfaceRequestDataTestTlv testData = new();
        UserInterfaceRequestData sut = UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan());
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
        UserInterfaceRequestDataTestTlv testData = new();
        UserInterfaceRequestData sut = UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue? testValue = sut.AsTagLengthValue();
        TagLengthValue expectedResult = new(UserInterfaceRequestData.Tag, testData.EncodeValue());
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
        UserInterfaceRequestDataTestTlv testData = new();
        UserInterfaceRequestData sut = UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan());

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
        UserInterfaceRequestDataTestTlv testData = new(new byte[] { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01 });

        Assert.Throws<DataElementParsingException>(() => UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan()));
    }

    /// <summary>
    ///     DataElement_InvokingGetValueByteCount_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    [Fact]
    public void DataElement_InvokingGetValueByteCount_ReturnsExpectedResult()
    {
        UserInterfaceRequestDataTestTlv testData = new();
        UserInterfaceRequestData sut = UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan());
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
        UserInterfaceRequestDataTestTlv testData = new();
        UserInterfaceRequestData sut = UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan());
        int expectedResult = testData.GetTagLengthValueByteCount();
        ushort testResult = sut.GetTagLengthValueByteCount();

        Assert.Equal(expectedResult, testResult);
    }

    #endregion

    #region UserInterfaceRequestData

    [Fact]
    public void UserInterfaceRequestData_IsValueQualifierPresent_ReturnsFalse()
    {
        UserInterfaceRequestDataTestTlv testData = new();
        UserInterfaceRequestData sut = UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsValueQualifierPresent());
    }

    [Fact]
    public void UserInterfaceRequestData_IsValueQualifierPresent_ReturnsTrue()
    {
        UserInterfaceRequestDataTestTlv testData = new();
        UserInterfaceRequestData sut = UserInterfaceRequestData.Decode(testData.EncodeValue().AsSpan());

        Assert.False(sut.IsValueQualifierPresent());
    }

    #endregion

    #region Builder

    [Fact]
    public void UserInterfaceRequestDataBuilder_Instantiate_BuilderInstantiated()
    {
        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        Assert.NotNull(builder);
    }

    [Fact]
    public void UserInterfaceRequestDataBuilder_Reset_ReturnsExpectedResult()
    {
        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        UserInterfaceRequestDataTestTlv testValue = new();
        UserInterfaceRequestData expected = UserInterfaceRequestData.Decode(testValue.EncodeValue().AsSpan());

        builder.Reset(expected);
        UserInterfaceRequestData actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UserInterfaceRequestDataBuilder_SetDisplayMessageIdentifiers_ReturnsExpectedResult()
    {
        byte[] testData = Enumerable.Range(0, 22).Select(x => (byte) 0).ToArray();
        testData[20] = 2;

        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        UserInterfaceRequestData expected = UserInterfaceRequestData.Decode(testData.AsSpan());

        builder.Set(DisplayMessageIdentifiers.AmountOk);

        UserInterfaceRequestData actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UserInterfaceRequestDataBuilder_SetDisplayStatuses_ReturnsExpectedResult()
    {
        byte[] testData = Enumerable.Range(0, 22).Select(x => (byte)0).ToArray();
        testData[19] = 1;

        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        UserInterfaceRequestData expected = UserInterfaceRequestData.Decode(testData.AsSpan());

        builder.Set(DisplayStatuses.CardReadSuccessful);

        UserInterfaceRequestData actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UserInterfaceRequestDataBuilder_SetMessageHoldTime_ReturnsExpectedResult()
    {
        byte[] testData = Enumerable.Range(0, 22).Select(x => (byte)0).ToArray();
        //testData[19] = 1;

        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        UserInterfaceRequestData expected = UserInterfaceRequestData.Decode(testData.AsSpan());

        builder.Set(MessageHoldTime.MinimumValue);

        UserInterfaceRequestData actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UserInterfaceRequestDataBuilder_SetLanguagePreference_ReturnsExpectedResult()
    {
        byte[] testData = Enumerable.Range(0, 22).Select(x => (byte)0).ToArray();
        //(byte)'R', (byte)'O', (byte)'U', (byte)'S'
        testData[8] = (byte)'R';
        testData[9] = (byte)'O';
        testData[10] = (byte)'U';
        testData[11] = (byte)'S';

        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        UserInterfaceRequestData expected = UserInterfaceRequestData.Decode(testData.AsSpan());

        LanguagePreference language = LanguagePreference.Decode(new LanguagePreferenceTestTlv().EncodeValue().AsSpan());

        builder.Set(language);

        UserInterfaceRequestData actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UserInterfaceRequestDataBuilder_SetValueQualifiers_ReturnsExpectedResult()
    {
        byte[] testData = Enumerable.Range(0, 22).Select(x => (byte)0).ToArray();
        testData[7] = 32;

        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        UserInterfaceRequestData expected = UserInterfaceRequestData.Decode(testData.AsSpan());

        builder.Set(ValueQualifiers.Balance);

        UserInterfaceRequestData actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UserInterfaceRequestDataBuilder_SetMoney_ReturnsExpectedResult()
    {
        byte[] testData = Enumerable.Range(0, 22).Select(x => (byte)0).ToArray();
        testData[1] = 125;

        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        UserInterfaceRequestData expected = UserInterfaceRequestData.Decode(testData.AsSpan());

        Money money = new Money(125, new Alpha3CurrencyCode(new char[] { 'U', 'S', 'D' }.AsSpan()));
        builder.Set(money);

        UserInterfaceRequestData actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UserInterfaceRequestDataBuilder_SetNumericCurrencyCode_ReturnsExpectedResult()
    {
        byte[] testData = Enumerable.Range(0, 22).Select(x => (byte)0).ToArray();
        testData[0] = 0b01001000;
        testData[1] = 0b11;

        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        UserInterfaceRequestData expected = UserInterfaceRequestData.Decode(testData.AsSpan());

        NumericCurrencyCode numericCurrencyCode = new(840);
        builder.Set(numericCurrencyCode);

        UserInterfaceRequestData actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UserInterfaceRequestDataBuilder_SetMoneyAndValueQualifiers_ReturnsExpectedResult()
    {
        byte[] testData = Enumerable.Range(0, 22).Select(x => (byte)0).ToArray();
        testData[1] = 125;
        testData[7] = 32;

        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

        UserInterfaceRequestData expected = UserInterfaceRequestData.Decode(testData.AsSpan());

        Money money = new Money(125, new Alpha3CurrencyCode(new char[] { 'U', 'S', 'D' }.AsSpan()));

        builder.Set(money);
        builder.Set(ValueQualifiers.Balance);

        UserInterfaceRequestData actual = builder.Complete();
        Assert.Equal(expected, actual);
    }

    #endregion
}
