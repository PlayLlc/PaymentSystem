using Play.Codecs.Exceptions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Codecs.Tests.Tests.UnsignedIntegers;

public class UnsignedIntegerTests : TestBase
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

    /// <param name="expected"></param>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="Exceptions.CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomBytes), 50, 1, 300, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] expected)
    {
        string? decoded = _SystemUnderTest.DecodeToString(expected);
        byte[]? actual = _SystemUnderTest.Encode(decoded);

        Assertion(() => { Assert.Equal(expected, actual!); }, Build.Equals.Message(expected, actual!));
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomUShort), 50, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedUShort_EncodingThenDecoding_ReturnsExpectedResult(ushort expected)
    {
        byte[]? decoded = _SystemUnderTest.Encode(expected);
        ushort actual = _SystemUnderTest.DecodeToUInt16(decoded);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomUInt), 50, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedUInt_EncodingThenDecoding_ReturnsExpectedResult(uint expected)
    {
        byte[]? decoded = _SystemUnderTest.Encode(expected);
        uint actual = _SystemUnderTest.DecodeToUInt32(decoded);

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(UnsignedIntegerFixture.GetRandomULong), 50, MemberType = typeof(UnsignedIntegerFixture))]
    public void RandomDecodedULong_EncodingThenDecoding_ReturnsExpectedResult(ulong expected)
    {
        byte[]? decoded = _SystemUnderTest.Encode(expected);
        ulong actual = _SystemUnderTest.DecodeToUInt64(decoded);

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    #endregion
}