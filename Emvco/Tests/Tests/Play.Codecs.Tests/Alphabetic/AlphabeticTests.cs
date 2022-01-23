using Play.Codecs.Exceptions;

using Xunit;

namespace Play.Codecs.Tests.Alphabetic;

public class AlphabeticTests
{
    #region Instance Values

    private readonly Strings.Alphabetic _SystemUnderTest;

    #endregion

    #region Constructor

    public AlphabeticTests()
    {
        _SystemUnderTest = PlayEncoding.Alphabetic;
    }

    #endregion

    #region Instance Members

    [Theory]
    [MemberData(nameof(AlphabeticFixture.GetRandomBytes), new object[] {100, 1, 300}, MemberType = typeof(AlphabeticFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.GetString(testValue);
        byte[] encoded = _SystemUnderTest.GetBytes(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(AlphabeticFixture.GetRandomString), new object[] {100, 1, 300}, MemberType = typeof(AlphabeticFixture))]
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