using Play.Codecs.Exceptions;

using Xunit;

namespace Play.Codecs.Tests.AlphaNumeric;

public class AlphaNumericTests
{
    #region Instance Values

    private readonly Strings.AlphaNumeric _SystemUnderTest;

    #endregion

    #region Constructor

    public AlphaNumericTests()
    {
        _SystemUnderTest = PlayEncoding.AlphaNumeric;
    }

    #endregion

    #region Instance Members

    [Theory]
    [MemberData(nameof(AlphaNumericFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(AlphaNumericFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.GetString(testValue);
        byte[] encoded = _SystemUnderTest.GetBytes(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(AlphaNumericFixture.GetRandomString), 100, 1, 300, MemberType = typeof(AlphaNumericFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
    {
        byte[] decoded = _SystemUnderTest.GetBytes(testValue);
        string encoded = _SystemUnderTest.GetString(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Fact]
    public void GivenInvalidAlphabeticString_GetBytes_ThrowsEncodingException()
    {
        const string testData = "FFC•3C01CD6E4F?A13021";

        Assert.Throws<EncodingException>(() => _SystemUnderTest.GetBytes(testData));
    }

    #endregion
}