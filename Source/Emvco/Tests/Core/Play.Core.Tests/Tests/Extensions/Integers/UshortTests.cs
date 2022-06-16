using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class UshortTests : TestBase
{
    #region GetNumberOfDigits

    [Fact]
    public void Ushort_GetNumberOfDigits_Returns5()
    {
        ushort testData = 12345;
        int expected = Specs.Integer.UInt16.MaxDigits;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ushort_GetNumberOfDigits_Returns3()
    {
        ushort testData = 123;
        int expected = 3;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ushort_GetNumberOfDigits_Returns0()
    {
        ushort testData = 0;
        int expected = 1;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion

    #region GetMostSignificantBit

    [Fact]
    public void Ushort_GetMostSignificantBit_Returns0()
    {
        ushort testData = 0;
        int expected = 0;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ushort_GetMostSignificantBit_Returns14()
    {
        ushort testData = 12345;
        int expected = 14;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ushort_GetMostSignificantBit_Returns11()
    {
        ushort testData = 0b0000010110110110;
        int expected = 11;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ushort_GetMostSignificantBit_Returns16()
    {
        ushort testData = ushort.MaxValue;
        int expected = Specs.Integer.UInt16.BitCount;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(IntFixture.MostSignificantBit.ForUShort), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
    public void Ushort_RandomGetMostSignificantBit_ReturnsExpectedResult(int actual, ushort testData)
    {
        int expected = testData.GetMostSignificantBit();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion

    #region GetMaskedValue

    [Fact]
    public void Ushort_GetMaskedValue_Returns0b101000()
    {
        ushort testData = 0b101010;
        ushort mask = 0b000010;
        ushort expected = 0b101000;

        Assertion(() => Assert.Equal(testData.GetMaskedValue(mask), expected));
    }

    [Fact]
    public void Ushort_GetMaskedValue_Returns0b1()
    {
        ushort testData = 0b11111111;
        ushort mask = 0b11111110;
        ushort expected = 0b1;

        Assertion(() => Assert.Equal(testData.GetMaskedValue(mask), expected));
    }

    [Fact]
    public void Ushort_GetMaskedValue_Returns0b0()
    {
        ushort testData = 0b11111110;
        ushort mask = 0b11111110;
        ushort expected = 0b0;

        Assertion(() => Assert.Equal(testData.GetMaskedValue(mask), expected));
    }

    #endregion

    #region AreAnyBitsSet

    [Fact]
    public void Ushort_AreAnyBitsSet1_ReturnsTrue()
    {
        ushort testData = 0b101010;
        ushort bitsToCompare = 0b100000;

        Assertion(() => Assert.True(testData.AreAnyBitsSet(bitsToCompare)));
    }

    [Fact]
    public void Ushort_AreAnyBitsSet3_ReturnsTrue()
    {
        ushort testData = 0b101010;
        ushort bitsToCompare = 0b101010;

        Assertion(() => Assert.True(testData.AreAnyBitsSet(bitsToCompare)));
    }

    [Fact]
    public void Ushort_AreAnyBitsSet0_ReturnsFalse()
    {
        ushort testData = 0b101010;
        ushort bitsToCompare = 0b010100;

        Assertion(() => Assert.False(testData.AreAnyBitsSet(bitsToCompare)));
    }

    #endregion

    #region GetSetBitCount

    [Fact]
    public void Ushort_GetSetBitCount3_ReturnsExpectedResult()
    {
        ushort testData = 0b101010;
        ushort expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void Ushort_GetSetBitCount16_ReturnsExpectedResult()
    {
        ushort testData = ushort.MaxValue;
        ushort expected = 16;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void Ushort_GetSetBitCount0_ReturnsExpectedResult()
    {
        ushort testData = ushort.MinValue;
        ushort expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    #endregion

    #region GetMostSignificantByte

    [Fact]
    public void Ushort_GetMostSignificantByte_Returns0()
    {
        ushort testData = 0;
        int expected = 0;
        int actual = testData.GetMostSignificantByte();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ushort_GetMostSignificantByte_Returns1()
    {
        ushort testData = 0xFF;
        int expected = 1;
        int actual = testData.GetMostSignificantByte();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Ushort_GetMostSignificantByte1_Returns2()
    {
        ushort testData = 0xFFFF;
        int expected = 2;
        int actual = testData.GetMostSignificantByte();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion

    #region IsBitSet

    [Fact]
    public void Ushort_IsBitSet2_ReturnsTrue()
    {
        ushort testData = 0b11111110;
        byte bitPosition = 2;
        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Ushort_IsBitSet2_ReturnsFalse()
    {
        ushort testData = 0b11111100;
        byte bitPosition = 2;
        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Ushort_IsBitSet0_ReturnsFalse()
    {
        ushort testData = 0b11111110;
        byte bitPosition = 0;
        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Ushort_IsBitSetBitsEight_ReturnsTrue()
    {
        ushort testData = ushort.MaxValue;
        Bits bitPosition = Bits.Eight;
        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Ushort_IsBitSetBitsTwo_ReturnsTrue()
    {
        ushort testData = 0b11111110;
        Bits bitPosition = Bits.Two;
        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Ushort_IsBitSetBitsOne_ReturnsFalse()
    {
        ushort testData = 0b11111110;
        Bits bitPosition = Bits.One;
        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Ushort_IsBitSet15_ReturnsTrue()
    {
        ushort testData = ushort.MaxValue;
        byte bitPosition = Specs.Integer.Int16.BitCount;
        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    #endregion

    #region IsEven

    [Fact]
    public void Ushort_IsEven_ReturnsTrue()
    {
        ushort testData = 8;
        Assertion(() => Assert.True(testData.IsEven()));
    }

    [Fact]
    public void Ushort_IsEven_ReturnsFalse()
    {
        ushort testData = 7;
        Assertion(() => Assert.False(testData.IsEven()));
    }

    #endregion

    #region SetBit

    [Fact]
    public void Ushort_SetBit_Returns0b1000()
    {
        ushort testData = 0;
        byte bitToSet = 4;
        ushort expected = 0b1000;
        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void Ushort_SetBit_Returns0b10000()
    {
        ushort testData = 0;
        byte bitToSet = 5;
        ushort expected = 0b10000;
        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void Ushort_SetBit_Returns0b00000()
    {
        ushort testData = 0;
        byte bitToSet = 0;
        ushort expected = 0;
        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void Ushort_SetBit_Returns0b11111()
    {
        ushort testData = 0b11111;
        byte bitToSet = 5;
        ushort expected = 0b11111;
        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet)));
    }

    #endregion
}