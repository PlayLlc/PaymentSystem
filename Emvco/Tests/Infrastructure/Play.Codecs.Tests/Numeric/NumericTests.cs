using Play.Emv.TestData.Encoding;

using Xunit;

namespace Play.Codecs.Tests.Numeric;

public class NumericTests
{
    #region Instance Values

    private readonly Strings.Numeric _SystemUnderTest;

    #endregion

    #region Constructor

    public NumericTests()
    {
        _SystemUnderTest = PlayEncoding.Numeric;
    }

    #endregion

    #region Instance Members

    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(NumericFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.GetString(testValue);
        byte[] encoded = _SystemUnderTest.GetBytes(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomString), 100, 1, 300, MemberType = typeof(NumericFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
    {
        byte[] decoded = _SystemUnderTest.GetBytes(testValue);
        string encoded = _SystemUnderTest.GetString(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomUShort), 100, MemberType = typeof(NumericFixture))]
    public void RandomDecodedUShort_EncodingThenDecoding_ReturnsExpectedResult(ushort testValue)
    {
        byte[]? decoded = _SystemUnderTest.GetBytes(testValue);
        ushort encoded = _SystemUnderTest.GetUInt16(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomUInt), 100, MemberType = typeof(NumericFixture))]
    public void RandomDecodedUInt_EncodingThenDecoding_ReturnsExpectedResult(uint testValue)
    {
        byte[]? decoded = _SystemUnderTest.GetBytes(testValue);
        uint encoded = _SystemUnderTest.GetUInt32(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(NumericFixture.GetRandomULong), 100, MemberType = typeof(NumericFixture))]
    public void RandomDecodedULong_EncodingThenDecoding_ReturnsExpectedResult(ulong testValue)
    {
        byte[]? decoded = _SystemUnderTest.GetBytes(testValue);
        ulong encoded = _SystemUnderTest.GetUInt64(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Fact]
    public void AmountUshort_ConvertingToByteArray_ReturnsExpectedResult()
    {
        ushort testData = EncodingTestDataFactory.Numeric.ValueUshort;

        byte[] result = _SystemUnderTest.GetBytes(testData, 3);

        Assert.Equal(result, EncodingTestDataFactory.Numeric.ValueBytes);
    }

    [Fact]
    public void AmountUint_ConvertingToByteArray_ReturnsExpectedResult()
    {
        uint testData = EncodingTestDataFactory.Numeric.ValueUint;
        byte[] result = _SystemUnderTest.GetBytes(testData, 3);

        Assert.Equal(result, EncodingTestDataFactory.Numeric.ValueBytes);
    }

    [Fact]
    public void AmountUlong_ConvertingToByteArray_ReturnsExpectedResult()
    {
        ulong testData = EncodingTestDataFactory.Numeric.ValueUlong;

        byte[] result = _SystemUnderTest.GetBytes(testData, 3);

        Assert.Equal(result, EncodingTestDataFactory.Numeric.ValueBytes);
    }

    #endregion
}