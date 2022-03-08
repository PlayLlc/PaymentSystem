using Play.Codecs.Exceptions;

using Xunit;

namespace Play.Codecs.Tests.Alphabetic;

public class AlphabeticTests
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

    #region Instance Members

    /// <summary>
    /// RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult
    /// </summary>
    /// <param name="testValue"></param>
    /// <exception cref="PlayEncodingException"></exception>
    [Theory]
    [MemberData(nameof(AlphabeticFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(AlphabeticFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);
        byte[] encoded = _SystemUnderTest.Encode(decoded);

        Assert.Equal(testValue, encoded);
    }

    /// <summary>
    /// RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult
    /// </summary>
    /// <param name="testValue"></param>
    /// <exception cref="PlayEncodingException"></exception>
    [Theory]
    [MemberData(nameof(AlphabeticFixture.GetRandomString), 100, 1, 300, MemberType = typeof(AlphabeticFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
    {
        byte[] decoded = _SystemUnderTest.Encode(testValue);
        string encoded = _SystemUnderTest.DecodeToString(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Fact]
    public void GivenInvalidAlphabeticString_GetBytes_ThrowsEncodingException()
    {
        const string testData = "FFC•3C01CD6E4F?A13021";

        Assert.Throws<PlayEncodingException>(() => _SystemUnderTest.Encode(testData));
    }

    #endregion
}