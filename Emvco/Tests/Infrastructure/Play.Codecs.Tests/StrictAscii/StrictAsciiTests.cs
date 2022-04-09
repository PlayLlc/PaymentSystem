using Play.Codecs.Exceptions;
using Play.Codecs.Tests.Numeric;
using Play.Emv.TestData.Ber.Primitive;
using Play.Tests.Core.BaseTestClasses;

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
        string expected = ApplicationLabelTestTlv.GetDefaultAsString();
        string actual = _SystemUnderTest.DecodeToString(ApplicationLabelTestTlv.GetDefaultAsBytes());
        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ApplicationLabelAsciiString_ConvertingToByteArray_ReturnsExpectedResult()
    {
        byte[] expected = ApplicationLabelTestTlv.GetDefaultAsBytes();
        byte[] actual = _SystemUnderTest.Encode(ApplicationLabelTestTlv.GetDefaultAsString());

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ApplicationLabelAsciiString_GettingByteCount_ReturnsExpectedResult()
    {
        int expected = ApplicationLabelTestTlv.GetDefaultAsBytes().Length;
        string testData = ApplicationLabelTestTlv.GetDefaultAsString();
        int actual = _SystemUnderTest.GetByteCount(testData);

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    #endregion
}