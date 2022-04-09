using Play.Codecs.Exceptions;
using Play.Codecs.Tests.Numeric;
using Play.Testing.Infrastructure.BaseTestClasses;

using Xunit;

namespace Play.Codecs.Tests.StrictAscii;

public class StrictAsciiTests : TestBase
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

    /// <summary>
    ///     RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult
    /// </summary>
    /// <param name="expected"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(AsciiFixture.GetRandomBytes), 100, 1, 300, MemberType = typeof(NumericFixture))]
    public void RandomByteEncoding_DecodingThenEncoding_ReturnsExpectedResult(byte[] expected)
    {
        string decoded = _SystemUnderTest.DecodeToString(expected);

        byte[] actual = _SystemUnderTest.Encode(decoded);

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult
    /// </summary>
    /// <param name="expected"></param>
    /// <exception cref="CodecParsingException"></exception>
    [Theory]
    [MemberData(nameof(AsciiFixture.GetRandomString), 100, 1, 300, MemberType = typeof(NumericFixture))]
    public void RandomDecodedValue_EncodingThenDecoding_ReturnsExpectedResult(string expected)
    {
        byte[] decoded = _SystemUnderTest.Encode(expected);
        string actual = _SystemUnderTest.DecodeToString(decoded);

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    /// <summary>
    ///     ApplicationLabelByteArray_ConvertingToAsciiString_ReturnsExpectedResult
    /// </summary>
    /// <exception cref="CodecParsingException"></exception>
    [Fact]
    public void ApplicationLabelByteArray_ConvertingToAsciiString_ReturnsExpectedResult()
    {
        string expected = GetDefaultTestString();
        string actual = _SystemUnderTest.DecodeToString(GetDefaultTestBytes());
        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ApplicationLabelAsciiString_ConvertingToByteArray_ReturnsExpectedResult()
    {
        byte[] expected = GetDefaultTestBytes();
        byte[] actual = _SystemUnderTest.Encode(GetDefaultTestString());

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ApplicationLabelAsciiString_GettingByteCount_ReturnsExpectedResult()
    {
        int expected = GetDefaultTestBytes().Length;
        string testData = GetDefaultTestString();
        int actual = _SystemUnderTest.GetByteCount(testData);

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    public static string GetDefaultTestString() => "VISA PREPAID";

    private static byte[] GetDefaultTestBytes() =>
        new byte[]
        {
            0x56, 0x49, 0x53, 0x41, 0x20, 0x50, 0x52, 0x45,
            0x50, 0x41, 0x49, 0x44
        };

    #endregion
}