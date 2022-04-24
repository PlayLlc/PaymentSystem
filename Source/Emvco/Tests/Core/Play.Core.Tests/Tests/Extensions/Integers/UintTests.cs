using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
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
    public void Uint_GetMostSignificantBit10_ReturnsExpectedResult()
    {
        uint testData = 0b0000001000000000;
        int expected = 10;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Uint_GetMostSignificantBit32_ReturnsExpectedResult()
    {
        uint testData = 0b10000000000000000000000000000000;
        int expected = 32;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(IntFixture.MostSignificantBit.ForUInt), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
    public void RandomByteArray_InvokesConcatArrays_CreatesValueCopyWithCorrectLength(int actual, uint testData)
    {
        int expected = testData.GetMostSignificantBit();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}