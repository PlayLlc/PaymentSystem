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
        _SystemUnderTest = PlayCodec.UnsignedIntegerCodec;
    }

    #endregion

    #region Instance Members

    /// <summary>
    /// RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult
    /// </summary>
    /// <param name="testValue"></param>
    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingFormatException"></exception>
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
        byte[]? decoded = _SystemUnderTest.Encode(testValue);
        ushort encoded = _SystemUnderTest.DecodeToUInt16(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomUInt), 100, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedUInt_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
    {
        byte[]? decoded = _SystemUnderTest.Encode(testValue);
        uint encoded = _SystemUnderTest.DecodeToUInt32(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomULong), 100, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedULong_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
    {
        byte[]? decoded = _SystemUnderTest.Encode(testValue);
        ulong encoded = _SystemUnderTest.DecodeToUInt64(decoded);

        Assert.Equal(testValue, encoded);
    }

    #endregion
}