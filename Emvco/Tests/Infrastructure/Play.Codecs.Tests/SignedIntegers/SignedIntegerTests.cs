using Play.Codecs.Integers;

using Xunit;

namespace Play.Codecs.Tests.SignedIntegers;

public class SignedIntegerTests
{
    #region Instance Values

    private readonly SignedIntegerCodec _SystemUnderTest;

    #endregion

    #region Constructor

    public SignedIntegerTests()
    {
        _SystemUnderTest = PlayEncoding.SignedIntegerCodec;
    }

    #endregion

    #region Instance Members

    [Theory]
    [MemberData(nameof(SignedIntegerFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(SignedIntegerFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);
        byte[] encoded = _SystemUnderTest.Encode(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(SignedIntegerFixture.GetRandomShort), 100, MemberType = typeof(SignedIntegerFixture))]
    public void RandomDecodedUShort_EncodingThenDecoding_ReturnsExpectedResult(short testValue)
    {
        byte[]? decoded = _SystemUnderTest.GetBytes(testValue);
        short encoded = _SystemUnderTest.GetInt16(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(SignedIntegerFixture.GetRandomInt), 100, MemberType = typeof(SignedIntegerFixture))]
    public void RandomDecodedUInt_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
    {
        byte[]? decoded = _SystemUnderTest.GetBytes(testValue);
        int encoded = _SystemUnderTest.GetInt32(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(SignedIntegerFixture.GetRandomLong), 100, MemberType = typeof(SignedIntegerFixture))]
    public void RandomDecodedULong_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
    {
        byte[]? decoded = _SystemUnderTest.GetBytes(testValue);
        long encoded = _SystemUnderTest.GetInt64(decoded);

        Assert.Equal(testValue, encoded);
    }

    #endregion
}