using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class IntTests : TestBase
{
    #region GetMostSignificantBit

    [Fact]
    public void Int_GetMostSignificantBit_Returns0()
    {
        int testData = 1;
        int expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Int_GetMostSignificantBit_Returns14()
    {
        int testData = 12345;
        int expected = 14;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Int_GetMostSignificantBit_Returns23()
    {
        int testData = 0b0100_0101_0100_0101_0110_1110;
        int expected = 23;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Int_GetMostSignificantBitForInt32MaxValue_ReturnsSpecsInt32BitCountMinusSignBit()
    {
        int testData = int.MaxValue;
        int expected = Specs.Integer.Int32.BitCount - 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Int_GetMostSignificantBitForInt32MinValue_ReturnsSpecsInt32BitCount()
    {
        int testData = int.MinValue;
        int expected = Specs.Integer.Int32.BitCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Theory]
    [MemberData(nameof(IntFixture.MostSignificantBit.ForInt), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
    public void RandomInt_GetMostSignificantForBitPositiveValue_ReturnsExpectedResult(int actual, int testData)
    {
        int expected = testData.GetMostSignificantBit();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion

    #region GetMostSignificantByte

    [Fact]
    public void Int_GetMostSignificantByte_Returns0()
    {
        int testData = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Int_GetMostSignificantByte_Returns1()
    {
        int testData = 64;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Int_GetMostSignificantByte_Returns3()
    {
        int testData = 0b0111_1011_111_1111_0000_1111;
        byte expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Int_GetMostSignificantByteIntMax_Returns4()
    {
        int testData = int.MaxValue;
        byte expected = 4;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    #endregion

    #region GetNumberOfDigits

    [Fact]
    public void Int_GetNumberOfDigits_Returns0()
    {
        int testData = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Int_GetNumberOfDigits_Returns1()
    {
        int testData = 7;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Int_GetNumberOfDigits_Returns3()
    {
        int testData = 348;
        byte expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Int_GetNumberOfDigits_Returns10()
    {
        int testData = int.MaxValue;
        byte expected = Specs.Integer.Int32.MaxDigits;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    #endregion

    #region TryGetRemainder

    [Fact]
    public void Int_TryGetRemainderFor5Divisor2_ReturnsExpectedResult()
    {
        int testData = 5;
        int divisor = 2;
        int expected = 1;
        int actual = testData.TryGetRemainder(divisor, out int resultWithOutRemainder);

        Assertion(() => Assert.Equal(expected, actual));

        int expectedResultWithoutRemainder = 2;

        Assertion(() => Assert.Equal(expectedResultWithoutRemainder, resultWithOutRemainder));
    }

    [Fact]
    public void Int_TryGetRemainderFor17Divisor3_ReturnsExpectedResult()
    {
        int testData = 17;
        int divisor = 3;
        int expected = 2;
        int actual = testData.TryGetRemainder(divisor, out int resultWithOutRemainder);

        Assertion(() => Assert.Equal(expected, actual));

        int expectedResultWithoutRemainder = 5;

        Assertion(() => Assert.Equal(expectedResultWithoutRemainder, resultWithOutRemainder));
    }

    [Fact]
    public void Int_TryGetRemainderFor1743Divisor36_ReturnsExpectedResult()
    {
        int testData = 1743;
        int divisor = 36;
        int expected = 15;
        int actual = testData.TryGetRemainder(divisor, out int resultWithOutRemainder);

        Assertion(() => Assert.Equal(expected, actual));

        int expectedResultWithoutRemainder = 48;

        Assertion(() => Assert.Equal(expectedResultWithoutRemainder, resultWithOutRemainder));
    }

    #endregion
}