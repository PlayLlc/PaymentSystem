using Play.Codecs.Integers;

using Xunit;

namespace Play.Codecs.Tests.UnsignedIntegers;

public class UnsignedIntegerTests
{
    #region Instance Values

    private readonly UnsignedIntegerCodec _SystemUnderTest;

    #endregion

    #region Constructor

    public UnsignedIntegerTests()
    {
        _SystemUnderTest = PlayEncoding.UnsignedIntegerCodec;
    }

    #endregion

    #region Instance Members

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);
        byte[] encoded = _SystemUnderTest.Encode(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomUShort), 100, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedUShort_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
    {
        byte[]? decoded = _SystemUnderTest.GetBytes(testValue);
        ushort encoded = _SystemUnderTest.GetUInt16(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomUInt), 100, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedUInt_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
    {
        byte[]? decoded = _SystemUnderTest.GetBytes(testValue);
        uint encoded = _SystemUnderTest.GetUInt32(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomULong), 100, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedULong_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
    {
        byte[]? decoded = _SystemUnderTest.GetBytes(testValue);
        ulong encoded = _SystemUnderTest.GetUInt64(decoded);

        Assert.Equal(testValue, encoded);
    }

    #endregion
}