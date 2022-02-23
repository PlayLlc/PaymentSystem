using Play.Codecs.Tests.Numeric;
using Play.Emv.TestData.Encoding;

using Xunit;

namespace Play.Codecs.Tests.StrictAscii;

public class StrictAsciiTests
{
    #region Instance Values

    private readonly StrictAsciiCodec _SystemUnderTest;

    #endregion

    #region Constructor

    public StrictAsciiTests()
    {
        _SystemUnderTest = PlayCodec.AsciiCodec;
    }

    #endregion

    #region Instance Members

    [Theory]
    [MemberData(nameof(AsciiFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(NumericFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] testValue)
    {
        string decoded = _SystemUnderTest.DecodeToString(testValue);

        byte[] encoded = _SystemUnderTest.Encode(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Theory]
    [MemberData(nameof(AsciiFixture.GetRandomString), 100, 1, 300, MemberType = typeof(NumericFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string testValue)
    {
        byte[] decoded = _SystemUnderTest.Encode(testValue);
        string encoded = _SystemUnderTest.DecodeToString(decoded);

        Assert.Equal(testValue, encoded);
    }

    [Fact]
    public void ApplicationLabelByteArray_ConvertingToAsciiString_ReturnsExpectedResult()
    {
        byte[] testData = EncodingTestDataFactory.StrictAscii.ApplicationLabelBytes;

        string result = _SystemUnderTest.DecodeToString(testData);

        Assert.Equal(result, EncodingTestDataFactory.StrictAscii.ApplicationLabelAscii);
    }

    [Fact]
    public void ApplicationLabelAsciiString_ConvertingToByteArray_ReturnsExpectedResult()
    {
        string testData = EncodingTestDataFactory.StrictAscii.ApplicationLabelAscii;

        byte[] result = _SystemUnderTest.Encode(testData);

        Assert.Equal(result, EncodingTestDataFactory.StrictAscii.ApplicationLabelBytes);
    }

    [Fact]
    public void ApplicationLabelAsciiString_GettingByteCount_ReturnsExpectedResult()
    {
        string testData = EncodingTestDataFactory.StrictAscii.ApplicationLabelAscii;
        int result = _SystemUnderTest.GetByteCount(testData);

        Assert.Equal(result, EncodingTestDataFactory.StrictAscii.ApplicationLabelBytes.Length);
    }

    #endregion
}