using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class LongTests : TestBase
{
    #region GetMaskedValue

    [Fact]
    public void Long_GetMaskedValueValue0Mask0_ReturnsExpectedResult()
    {
        long testData = 0;
        uint mask = 0;

        long expectedOutput = 0;

        Assertion(() => Assert.Equal(expectedOutput, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Long_GetMaskedValue0b11001100010111111011Mask0b0011_1011_ReturnsExpectedResult()
    {
        long testData = 0b1100_1100_0101_1111_1011;
        long mask = 0b0011_1011;

        long expectedOutput = 0b1100_1100_0101_1100_0000;

        Assertion(() => Assert.Equal(expectedOutput, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Long_GetMaskedValue_Value0b1100_0000_1101_1111_Mask0b0010_1111_ReturnsExpectedResult()
    {
        long testData = 0b1100_0111_1101_1111;
        long mask = 0b0011_0010_0000;

        long expectedOutput = 0b1100_0100_1101_1111;

        Assertion(() => Assert.Equal(expectedOutput, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Long_GetMaskedValueLongMaxMask0b110011001100_ReturnsExpectedResult()
    {
        long testData = long.MaxValue;
        long mask = 0b110011001100;

        long expectedOutput = 0b0111111111111111111111111111111111111111111111111111001100110011;
        Assertion(() => Assert.Equal(expectedOutput, testData.GetMaskedValue(mask)));
    }

    #endregion

    #region GetMostSignificantBit

    [Fact]
    public void Long_GetMostSignificantBit_Returns0()
    {
        long testData = 1;
        int expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Long_GetMostSignificantBit_Returns20()
    {
        long testData = 645132;
        int expected = 20;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Long_GetMostSignificantBit_Returns28()
    {
        long testData = 0b1111_0100_0101_0100_0101_0110_1110;
        int expected = 28;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Long_GetMostSignificantBitLongMaxValue_ReturnsLongSpecsMaxBitMinusTheSignBit()
    {
        long testData = long.MaxValue;
        int expected = Specs.Integer.Int64.BitCount - 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Long_GetMostSignificantBitLongMinValue_ReturnsLongSpecsMaxBitMinusTheSignBit()
    {
        long testData = long.MinValue;
        int expected = Specs.Integer.Int64.BitCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Theory]
    [MemberData(nameof(IntFixture.MostSignificantBit.ForLong), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
    public void RandomLong_GetMostSignificantBit_ReturnsExpectedResult(int actual, long testData)
    {
        int expected = testData.GetMostSignificantBit();

        if (testData > 0)
            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        else
            Assertion(() => Assert.Equal(expected, int.MinValue), Build.Equals.Message(expected, actual));
    }

    #endregion

    #region GetMostSignificantByte

    [Fact]
    public void Long_GetMostSignificantByte_Returns1()
    {
        long testData = 64;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Long_GetMostSignificantByte_Returns3()
    {
        long testData = 0b0111_1011_111_1111_0000_1111;
        byte expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Long_GetMostSignificantByte_Returns5()
    {
        long testData = 0b1001_1011_1011_0111_1011_111_1111_0000_1111;
        byte expected = 5;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Long_GetMostSignificantByteIntMax_ReturnsSpecsIntegerInt64ByteCount()
    {
        long testData = long.MaxValue;
        byte expected = Specs.Integer.Int64.ByteCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    #endregion

    #region GetNumberOfDigits

    [Fact]
    public void Long_GetNumberOfDigits_Returns0()
    {
        long testData = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Long_GetNumberOfDigits_Returns1()
    {
        long testData = 7;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Long_GetNumberOfDigits_Returns3()
    {
        long testData = 348;
        byte expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Long_GetNumberOfDigits_Returns5()
    {
        long testData = 84813;
        byte expected = 5;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Long_GetNumberOfDigits_ReturnsSpecsIntegerInt64MaxDigits()
    {
        long testData = long.MaxValue;
        byte expected = Specs.Integer.Int64.MaxDigits;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    #endregion
}