using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class UintTests : TestBase
{
    #region Instance Members

    [Fact]
    public void Uint_GetNumberOfDigits5_ReturnsExpectedResult()
    {
        uint testData = uint.MaxValue;
        int expected = Specs.Integer.UInt32.MaxDigits;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Uint_GetNumberOfDigits3_ReturnsExpectedResult()
    {
        uint testData = 123;
        int expected = 3;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Uint_GetNumberOfDigits0_ReturnsExpectedResult()
    {
        uint testData = 0;
        int expected = 1;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Uint_GetMostSignificantBit0_ReturnsExpectedResult()
    {
        uint testData = 0;
        int expected = 0;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Uint_GetMostSignificantBit14_ReturnsExpectedResult()
    {
        uint testData = 12345;
        int expected = 14;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Uint_GetMostSignificantBit16_ReturnsExpectedResult()
    {
        uint testData = uint.MaxValue;
        int expected = Specs.Integer.UInt32.BitCount;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}