using System;

using Play.Codecs.Exceptions;
using Play.Emv.TestData.Icc.Apdu;

using Xunit;

namespace Play.Codecs.Tests.Hexadecimal;

public class HexadecimalTests
{
    #region Instance Values

    private readonly Strings.Hexadecimal _SystemUnderTest;

    #endregion

    #region Constructor

    public HexadecimalTests()
    {
        _SystemUnderTest = PlayEncoding.Hexadecimal;
    }

    #endregion

    #region Instance Members

    [Theory]
    [MemberData(nameof(HexadecimalFixture.GetRandomBytes), new object[] {100, 1, 300}, MemberType = typeof(HexadecimalFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.GetString(testValue);
        byte[] encoded = _SystemUnderTest.GetBytes(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(HexadecimalFixture.GetRandomString), new object[] {100, 1, 300}, MemberType = typeof(HexadecimalFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
    {
        byte[] decoded = _SystemUnderTest.GetBytes(testValue);
        string encoded = _SystemUnderTest.GetString(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Fact]
    public void SelectPpseRApduByteArray_ConvertingToHexadecimalString_ReturnsExpectedResult()
    {
        byte[] testData = ApduTestData.RApdu.Select.Ppse.PpseBytes;

        string result = _SystemUnderTest.GetString(testData);

        Assert.Equal(result, ApduTestData.RApdu.Select.Ppse.PpseHex);
    }

    [Fact]
    public void SelectPpseCApduByteArray_ConvertingToHexadecimalString_ReturnsExpectedResult()
    {
        byte[] testData = ApduTestData.CApdu.Select.Ppse.PpseBytes;

        string result = _SystemUnderTest.GetString(testData);

        Assert.Equal(result, ApduTestData.CApdu.Select.Ppse.PpseHex);
    }

    [Fact]
    public void SelectPpseRApduHexString_ConvertingToByteArray_ReturnsExpectedResult()
    {
        string testData = ApduTestData.RApdu.Select.Ppse.PpseHex;

        byte[] result = _SystemUnderTest.GetBytes(testData);

        Assert.Equal(result, ApduTestData.RApdu.Select.Ppse.PpseBytes);
    }

    [Fact]
    public void SelectPpseCApduHexString_ConvertingToByteArray_ReturnsExpectedResult()
    {
        string testData = ApduTestData.CApdu.Select.Ppse.PpseHex;

        byte[] result = _SystemUnderTest.GetBytes(testData);

        Assert.Equal(result, ApduTestData.CApdu.Select.Ppse.PpseBytes);
    }

    [Fact]
    public void SelectPpseRApduHexString_GettingByteCount_ReturnsExpectedResult()
    {
        string testData = ApduTestData.RApdu.Select.Ppse.PpseHex;

        int result = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(result, ApduTestData.RApdu.Select.Ppse.PpseBytes.Length);
    }

    [Fact]
    public void SelectPpseCApduHexString_GettingByteCount_ReturnsExpectedResult()
    {
        string testData = ApduTestData.CApdu.Select.Ppse.PpseHex;

        int result = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(result, ApduTestData.CApdu.Select.Ppse.PpseBytes.Length);
    }

    [Fact]
    public void GivenAsciiString_GetBytes_ThrowsEncodingException()
    {
        string testData = "Hello how are you people?";

        Assert.Throws<EncodingException>(() => PlayEncoding.Hexadecimal.GetBytes(testData));
    }

    [Fact]
    public void GivenInvalidHexadecimalString_GetBytes_ThrowsEncodingException()
    {
        const string testData = "FFC•3C01CD6E4F?A13021";

        Assert.Throws<EncodingException>(() => PlayEncoding.Hexadecimal.GetBytes(testData));
    }

    [Fact]
    public void GivenNullByteArray_GetString_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PlayEncoding.Hexadecimal.GetString(null));
    }

    [Fact]
    public void GivenValidHexadecimalByteArray_GetString_ReturnsExpectedString()
    {
        byte[] testData = {0xFF, 0x35, 0xA3, 0xBC};

        string expected = "FF35A3BC";
        string result = PlayEncoding.Hexadecimal.GetString(testData);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GivenValidHexadecimalString_GetBytes_ReturnsExpectedByteArray()
    {
        string testData = "DFFC3C01CD6E4FA13021";

        byte[] expected =
        {
            0xDF, 0xFC, 0x3C, 0x01, 0xCD, 0x6E, 0x4F, 0xA1,
            0x30, 0x21
        };
        byte[] result = PlayEncoding.Hexadecimal.GetBytes(testData);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GivenValidHexadecimalString_GetBytes_ReturnsExpectedBytes()
    {
        string testData = "F37DCA34EA";

        //new string(GetRandomHexChars(_Random, _Random.Next(1, 50)));

        byte[] expected = {0xF3, 0x7D, 0xCA, 0x34, 0xEA};

        byte[] result = PlayEncoding.Hexadecimal.GetBytes(testData);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GivenValidHexadecimalStringWithOddLength_GetBytes_ReturnsExpectedByteArray()
    {
        string testData = "FFC3C01CD6E4FA13021";

        byte[] expected =
        {
            0x0F, 0xFC, 0x3C, 0x01, 0xCD, 0x6E, 0x4F, 0xA1,
            0x30, 0x21
        };
        byte[] result = PlayEncoding.Hexadecimal.GetBytes(testData);

        Assert.Equal(expected, result);
    }

    #endregion
}