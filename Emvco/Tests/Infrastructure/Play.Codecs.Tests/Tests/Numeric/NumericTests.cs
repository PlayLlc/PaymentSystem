using Play.Codecs.Exceptions;
using Play.Testing.Infrastructure.BaseTestClasses;

using Xunit;

namespace Play.Codecs.Tests.Numeric;

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

    #region Instance Members

    /// <summary>
    ///     RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult
    /// </summary>
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

    /// <summary>
    ///     RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult
    /// </summary>
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

    /// <summary>
    ///     RandomDecodedUShort_EncodingThenDecoding_ReturnsExpectedResult
    /// </summary>
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

    /// <summary>
    ///     RandomDecodedUInt_EncodingThenDecoding_ReturnsExpectedResult
    /// </summary>
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

    /// <summary>
    ///     RandomDecodedULong_EncodingThenDecoding_ReturnsExpectedResult
    /// </summary>
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

    [Fact]
    public void AmountUshort_ConvertingToByteArray_ReturnsExpectedResult()
    {
        ushort testData = 12345;
        byte[] expected = new byte[] {1, 23, 45};

        byte[] actual = _SystemUnderTest.Encode(testData, 3);

        Assert.Equal(actual, expected);
    }

    [Fact]
    public void AmountUint_ConvertingToByteArray_ReturnsExpectedResult()
    {
        uint testData = 12345;
        byte[] expected = new byte[] {1, 23, 45};
        byte[] actual = _SystemUnderTest.Encode(testData, 3);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AmountUlong_ConvertingToByteArray_ReturnsExpectedResult()
    {
        ulong testData = 12345;

        byte[] expected = new byte[] {1, 23, 45};
        byte[] actual = _SystemUnderTest.Encode(testData, 3);

        Assert.Equal(expected, actual);
    }

    #endregion
}