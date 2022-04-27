using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class UlongTests : TestBase
{
    #region Instance Members

    [Fact]
    public void Ulong_GetNumberOfDigits5_ReturnsExpectedResult()
    {
        ulong testData = ulong.MaxValue;
        int expected = Specs.Integer.UInt64.MaxDigits;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ulong_GetNumberOfDigits3_ReturnsExpectedResult()
    {
        ulong testData = 123;
        int expected = 3;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ulong_GetNumberOfDigits0_ReturnsExpectedResult()
    {
        ulong testData = 0;
        int expected = 1;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ulong_GetMostSignificantBit0_ReturnsExpectedResult()
    {
        ulong testData = 0;
        int expected = 0;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ulong_GetMostSignificantBit6_ReturnsExpectedResult()
    {
        ulong testData = 0b111111;
        int expected = 6;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ulong_GetMostSignificantBit64_ReturnsExpectedResult()
    {
        ulong testData = 0b1111111111111111111111111111111111111111111111111111111111111111;
        int expected = Specs.Integer.UInt64.BitCount;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(IntFixture.MostSignificantBit.ForULong), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
    public void RandomByteArray_InvokesConcatArrays_CreatesValueCopyWithCorrectLength(int actual, ulong testData)
    {
        int expected = testData.GetMostSignificantBit();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}