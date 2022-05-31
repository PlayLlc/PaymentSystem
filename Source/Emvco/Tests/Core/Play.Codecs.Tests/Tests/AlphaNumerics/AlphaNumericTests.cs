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

    #region Instance Members

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

    [Fact]
    public void GivenInvalidAlphabeticString_GetBytes_ThrowsEncodingException()
    {
        const string testData = "FFC•3C01CD6E4F?A13021";

        Assert.Throws<CodecParsingException>(() => _SystemUnderTest.Encode(testData));
    }

    #endregion
}