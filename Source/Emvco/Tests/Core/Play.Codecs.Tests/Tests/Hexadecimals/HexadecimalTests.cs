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

    #region Instance Members

    /// <summary>
    ///     RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedactual
    /// </summary>
    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(HexadecimalFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(HexadecimalFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedactual(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);
        byte[] actual = _SystemUnderTest.Encode(decoded);
        Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
    }

    /// <summary>
    ///     RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedactual
    /// </summary>
    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(HexadecimalFixture.GetRandomString), 100, 1, 300, MemberType = typeof(HexadecimalFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedactual(string testValue)
    {
        byte[] decoded = _SystemUnderTest.Encode(testValue);
        string actual = _SystemUnderTest.DecodeToString(decoded);
        Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
    }

    /// <summary>
    ///     SelectPpseRApduByteArray_ConvertingToHexadecimalString_ReturnsExpectedactual
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void SelectPpseRApduByteArray_ConvertingToHexadecimalString_ReturnsExpectedactual()
    {
        string expected = ApduTestData.RApdu.Select.Ppse.PpseHex;
        string actual = _SystemUnderTest.DecodeToString(ApduTestData.RApdu.Select.Ppse.PpseBytes);

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     SelectPpseCApduByteArray_ConvertingToHexadecimalString_ReturnsExpectedactual
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void SelectPpseCApduByteArray_ConvertingToHexadecimalString_ReturnsExpectedactual()
    {
        string expected = ApduTestData.CApdu.Select.Ppse.PpseHex;
        string actual = _SystemUnderTest.DecodeToString(ApduTestData.CApdu.Select.Ppse.PpseBytes);

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void SelectPpseRApduHexString_ConvertingToByteArray_ReturnsExpectedactual()
    {
        string testData = ApduTestData.RApdu.Select.Ppse.PpseHex;

        byte[] actual = _SystemUnderTest.Encode(testData);

        Assert.Equal(actual, ApduTestData.RApdu.Select.Ppse.PpseBytes);
    }

    [Fact]
    public void SelectPpseCApduHexString_ConvertingToByteArray_ReturnsExpectedactual()
    {
        string testData = ApduTestData.CApdu.Select.Ppse.PpseHex;

        byte[] actual = _SystemUnderTest.Encode(testData);

        Assert.Equal(actual, ApduTestData.CApdu.Select.Ppse.PpseBytes);
    }

    [Fact]
    public void SelectPpseRApduHexString_GettingByteCount_ReturnsExpectedactual()
    {
        string testData = ApduTestData.RApdu.Select.Ppse.PpseHex;

        int actual = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(actual, ApduTestData.RApdu.Select.Ppse.PpseBytes.Length);
    }

    [Fact]
    public void SelectPpseCApduHexString_GettingByteCount_ReturnsExpectedactual()
    {
        string testData = ApduTestData.CApdu.Select.Ppse.PpseHex;

        int actual = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(actual, ApduTestData.CApdu.Select.Ppse.PpseBytes.Length);
    }

    [Fact]
    public void GivenAsciiString_GetBytes_ThrowsEncodingException()
    {
        string testData = "Hello how are you people?";

        Assert.Throws<CodecParsingException>(() => PlayCodec.HexadecimalCodec.Encode(testData));
    }

    [Fact]
    public void GivenInvalidHexadecimalString_GetBytes_ThrowsEncodingException()
    {
        const string testData = "FFC•3C01CD6E4F?A13021";

        Assert.Throws<CodecParsingException>(() => PlayCodec.HexadecimalCodec.Encode(testData));
    }

    /// <summary>
    ///     GivenNullByteArray_GetString_ThrowsArgumentNullException
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void GivenNullByteArray_GetString_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PlayCodec.HexadecimalCodec.DecodeToString(null));
    }

    /// <summary>
    ///     GivenValidHexadecimalByteArray_GetString_ReturnsExpectedString
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void GivenValidHexadecimalByteArray_GetString_ReturnsExpectedString()
    {
        byte[] testData = {0xFF, 0x35, 0xA3, 0xBC};

        string expected = "FF35A3BC";
        string actual = PlayCodec.HexadecimalCodec.DecodeToString(testData);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GivenValidHexadecimalString_GetBytes_ReturnsExpectedByteArray()
    {
        string testData = "DFFC3C01CD6E4FA13021";

        byte[] expected = {0xDF, 0xFC, 0x3C, 0x01, 0xCD, 0x6E, 0x4F, 0xA1, 0x30, 0x21};
        byte[] actual = PlayCodec.HexadecimalCodec.Encode(testData);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GivenValidHexadecimalString_GetBytes_ReturnsExpectedBytes()
    {
        string testData = "F37DCA34EA";

        //new string(GetRandomHexChars(_Random, _Random.Next(1, 50)));

        byte[] expected = {0xF3, 0x7D, 0xCA, 0x34, 0xEA};

        byte[] actual = PlayCodec.HexadecimalCodec.Encode(testData);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GivenValidHexadecimalStringWithOddLength_GetBytes_ReturnsExpectedByteArray()
    {
        string testData = "FFC3C01CD6E4FA13021";

        byte[] expected = {0x0F, 0xFC, 0x3C, 0x01, 0xCD, 0x6E, 0x4F, 0xA1, 0x30, 0x21};
        byte[] actual = PlayCodec.HexadecimalCodec.Encode(testData);

        Assert.Equal(expected, actual);
    }

    #endregion
}