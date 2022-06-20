using Play.Codecs.Exceptions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Codecs.Tests.Tests.AlphaNumericSpecials;

public class AlphaNumericSpecialTests : TestBase
{
    #region Instance Values

    private readonly AlphaNumericSpecialCodec _SystemUnderTest;

    #endregion

    #region Constructor

    public AlphaNumericSpecialTests()
    {
        _SystemUnderTest = PlayCodec.AlphaNumericSpecialCodec;
    }

    #endregion

    #region Encode

    [Fact]
    public void GivenValidEvenAlphaNumericString_InvokingEncode_ThrowsEncodingException()
    {
        const string testData = "?bcAF:,";
        byte[] expected = new byte[] {(byte) '?', (byte) 'b', (byte) 'c', (byte) 'A', (byte) 'F', (byte) ':', (byte) ','};
        Assertion(() => Assert.Equal(expected, _SystemUnderTest.Encode(testData)));
    }

    [Fact]
    public void GivenValidOddAlphaNumericString_InvokingEncode_ThrowsEncodingException()
    {
        const string testData = "#bcAF3'";
        byte[] expected = new byte[] {(byte) '#', (byte) 'b', (byte) 'c', (byte) 'A', (byte) 'F', (byte) '3', (byte) '\''};
        Assertion(() => Assert.Equal(expected, _SystemUnderTest.Encode(testData)));
    }

    [Fact]
    public void GivenInvalidAlphaNumericString_InvokingEncode_ThrowsEncodingException()
    {
        const string testData = "🐝FFC•🦼3C01CD🐝6E4F?A13021🐝";

        Assert.Throws<CodecParsingException>(() => _SystemUnderTest.Encode(testData));
    }

    #endregion

    #region Decode Then Encode

    /// <summary>
    ///     RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult
    /// </summary>
    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(AlphaNumericSpecialFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(AlphaNumericSpecialFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);
        byte[] actual = _SystemUnderTest.Encode(decoded);
        Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
    }

    /// <summary>
    ///     RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult
    /// </summary>
    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(AlphaNumericSpecialFixture.GetRandomString), 100, 1, 300, MemberType = typeof(AlphaNumericSpecialFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
    {
        byte[] decoded = _SystemUnderTest.Encode(testValue);
        string actual = _SystemUnderTest.DecodeToString(decoded);
        Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
    }

    #endregion

    #region GetByteCount

    [Fact]
    public void ValidOddString_GetByteCount_ReturnsExpectedResult()
    {
        string testData = "a^23*d45";
        int expected = 8;
        int actual = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void ValidEvenString_GetByteCount_ReturnsExpectedResult()
    {
        string testData = "a1@3f45+EPc";
        int expected = 11;
        int actual = _SystemUnderTest.GetByteCount(testData);

        Assertion(() => Assert.Equal(actual, expected));
    }

    [Fact]
    public void EmptyString_GetByteCount_ReturnsExpectedResult()
    {
        string testData = "";
        int expected = 0;
        int actual = _SystemUnderTest.GetByteCount(testData);

        Assertion(() => Assert.Equal(actual, expected));
    }

    #endregion

    #region IsValid

    [Fact]
    public void ValidByteArray_InvokingIsValid_ReturnsTrue()
    {
        byte[] testData = new byte[] {(byte) '&', (byte) 'Z', (byte) 'p', (byte) 'o', (byte) '1', (byte) '>'};
        Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void InvalidByteArray_InvokingIsValid_ReturnsFalse()
    {
        byte[] testData = new byte[] {(byte) byte.MaxValue, (byte) 'b', (byte) 'Z'};
        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void InvalidString_InvokingIsValid_ReturnsFalse()
    {
        string testData = "🦖AbcE🐉🐉z;jLm🦖";
        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void ValidString_InvokingIsValid_ReturnsTrue()
    {
        string testData = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
    }

    #endregion

    #region GetCharCount

    [Fact]
    public void ValidEvenByteArray_GetCharCount_ReturnsExpectedResult()
    {
        byte[] testData = {(byte) '9', (byte) 'B', (byte) '3', (byte) 'D', (byte) 'z', (byte) '{'};
        int expected = 6;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidOddByteArray_GetCharCount_ReturnsExpectedResult()
    {
        byte[] testData = {(byte) 'a', (byte) '#', (byte) '8', (byte) 'D', (byte) 'z'};
        int expected = 5;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidEvenString_GetCharCount_ReturnsExpectedResult()
    {
        string testData = "ab3dz0;fjz";
        int expected = 10;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidOddString_GetCharCount_ReturnsExpectedResult()
    {
        string testData = "oepf(ekcVj2J";
        int expected = 12;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidEmptyString_GetCharCount_ReturnsExpectedResult()
    {
        string testData = "";
        int expected = 0;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion
}