using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;
public class AccountTypeTests : TestBase
{
    [Fact]
    public void PlayCodec_Encode_ReturnsExpectedResult()
    {
        AccountTypeTestTlv testData = new AccountTypeTestTlv(0x00);
        AccountType sut = AccountType.Decode(testData.EncodeValue().AsSpan());

        byte[] expected = testData.EncodeValue();
        byte[] actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_EncodeDefaultAccount_ReturnsExpectedResult()
    {
        AccountTypeTestTlv testData = new AccountTypeTestTlv(0);
        AccountType sut = AccountType.Default;

        byte[] expected = testData.EncodeValue();
        byte[] actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_EncodeSavingsAccount_ReturnsExpectedResult()
    {
        AccountTypeTestTlv testData = new AccountTypeTestTlv(10);
        AccountType sut = AccountType.Savings;

        byte[] expected = testData.EncodeValue();
        byte[] actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_EncodeDebitAccount_ReturnsExpectedResult()
    {
        AccountTypeTestTlv testData = new AccountTypeTestTlv(20);
        AccountType sut = AccountType.Checking;

        byte[] expected = testData.EncodeValue();
        byte[] actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_EncodeCreditAccount_ReturnsExpectedResult()
    {
        AccountTypeTestTlv testData = new AccountTypeTestTlv(30);
        AccountType sut = AccountType.Credit;

        byte[] expected = testData.EncodeValue();
        byte[] actual = sut.EncodeValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BerCodec_EncodeWithMoreThen1Byte_ThrowsException()
    {
        AccountTypeTestTlv testData = new AccountTypeTestTlv(new byte[]  { 1, 2 });

        Assert.Throws<DataElementParsingException>(() =>
        {
            AccountType sut = AccountType.Decode(testData.EncodeValue().AsSpan());
        });
    }

    [Fact]
    public void BerEncoding_EncodingToTagLengthValue_SerializesExpectedValue()
    {
        AccountTypeTestTlv testData = new(20);
        AccountType sut = AccountType.Decode(testData.EncodeValue().AsSpan());
        TagLengthValue expected = new(AccountType.Tag, testData.EncodeValue());
        TagLengthValue? actual = sut.AsTagLengthValue();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void TagLengthValue_SerializingToBer_ReturnsExpectedResult()
    {
        AccountTypeTestTlv testData = new(10);
        AccountType sut = AccountType.Decode(testData.EncodeValue().AsSpan());
        byte[] expected = testData.EncodeTagLengthValue();
        byte[] actual = sut.AsTagLengthValue().EncodeTagLengthValue();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void AccountType_InitializedFromSameContentOctets_IsEqual()
    {
        AccountTypeTestTlv testData = new(10);
        AccountType sut = AccountType.Decode(testData.EncodeValue().AsSpan());

        Assert.Equal(sut, AccountType.Savings);
    }
}
