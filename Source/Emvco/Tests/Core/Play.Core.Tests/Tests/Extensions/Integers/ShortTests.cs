using Play.Core.Extensions;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class ShortTests : TestBase
{
    #region InstanceMembers

    #endregion

    #region GetMaskedShort

    [Fact]
    public void Short_GetMaskedShortFor0b10111BitsOne_ReturnsExpectedResult()
    {
        short testData = 0b10111;

        Bits[] leftbyteMaskBits = new[] { Bits.One };
        Bits[] rightbyteMaskBits = new[] { Bits.One };

        short expected = 0b10110;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedShort(leftbyteMaskBits, rightbyteMaskBits)));
    }

    [Fact]
    public void Short_GetMaskedShortFor0b0100010101101110BitsOneAndThree_ReturnsExpectedResult()
    {
        short testData = 0b0100_0101_0110_1110;

        Bits[] leftbyteMaskBits = new[] { Bits.One };
        Bits[] rightbyteMaskBits = new[] { Bits.Three };

        short expected = 0b0100_0100_0110_1010;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedShort(leftbyteMaskBits, rightbyteMaskBits)));
    }

    [Fact]
    public void Short_GetMaskedShortFor0b0100010101101110BitsOneThreeAndThreeSix_ReturnsExpectedResult()
    {
        short testData = 0b0100_0101_0110_1110;

        Bits[] leftbyteMaskBits = new[] { Bits.One, Bits.Three };
        Bits[] rightbyteMaskBits = new[] { Bits.Three, Bits.Six };

        short expected = 0b0100_0000_0100_1010;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedShort(leftbyteMaskBits, rightbyteMaskBits)));
    }

    [Fact]
    public void Short_GetMaskedShortForShortMaxValueBitsOneFourAndTwoFive_ReturnsExpectedResult()
    {
        short testData = short.MaxValue;

        Bits[] leftbyteMaskBits = new[] { Bits.One, Bits.Four };
        Bits[] rightbyteMaskBits = new[] { Bits.Two, Bits.Five };

        short expected = 0b111_0110_1110_1101;
        short see = testData.GetMaskedShort(leftbyteMaskBits, rightbyteMaskBits);

        Assertion(() => Assert.Equal(expected, testData.GetMaskedShort(leftbyteMaskBits, rightbyteMaskBits)));
    }
    #endregion

    #region GetMostSignificantBit

    [Fact]
    public void Short_GetMostSignificantBit_Returns0()
    {
        short testData = 0;
        int expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Short_GetMostSignificantBit_Returns14()
    {
        short testData = 12345;
        int expected = 14;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void Short_GetMostSignificantBit_Returns15()
    {
        short testData = 0b0100_0101_0110_1110;
        int expected = 15;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Theory]
    [MemberData(nameof(IntFixture.MostSignificantBit.ForPositiveShort), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
    public void RandomShort_GetMostSignificantBitPositiveValue_ReturnsExpectedResult(int actual, short testData)
    {
        int expected = testData.GetMostSignificantBit();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(IntFixture.MostSignificantBit.ForNegativeShort), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
    public void RandomShort_GetMostSignificantBitNegativeValue_ReturnsExpectedResult(int actual, short testData)
    {
        int expected = 16;

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion

    #region GetMostSignificantByte

    [Fact]
    public void Short_GetMostSignificantByte_Returns0()
    {
        short testData = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Short_GetMostSignificantByte_Returns1()
    {
        short testData = 23;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Short_GetMostSignificantByte_Returns2()
    {
        short testData = 0b0111_1111_0000_1111;
        byte expected = 2;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Short_GetMostSignificantByteShortMax_Returns2()
    {
        short testData = short.MaxValue;
        byte expected = 2;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    #endregion

    #region GetNumberOfDigits

    [Fact]
    public void Short_GetNumberOfDigitsFor0_Returns1()
    {
        short testData = 0;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Short_GetNumberOfDigits_Returns1()
    {
        short testData = 7;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Short_GetNumberOfDigits_Returns3()
    {
        short testData = 348;
        byte expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Short_GetNumberOfDigits_Returns4()
    {
        short testData = 8319;
        byte expected = 4;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void Short_GetNumberOfDigitsForShortMaxValue_ReturnsSpecsIntegerInt16MaxDigits()
    {
        short testData = short.MaxValue;
        byte expected = Specifications.Specs.Integer.Int16.MaxDigits;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    #endregion
}

