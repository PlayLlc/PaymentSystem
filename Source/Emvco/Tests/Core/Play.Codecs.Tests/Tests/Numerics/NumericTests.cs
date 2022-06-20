using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Codecs.Tests.Tests.Numerics;

public class NumericTests : TestBase
{
    #region Instance Values

    private readonly NumericCodec _SystemUnderTest;

    #endregion

    #region Constructor

    public NumericTests()
    {
        _SystemUnderTest = PlayCodec.NumericCodec;
    }

    #endregion

    #region GetByteCount

    [Fact]
    public void ValidOddString_GetByteCount_ReturnsExpectedResult()
    {
        string testData = "12345";
        int expected = 3;
        ushort actual = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void ValidEvenString_GetByteCount_ReturnsExpectedResult()
    {
        string testData = "123456";
        int expected = 3;
        ushort actual = _SystemUnderTest.GetByteCount(testData);

        Assertion(() => Assert.Equal(actual, expected));
    }

    #endregion

    #region GetCharCount

    [Fact]
    public void ValidEvenByteArray_GetCharCount_ReturnsExpectedResult()
    {
        byte[] testData = new byte[] {12, 34, 56};
        int expected = 6;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidOddByteArray_GetCharCount_ReturnsExpectedResult()
    {
        byte[] testData = new byte[] {12, 34, 5};
        int expected = 5;
        int actual = _SystemUnderTest.GetCharCount(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion

    #region Decode Then Encode

    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(NumericFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);
        byte[] encoded = _SystemUnderTest.Encode(decoded);

        Assert.Equal(testValue, encoded);
    }

    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomString), 100, 1, 300, MemberType = typeof(NumericFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
    {
        byte[] decoded = _SystemUnderTest.Encode(testValue);
        string encoded = _SystemUnderTest.DecodeToString(decoded);

        Assert.Equal(testValue, encoded);
    }

    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomUShort), 100, MemberType = typeof(NumericFixture))]
    public void RandomDecodedUShort_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
    {
        byte[]? decoded = _SystemUnderTest.Encode(testValue);
        ushort encoded = _SystemUnderTest.DecodeToUInt16(decoded);

        Assert.Equal(testValue, encoded);
    }

    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomUInt), 100, MemberType = typeof(NumericFixture))]
    public void RandomDecodedUInt_EncodingThenDecoding_ReturnsExpectedResult(uint testValue)
    {
        byte[]? decoded = _SystemUnderTest.Encode(testValue);
        uint encoded = _SystemUnderTest.DecodeToUInt32(decoded);

        Assert.Equal(testValue, encoded);
    }

    /// <param name="testValue"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomULong), 100, MemberType = typeof(NumericFixture))]
    public void RandomDecodedULong_EncodingThenDecoding_ReturnsExpectedResult(ulong testValue)
    {
        byte[]? decoded = _SystemUnderTest.Encode(testValue);
        ulong encoded = _SystemUnderTest.DecodeToUInt64(decoded);

        Assert.Equal(testValue, encoded);
    }

    #endregion

    #region Encode

    [Fact]
    public void AmountUshort_Encode_ReturnsExpectedResult()
    {
        ushort testData = 12345;
        byte[] expected = {1, 23, 45};

        byte[] actual = _SystemUnderTest.Encode(testData, 3);

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void AmountUint_Encode_ReturnsExpectedResult()
    {
        uint testData = 12345;
        byte[] expected = {1, 23, 45};
        byte[] actual = _SystemUnderTest.Encode(testData, 3);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AmountUlong_Encode_ReturnsExpectedResult()
    {
        ulong testData = 12345;

        byte[] expected = {1, 23, 45};
        byte[] actual = _SystemUnderTest.Encode(testData, 3);

        Assert.Equal(expected, actual);
    }

    #endregion

    #region IsValid

    [Fact]
    public void ValidByteArray_InvokingIsValid_ReturnsTrue()
    {
        byte[] testData = new byte[] {0x12, 0x34, 0x56};
        Assertion(() => Assert.True(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void InvalidByteArray_InvokingIsValid_ReturnsFalse()
    {
        byte[] testData = new byte[] {12, 34, 45, 0x5D};
        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void InvalidString_InvokingIsValid_ReturnsFalse()
    {
        string testData = "534C34";
        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    [Fact]
    public void ValidString_InvokingIsValid_ReturnsTrue()
    {
        string testData = "534C34";
        Assertion(() => Assert.False(_SystemUnderTest.IsValid(testData)));
    }

    #endregion
}