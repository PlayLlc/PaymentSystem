﻿using Play.Codecs.Exceptions;
using Play.Codecs.Strings;

using Xunit;

namespace Play.Codecs.Tests.AlphaNumeric;

public class AlphaNumericTests
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

    [Theory]
    [MemberData(nameof(AlphaNumericFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(AlphaNumericFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);
        byte[] encoded = _SystemUnderTest.Encode(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(AlphaNumericFixture.GetRandomString), 100, 1, 300, MemberType = typeof(AlphaNumericFixture))]
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