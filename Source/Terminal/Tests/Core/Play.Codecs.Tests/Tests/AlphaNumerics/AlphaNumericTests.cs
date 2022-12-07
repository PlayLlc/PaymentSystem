using Play.Codecs.Exceptions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Codecs.Tests.Tests.AlphaNumerics;

public class AlphaNumericTests : TestBase
{
    #region Instance Values

    private readonly AlphaNumericCodec _SystemUnderTest;

    #endregion

    #region Constructor

    public AlphaNumericTests()
    {
        _SystemUnderTest = PlayCodec.AlphaNumericCodec;
    }

    #endregion

    #region Encode

    [Fact]
    public void GivenValidEvenAlphaNumericString_InvokingEncode_ThrowsEncodingException()
    {
        const string testData = "abcAF3";
        byte[] expected = new byte[] {(byte) 'a', (byte) 'b', (byte) 'c', (byte) 'A', (byte) 'F', (byte) '3'};
        Assertion(() => Assert.Equal(expected, _SystemUnderTest.Encode(testData)));
    }

    [Fact]
    public void GivenValidOddAlphaNumericString_InvokingEncode_ThrowsEncodingException()
    {
        const string testData = "abcAF30";
        byte[] expected = new byte[] {(byte) 'a', (byte) 'b', (byte) 'c', (byte) 'A', (byte) 'F', (byte) '3', (byte) '0'};
        Assertion(() => Assert.Equal(expected, _SystemUnderTest.Encode(testData)));
    }

    [Fact]
    public void GivenInvalidAlphaNumericString_InvokingEncode_ThrowsEncodingException()
    {
        const string testData = "FFC•3C01CD6E4F?A13021";

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
    [MemberData(nameof(AlphaNumericFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(AlphaNumericFixture))]
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
    [MemberData(nameof(AlphaNumericFixture.GetRandomString), 100, 1, 300, MemberType = typeof(AlphaNumericFixture))]
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
        string testData = "a123Bd45";
        int expected = 8;
        int actual = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void ValidEvenString_GetByteCount_ReturnsExpectedResult()
    {
        string testData = "a123f456EPc";
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
        byte[] testData = new byte[] {(byte) 'a', (byte) 'Z', (byte) 'p', (byte) 'o', (byte) '1', (byte) '9'};
        Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void InvalidByteArray_InvokingIsValid_ReturnsFalse()
    {
        byte[] testData = new byte[] {(byte) ',', (byte) 'b', (byte) 'Z'};
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
        string testData = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
    }

    #endregion

    #region GetCharCount

    [Fact]
    public void ValidEvenByteArray_GetCharCount_ReturnsExpectedResult()
    {
        byte[] testData = {(byte) '9', (byte) 'B', (byte) '3', (byte) 'D', (byte) 'z', (byte) 'P'};
        int expected = 6;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidOddByteArray_GetCharCount_ReturnsExpectedResult()
    {
        byte[] testData = {(byte) 'a', (byte) 'B', (byte) '8', (byte) 'D', (byte) 'z'};
        int expected = 5;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidEvenString_GetCharCount_ReturnsExpectedResult()
    {
        string testData = "ab3dz0pfjz";
        int expected = 10;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidOddString_GetCharCount_ReturnsExpectedResult()
    {
        string testData = "oepf6ekcVj2J";
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