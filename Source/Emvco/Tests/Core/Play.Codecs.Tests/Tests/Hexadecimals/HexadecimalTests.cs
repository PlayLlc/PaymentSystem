using System;

using Play.Codecs.Exceptions;
using Play.Testing.BaseTestClasses;
using Play.Testing.Icc.Apdu;

using Xunit;

namespace Play.Codecs.Tests.Tests.Hexadecimals;

public class HexadecimalTests : TestBase
{
    #region Instance Values

    private readonly HexadecimalCodec _SystemUnderTest;

    #endregion

    #region Constructor

    public HexadecimalTests()
    {
        _SystemUnderTest = PlayCodec.HexadecimalCodec;
    }

    #endregion

    #region Decode And Encode

    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(HexadecimalFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(HexadecimalFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpected(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);
        byte[] actual = _SystemUnderTest.Encode(decoded);
        Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
    }

    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(HexadecimalFixture.GetRandomString), 100, 1, 300, MemberType = typeof(HexadecimalFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpected(string testValue)
    {
        byte[] decoded = _SystemUnderTest.Encode(testValue);
        string actual = _SystemUnderTest.DecodeToString(decoded);
        Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
    }

    #endregion

    #region DecodeToString

    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void SelectPpseRApduByteArray_DecodeToString_ReturnsExpected()
    {
        string expected = ApduTestData.RApdu.Select.Ppse.PpseHex;
        string actual = _SystemUnderTest.DecodeToString(ApduTestData.RApdu.Select.Ppse.PpseBytes);

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void SelectPpseCApduByteArray_DecodeToString_ReturnsExpected()
    {
        string expected = ApduTestData.CApdu.Select.Ppse.PpseHex;
        string actual = _SystemUnderTest.DecodeToString(ApduTestData.CApdu.Select.Ppse.PpseBytes);

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void GivenNullByteArray_DecodeToString_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PlayCodec.HexadecimalCodec.DecodeToString(null));
    }

    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void GivenValidHexadecimalByteArray_DecodeToString_ReturnsExpectedString()
    {
        byte[] testData = {0xFF, 0x35, 0xA3, 0xBC};

        string expected = "FF35A3BC";
        string actual = PlayCodec.HexadecimalCodec.DecodeToString(testData);

        Assert.Equal(expected, actual);
    }

    #endregion

    #region Encode

    [Fact]
    public void GivenValidHexadecimalStringWithOddLength_Encode_ReturnsExpectedByteArray()
    {
        string testData = "FFC3C01CD6E4FA13021";

        byte[] expected = {0x0F, 0xFC, 0x3C, 0x01, 0xCD, 0x6E, 0x4F, 0xA1, 0x30, 0x21};
        byte[] actual = PlayCodec.HexadecimalCodec.Encode(testData);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectPpseRApduHexString_Encode_ReturnsExpected()
    {
        string testData = ApduTestData.RApdu.Select.Ppse.PpseHex;

        byte[] actual = _SystemUnderTest.Encode(testData);

        Assert.Equal(actual, ApduTestData.RApdu.Select.Ppse.PpseBytes);
    }

    [Fact]
    public void SelectPpseCApduHexString_Encode_ReturnsExpected()
    {
        string testData = ApduTestData.CApdu.Select.Ppse.PpseHex;

        byte[] actual = _SystemUnderTest.Encode(testData);

        Assert.Equal(actual, ApduTestData.CApdu.Select.Ppse.PpseBytes);
    }

    [Fact]
    public void GivenAsciiString_Encode_ThrowsEncodingException()
    {
        string testData = "Hello how are you people?";

        Assert.Throws<CodecParsingException>(() => PlayCodec.HexadecimalCodec.Encode(testData));
    }

    [Fact]
    public void GivenInvalidHexadecimalString_Encode_ThrowsEncodingException()
    {
        const string testData = "FFC•3C01CD6E4F?A13021";

        Assert.Throws<CodecParsingException>(() => PlayCodec.HexadecimalCodec.Encode(testData));
    }

    [Fact]
    public void GivenValidHexadecimalString_Encode_ReturnsExpectedByteArray()
    {
        string testData = "DFFC3C01CD6E4FA13021";

        byte[] expected = {0xDF, 0xFC, 0x3C, 0x01, 0xCD, 0x6E, 0x4F, 0xA1, 0x30, 0x21};
        byte[] actual = PlayCodec.HexadecimalCodec.Encode(testData);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GivenValidHexadecimalString_Encode_ReturnsExpectedBytes()
    {
        string testData = "F37DCA34EA";

        byte[] expected = {0xF3, 0x7D, 0xCA, 0x34, 0xEA};

        byte[] actual = PlayCodec.HexadecimalCodec.Encode(testData);

        Assert.Equal(expected, actual);
    }

    #endregion

    #region GetByteCount

    [Fact]
    public void SelectPpseRApduHexString_GetByteCount_ReturnsExpected()
    {
        string testData = ApduTestData.RApdu.Select.Ppse.PpseHex;

        int actual = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(actual, ApduTestData.RApdu.Select.Ppse.PpseBytes.Length);
    }

    [Fact]
    public void SelectPpseCApduHexString_GetByteCount_ReturnsExpected()
    {
        string testData = ApduTestData.CApdu.Select.Ppse.PpseHex;

        int actual = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(actual, ApduTestData.CApdu.Select.Ppse.PpseBytes.Length);
    }

    #endregion

    #region IsValid

    [Fact]
    public void ValidHexadecimalString_IsValid_ReturnsTrue()
    {
        string testData = ApduTestData.RApdu.Select.Ppse.PpseHex;

        Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void InvalidHexadecimalString_IsValid_ReturnsTrue()
    {
        string testData = "5hxfF03";

        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    #endregion
}