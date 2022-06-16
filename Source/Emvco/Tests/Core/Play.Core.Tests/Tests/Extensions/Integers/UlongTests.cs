using System;

using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class UlongTests : TestBase
{
    #region GetNumberOfDigits

    [Fact]
    public void Ulong_GetNumberOfDigits20_ReturnsExpectedResult()
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

    #endregion

    #region GetMostSignificantBit

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
    public void RandomULong_InvokesGetMostSignificantBit_ReturnsExpectedResult(int actual, ulong testData)
    {
        int expected = testData.GetMostSignificantBit();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion

    #region AreBitsSet

    [Fact]
    public void ULong_AreBitsSetBit1_ReturnsTrue()
    {
        ulong testData = 0b0000_0001;
        ulong bitsToCompare = 0b0001;

        Assertion(() => Assert.True(testData.AreBitsSet(bitsToCompare)));
    }

    [Fact]
    public void ULong_AreBitsSetBit1_ReturnsFalse()
    {
        ulong testData = 0b1000_0000;
        ulong bitsToCompare = 0b0001;

        Assertion(() => Assert.False(testData.AreBitsSet(bitsToCompare)));
    }

    [Fact]
    public void ULong_AreBitsSetBit5_ReturnsTrue()
    {
        ulong testData = 0b0001_0001;
        ulong bitsToCompare = 0b0001_0000;

        Assertion(() => Assert.True(testData.AreBitsSet(bitsToCompare)));
    }

    [Fact]
    public void ULong_AreBitsSetBit5_ReturnsFalse()
    {
        ulong testData = 0b0000_0001;
        ulong bitsToCompare = 0b0001_0000;

        Assertion(() => Assert.False(testData.AreBitsSet(bitsToCompare)));
    }

    [Fact]
    public void ULong_AreBitsSetBit64_ReturnsTrue()
    {
        ulong testData = 0b1111111111111111111111111111111111111111111111111111111111111111;
        ulong bitsToCompare = 0b1000000000000000000000000000000000000000000000000000000000000000;

        Assertion(() => Assert.True(testData.AreBitsSet(bitsToCompare)));
    }

    [Fact]
    public void ULong_AreBitsSetBit64_ReturnsFalse()
    {
        ulong testData = 0b1000000000000000000000000000000000000000000000000000000000000000;
        ulong bitsToCompare = 0b0100000000000000000000000000000000000000000000000000000000000000;

        Assertion(() => Assert.False(testData.AreBitsSet(bitsToCompare)));
    }

    #endregion

    #region AreAnyBitsSet

    [Fact]
    public void ULong_AreAnyBitsSetBit64IsSet_ReturnsTrue()
    {
        ulong testData = 0b1111111111111111111111111111111111111111111111111111111111111111;
        ulong bitsToCompare = 0b1000000000000000000000000000000000000000000000000000000000000000;

        Assertion(() => Assert.True(testData.AreAnyBitsSet(bitsToCompare)));
    }

    [Fact]
    public void ULong_AreAnyBitsSetNoBitSet_ReturnsFalse()
    {
        ulong testData = 0b0111111111111111111111111111111111111111111111111111111111111111;
        ulong bitsToCompare = 0b1000000000000000000000000000000000000000000000000000000000000000;

        Assertion(() => Assert.False(testData.AreAnyBitsSet(bitsToCompare)));
    }

    [Theory]
    [MemberData(nameof(IntFixture.ForULong), 50, MemberType = typeof(IntFixture))]
    public void RandomULong_AreAnyBitsSet_ReturnsAlwaysTrueForSameBitsToCompare(ulong testData)
    {
        Assertion(() => Assert.True(testData.AreAnyBitsSet(testData)));
    }

    #endregion

    #region GetSetBitCount

    [Fact]
    public void ULong_GetSetBitCount1_ReturnsExpectedResult()
    {
        ulong testData = 0b0001;
        int expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void ULong_GetSetBitCount3_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_0001;
        int expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void ULong_GetSetBitCount0_ReturnsExpectedResult()
    {
        ulong testData = 0;
        int expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void ULong_GetSetBitCountULongMax_ReturnsExpectedResult()
    {
        ulong testData = ulong.MaxValue;
        int expected = Specs.Integer.Int64.BitCount;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    #endregion

    #region ClearBit

    [Fact]
    public void ULong_ClearBitBit1BytePosition1_ReturnsExpectedResult()
    {
        ulong testData = 0b0000_0001;
        Bits bitToClear = Bits.One;
        byte bytePosition = 1;

        ulong expected = 0b0000_0000;

        Assertion(() => Assert.Equal(expected, testData.ClearBit(bitToClear, bytePosition)));
    }

    [Fact]
    public void ULong_ClearBitBit1BytePosition2_ReturnsExpectedResult()
    {
        ulong testData = 0b0000_0001_0000_0001;
        Bits bitToClear = Bits.One;
        byte bytePosition = 2;

        ulong expected = 0b0000_0000_0000_0001;

        Assertion(() => Assert.Equal(expected, testData.ClearBit(bitToClear, bytePosition)));
    }

    [Fact]
    public void ULong_ClearBitBit3BytePosition3_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_0101_0110_0101_0000_0001_0000_0001;
        Bits bitToClear = Bits.Three;
        byte bytePosition = 3;

        ulong expected = 0b1100_0101_0110_0001_0000_0001_0000_0001;

        Assertion(() => Assert.Equal(expected, testData.ClearBit(bitToClear, bytePosition)));
    }

    [Fact]
    public void ULong_ClearBitBit3BytePosition4_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_0101_0110_0101_0000_0001_0000_0001;
        Bits bitToClear = Bits.Three;
        byte bytePosition = 4;

        ulong expected = 0b1100_0001_0110_0101_0000_0001_0000_0001;

        Assertion(() => Assert.Equal(expected, testData.ClearBit(bitToClear, bytePosition)));
    }

    [Fact]
    public void ULong_ClearBitBit1BytePosition9_ThrowsException()
    {
        ulong testData = 0b1100_0101_0110_0101_0000_0001_0000_0001;
        Bits bitToClear = Bits.Three;
        byte bytePosition = 9;

        Assertion(() => Assert.Throws<ArgumentOutOfRangeException>(() => testData.ClearBit(bitToClear, bytePosition)));
    }

    #endregion

    #region ClearBits

    [Fact]
    public void ULong_ClearBitsBits1_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_0101_0110_0101_0010_0001_0101_0001;
        ulong bitsToClear = 0b0001;

        ulong expected = 0b1100_0101_0110_0101_0010_0001_0101_0000;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void ULong_ClearBitsBits157_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_0101_0110_0101_0010_0001_0101_0001;
        ulong bitsToClear = 0b0101_0001;

        ulong expected = 0b1100_0101_0110_0101_0010_0001_0000_0000;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void ULong_ClearBitsBits17_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_0101_0110_0101_0010_0001_0101_0001;
        ulong bitsToClear = 0b0100_0001;

        ulong expected = 0b1100_0101_0110_0101_0010_0001_0001_0000;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Theory]
    [MemberData(nameof(IntFixture.ForULong), 50, MemberType = typeof(IntFixture))]
    public void ULong_ClearBitsSameValue_AllBitsAreAlwaysCleared(ulong testData)
    {
        Assertion(() => Assert.Equal((ulong) 0, testData.ClearBits(testData)));
    }

    #endregion

    #region GetMaskedValue

    [Fact]
    public void ULong_GetMaskedValueMask0b0101_0001_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_0101_0110_0101_0010_0001_0101_0001;
        ulong bitMaskValue = 0b0101_0001;

        ulong expected = 0b1100_0101_0110_0101_0010_0001_0000_0000;
        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(bitMaskValue)));
    }

    [Fact]
    public void ULong_GetMaskedValueMask0b1100_0101_0110_0101_0010_0001_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_0101_0110_0101_0010_0001_0101_0001;
        ulong bitMaskValue = 0b1100_0101_0110_0101_0010_0001_0000_0000;

        ulong expected = 0b0000_0000_0000_0000_0000_0000_0101_0001;
        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(bitMaskValue)));
    }

    [Fact]
    public void ULong_GetMaskedValueMask0b1100_0101_0000_0000_0010_0001_0101_0001_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_0101_0110_0101_0010_0001_0101_0001;
        ulong bitMaskValue = 0b1100_0101_0000_0000_0010_0001_0101_0001;

        ulong expected = 0b0000_0000_0110_0101_0000_0000_0000_0000;
        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(bitMaskValue)));
    }

    #endregion

    #region IsBitSet

    [Fact]
    public void ULong_IsBitSet1_ReturnsTrue()
    {
        ulong testData = 0b1100_0001;
        byte bitPosition = 1;

        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void ULong_IsBitSet1_ReturnsFalse()
    {
        ulong testData = 0b1100_0010;
        byte bitPosition = 1;

        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void ULong_IsBitSet7_ReturnsTrue()
    {
        ulong testData = 0b0100_0010;
        byte bitPosition = 7;

        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void ULong_IsBitSet7_ReturnsFalse()
    {
        ulong testData = 0b0100_0010;
        byte bitPosition = 8;
        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void ULong_IsBitSetBitOne_ReturnsTrue()
    {
        ulong testData = 0b0100_0001;
        Bits bitPosition = Bits.One;

        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void ULong_IsBitSetBitOne_ReturnsFalse()
    {
        ulong testData = 0b0100_0010;
        Bits bitPosition = Bits.One;

        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void ULong_IsBitSetBitFour_ReturnsTrue()
    {
        ulong testData = 0b1001_1010;
        Bits bitPosition = Bits.Four;

        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void ULong_IsBitSetBitFour_ReturnsFalse()
    {
        ulong testData = 0b1001_0110;
        Bits bitPosition = Bits.Four;

        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    #endregion

    #region SetBit

    [Fact]
    public void ULong_SetBit_Returns0b1000()
    {
        ulong testData = 0;
        byte bitToSet = 4;
        ulong expected = 0b1000;

        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void ULong_SetBitPosition7_ReturnsExpectedResult()
    {
        ulong testData = 0b1100_1010_0011_1111_0111_1000;
        byte bitPosition = 8;
        ulong expected = 0b1100_1010_0011_1111_1111_1000;

        Assertion(() => Assert.Equal(expected, testData.SetBit(bitPosition)));
    }

    [Fact]
    public void ULong_SetBitPosition5_ReturnsExpectedResult()
    {
        ulong testData = 0b11111;
        byte bitPosition = 5;
        ulong expected = 0b11111;

        Assertion(() => Assert.Equal(expected, testData.SetBit(bitPosition)));
    }

    [Fact]
    public void ULong_SetBitPosition65_ThrowsArgumentOutOfRangeException()
    {
        ulong testData = 0;
        byte bitPosition = 65;

        Assertion(() => Assert.Throws<ArgumentOutOfRangeException>(() => testData.SetBit(bitPosition)));
    }

    #endregion

    #region SetBits

    [Fact]
    public void ULong_SetBits_Returns0b0111()
    {
        ulong testData = 0;
        ulong bitsToSet = 0b0111;

        ulong expected = 0b0111;
        Assertion(() => Assert.Equal(expected, testData.SetBits(bitsToSet)));
    }

    [Fact]
    public void ULong_SetBits_Returns0b110111()
    {
        ulong testData = 0b100001;
        ulong bitsToSet = 0b010111;

        ulong expected = 0b110111;
        Assertion(() => Assert.Equal(expected, testData.SetBits(bitsToSet)));
    }

    [Fact]
    public void ULong_SetBits_Returns0b1111_0000_1111()
    {
        ulong testData = 0b1100_0000_0100;
        ulong bitsToSet = 0b0011_0000_1111;

        ulong expected = 0b1111_0000_1111;
        Assertion(() => Assert.Equal(expected, testData.SetBits(bitsToSet)));
    }

    #endregion

    #region GetMostSignificantByte

    [Fact]
    public void ULong_GetMostSignificantByte_ReturnsByte1()
    {
        ulong testData = 0b0001;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void ULong_GetMostSignificantByte_ReturnsByte2()
    {
        ulong testData = 0b0001_0001_0001_0001;
        byte expected = 2;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void ULong_GetMostSignificantByte_ReturnsByte5()
    {
        ulong testData = 0b0001_1100_0011_1010_0011_1001_1111_0000_1011;
        byte expected = 5;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    #endregion

    #region ClearBit

    [Fact]
    public void ULong_ClearBit1_ReturnsExpectedResult()
    {
        ulong testData = 0b0001;
        byte bitToClear = 1;

        ulong expected = 0b0000;
        Assertion(() => Assert.Equal(expected, testData.ClearBit(bitToClear)));
    }

    [Fact]
    public void ULong_ClearBit4_ReturnsExpectedResult()
    {
        ulong testData = 0b1101;
        byte bitToClear = 4;

        ulong expected = 0b0101;
        Assertion(() => Assert.Equal(expected, testData.ClearBit(bitToClear)));
    }

    [Fact]
    public void ULong_ClearBit21_ReturnsExpectedResult()
    {
        ulong testData = 0b1010_1101_0101_0000_1111_0110_1001_1000;
        byte bitToClear = 21;

        ulong expected = 0b1010_1101_0100_0000_1111_0110_1001_1000;
        Assertion(() => Assert.Equal(expected, testData.ClearBit(bitToClear)));
    }

    [Fact]
    public void ULong_ClearBit26_ReturnsExpectedResult()
    {
        ulong testData = 0b1010_1101_0101_0000_1111_0110_1001_1000;
        byte bitToClear = 26;

        ulong expected = 0b1010_1101_0101_0000_1111_0110_1001_1000;
        Assertion(() => Assert.Equal(expected, testData.ClearBit(bitToClear)));
    }

    [Fact]
    public void ULong_ClearBit65_ThrowsArgumentOutOfRangeException()
    {
        ulong testData = ulong.MaxValue;
        byte bitToClear = 65;

        Assertion(() => Assert.Throws<ArgumentOutOfRangeException>(() => testData.ClearBit(bitToClear)));
    }

    #endregion
}