using Play.Codecs.Exceptions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Codecs.Tests.Tests.Alphabetics;

public class AlphabeticTests : TestBase
{
    #region Instance Values

    private readonly AlphabeticCodec _SystemUnderTest;

    #endregion

    #region Constructor

    public AlphabeticTests()
    {
        _SystemUnderTest = PlayCodec.AlphabeticCodec;
    }

    #endregion

    #region Encoding Then Decoding

    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(AlphabeticFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(AlphabeticFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);
        byte[] actual = _SystemUnderTest.Encode(decoded);

        Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
    }

    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(AlphabeticFixture.GetRandomString), 100, 1, 300, MemberType = typeof(AlphabeticFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
    {
        byte[] decoded = _SystemUnderTest.Encode(testValue);
        string actual = _SystemUnderTest.DecodeToString(decoded);
        Assertion(() => Assert.Equal(testValue, actual), Build.Equals.Message(testValue, actual));
    }

    [Fact]
    public void GivenInvalidAlphabeticString_Encode_ThrowsEncodingException()
    {
        const string testData = "FFC•3C01CD6E4F?A13021";

        Assert.Throws<CodecParsingException>(() => _SystemUnderTest.Encode(testData));
    }

    #endregion

    #region GetByteCount

    [Fact]
    public void ValidOddString_GetByteCount_ReturnsExpectedResult()
    {
        string testData = "12345";
        int expected = 5;
        int actual = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void ValidEvenString_GetByteCount_ReturnsExpectedResult()
    {
        string testData = "123456";
        int expected = 6;
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
        byte[] testData = new byte[] {(byte) 'a', (byte) 'Z', (byte) 'p', (byte) 'o'};
        Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void InvalidByteArray_InvokingIsValid_ReturnsFalse()
    {
        byte[] testData = new byte[] {(byte) '7', (byte) 'b', (byte) 'Z'};
        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void InvalidString_InvokingIsValid_ReturnsFalse()
    {
        string testData = "AbcEz;jLm";
        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void ValidString_InvokingIsValid_ReturnsTrue()
    {
        string testData = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
    }

    #endregion

    #region GetCharCount

    [Fact]
    public void ValidEvenString_GetCharCount_ReturnsExpectedResult()
    {
        string testData = "abcdzOpfjz";
        int expected = 10;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidOddString_GetCharCount_ReturnsExpectedResult()
    {
        string testData = "oepfuekcVjEJ";
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

    #region Encode

    [Fact]
    public void ValidEvenString_Encode_ReturnsExpectedResult()
    {
        string testData = "aBcDzP";
        byte[] expected = {(byte) 'a', (byte) 'B', (byte) 'c', (byte) 'D', (byte) 'z', (byte) 'P'};

        byte[] actual = _SystemUnderTest.Encode(testData);

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void ValidOddString_Encode_ReturnsExpectedResult()
    {
        string testData = "aBcDzPW";
        byte[] expected = {(byte) 'a', (byte) 'B', (byte) 'c', (byte) 'D', (byte) 'z', (byte) 'P', (byte) 'W'};

        byte[] actual = _SystemUnderTest.Encode(testData);

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void EmptyString_Encode_ReturnsExpectedResult()
    {
        string testData = "";

        byte[] expected = { };
        byte[] actual = _SystemUnderTest.Encode(testData);

        Assert.Equal(expected, actual);
    }

    #endregion

    #region GetCharCount

    [Fact]
    public void ValidEvenByteArray_GetCharCount_ReturnsExpectedResult()
    {
        byte[] testData = {(byte) 'a', (byte) 'B', (byte) 'c', (byte) 'D', (byte) 'z', (byte) 'P'};
        int expected = 6;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidOddByteArray_GetCharCount_ReturnsExpectedResult()
    {
        byte[] testData = {(byte) 'a', (byte) 'B', (byte) 'c', (byte) 'D', (byte) 'z'};
        int expected = 5;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion
}