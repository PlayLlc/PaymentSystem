using System;

using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class UintTests : TestBase
{
    #region GetNumberOfDigits

    [Fact]
    public void Uint_GetNumberOfDigits10_ReturnsExpectedResult()
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

    #endregion

    #region GetMostSignificantBit

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

    #region GetSetBitCount

    [Fact]
    public void Uint_GetSetBitCount0_ReturnsExpectedResult()
    {
        uint testData = uint.MinValue;
        int expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void Uint_GetSetBitCount3_ReturnsExpectedResult()
    {
        uint testData = 0b0101_0001;
        int expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void Uint_GetSetBitCount5_ReturnsExpectedResult()
    {
        uint testData = 0b_1110_0010_0001;
        int expected = 5;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void Uint_GetSetBitCountUIntMaxValue_ReturnsExpectedResult()
    {
        uint testData = uint.MaxValue;
        int expected = Specs.Integer.Int32.BitCount;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    #endregion

    #region IsBitSet

    [Fact]
    public void Uint_IsBitSet0_ReturnsTrue()
    {
        uint testData = 0b001;
        byte expectedBitPosition = 1;

        Assertion(() => Assert.True(testData.IsBitSet(expectedBitPosition)));
    }

    [Fact]
    public void Uint_IsBitSet3_ReturnsFalse()
    {
        uint testData = 0b0001;
        byte expectedBitPosition = 3;

        Assertion(() => Assert.False(testData.IsBitSet(expectedBitPosition)));
    }

    [Fact]
    public void Uint_IsBitSet3_ReturnsTrue()
    {
        uint testData = 0b0101;
        byte expectedBitPosition = 3;

        Assertion(() => Assert.True(testData.IsBitSet(expectedBitPosition)));
    }

    [Fact]
    public void Uint_IsBitSetBitTwo_ReturnsTrue()
    {
        uint testData = 0b0010;
        Bits expectedBitTwo = Bits.Two;

        Assertion(() => Assert.True(testData.IsBitSet(expectedBitTwo)));
    }

    [Fact]
    public void Uint_IsBitSetBitEight_ReturnsTrue()
    {
        uint testData = 0b_1000_0000;
        Bits expectedBitEight = Bits.Eight;

        Assertion(() => Assert.True(testData.IsBitSet(expectedBitEight)));
    }

    [Fact]
    public void Uint_IsBitSetBitEight_ReturnsFalse()
    {
        uint testData = 0b_0100_0000;
        Bits expectedBitEight = Bits.Eight;

        Assertion(() => Assert.False(testData.IsBitSet(expectedBitEight)));
    }

    #endregion

    #region AreAnyBitsSet

    [Fact]
    public void Uint_AreAnyBitsSet_ReturnsTrue()
    {
        uint testData = 0b_0001_1000;
        uint bitsToCompare = 0b_0000_1000;

        Assertion(() => Assert.True(testData.AreAnyBitsSet(bitsToCompare)));
    }

    [Fact]
    public void Uint_AreAnyBitsSet_ReturnsFalse()
    {
        uint testData = 0b_0001_0000;
        uint bitsToCompare = 0b_0000_1000;

        Assertion(() => Assert.False(testData.AreAnyBitsSet(bitsToCompare)));
    }

    [Theory]
    [MemberData(nameof(IntFixture.ForUInt), 50, MemberType = typeof(IntFixture))]
    public void RandomUint_AreAnyBitsSet_AlwaysReturnsTrueForSameValue(uint testData)
    {
        Assertion(() => Assert.True(testData.AreAnyBitsSet(testData)));
    }

    #endregion

    #region ClearBits

    [Fact]
    public void Uint_ClearBits3And4_BitsAreCleared()
    {
        uint testData = 0b_0000_1111_0000_1111_0000_1111_0000_1100;
        uint bitsToClear = 0b1100;

        uint expectedOutput = 0b_0000_1111_0000_1111_0000_1111_0000_0000;

        Assertion(() => Assert.Equal(expectedOutput, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void Uint_ClearBits5and6_BitsAreCleared()
    {
        uint testData = 0b0000_1111_0000_1111_0000_1111_0011_1100;
        uint bitsToClear = 0b11_0000;

        uint expectedOutput = 0b0000_1111_0000_1111_0000_1111_0000_1100;
        Assertion(() => Assert.Equal(expectedOutput, testData.ClearBits(bitsToClear)));
    }

    [Theory]
    [MemberData(nameof(IntFixture.ForUInt), 50, MemberType = typeof(IntFixture))]
    public void RandomUint_ClearBits_ClearingSameValueResultsTo0(uint testData)
    {
        Assertion(() => Assert.True(testData.ClearBits(testData) == 0));
    }

    #endregion

    #region SetBit

    [Fact]
    public void Uint_SetBit15_ReturnsExpected()
    {
        uint testData = 0b1000_0000_1110_0000_1000_0000_1110_0000;
        byte bitToSet = 15;

        uint expectedOutput = 0b1000_0000_1110_0000_1000_0000_1110_0111;
        Assertion(() => Assert.Equal(expectedOutput, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void Uint_SetBit16_ReturnsExpected()
    {
        uint testData = 0b1000_0000_1110_0000_1000_0000_1110_0000;
        byte bitToSet = 16;

        uint expectedOutput = 0b1000_0000_1110_0000_1000_0000_1110_1000;
        Assertion(() => Assert.Equal(expectedOutput, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void Uint_SetBit9_ReturnsExpected()
    {
        uint testData = 0b1000_0000_1110_0000_1000_0000_1110_0000;
        byte bitToSet = 9;

        uint expectedOutput = 0b1000_0000_1110_0000_1000_0000_1110_0001;
        Assertion(() => Assert.Equal(expectedOutput, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void Uint_SetBit8_InputDoesntChange_ReturnsExpected()
    {
        uint testData = 0b1000_0000_1110_0000_1000_0000_1110_0000;
        byte bitToSet = 8;

        uint expectedOutput = 0b1000_0000_1110_0000_1000_0000_1110_0000;
        Assertion(() => Assert.Equal(expectedOutput, testData.SetBit(bitToSet)));
    }

    #endregion

    #region ClearBit

    [Fact]
    public void Uint_ClearBitFourOn2ndBytePosition_BitIsCleared()
    {
        uint testData = 0b1000_1000_1110_1000;
        Bits bitToClear = Bits.Four;
        byte bytePosition = 2;

        uint expectedOutput = 0b1000_0000_1110_1000;

        Assertion(() => Assert.Equal(expectedOutput, testData.ClearBit(bitToClear, bytePosition)));
    }

    [Fact]
    public void Uint_ClearBitThreeOn2ndBytePosition_BitIsCleared()
    {
        uint testData = 0b1000_0100_1110_1000;

        Bits bitToClear = Bits.Three;
        byte bytePosition = 2;

        uint expectedOutput = 0b1000_0000_1110_1000;
        Assertion(() => Assert.Equal(expectedOutput, testData.ClearBit(bitToClear, bytePosition)));
    }

    [Fact]
    public void Uint_ClearBitEightOn2ndBytePosition_BitIsCleared()
    {
        uint testData = 0b1000_0100_1110_1010;

        Bits bitToClear = Bits.Eight;
        byte bytePosition = 2;

        uint expectedOutput = 0b0100_1110_1010;
        Assertion(() => Assert.Equal(expectedOutput, testData.ClearBit(bitToClear, bytePosition)));
    }

    [Fact]
    public void Uint_ClearBitEightOn4thBytePosition_ReturnsSameValue()
    {
        uint testData = 0b1000_0100_1110_1000;

        Bits bitToClear = Bits.Eight;
        byte bytePosition = 4;

        uint expectedOutput = 0b1000_0100_1110_1000;
        Assertion(() => Assert.Equal(expectedOutput, testData.ClearBit(bitToClear, bytePosition)));
    }

    [Fact]
    public void Uint_ClearBitSixOn1stBytePosition_ReturnsCorrectValue()
    {
        uint testData = 0b1000_0100_1110_1010;

        Bits bitToClear = Bits.Six;
        byte bytePosition = 1;

        uint expectedOutput = 0b1000_0100_1100_1010;
        Assertion(() => Assert.Equal(expectedOutput, testData.ClearBit(bitToClear, bytePosition)));
    }

    [Fact]
    public void Uint_ClearBitOn5thByteForUInt_ArgumentOutOfRangeExceptionIsThrown()
    {
        uint testData = 0b1000_0100_1110_1000_1000_0100_1110_1000;
        Bits bitToClear = Bits.Eight;
        byte bytePosition = 5;

        Assertion(() => Assert.Throws<ArgumentOutOfRangeException>(() => testData.ClearBit(bitToClear, bytePosition)));
    }

    #endregion

    #region GetMaskedValue

    [Fact]
    public void Uint_GetMaskedValue_Value0b1100_0000_1101_1111_Mask0b0010_1111_ReturnsExpectedResult()
    {
        uint testData = 0b1100_0000_1101_1111;
        uint mask = 0b0010_1111;

        uint expectedOutput = 0b1101_0000;

        Assertion(() => Assert.Equal(expectedOutput, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Uint_GetMaskedValue_Value0b1100_0000_1101_1001_Mask0b0010_1001_ReturnsExpectedResult()
    {
        uint testData = 0b1100_0000_1101_1001;
        uint mask = 0b0010_1111;

        uint expectedOutput = 0b1101_0000;

        Assertion(() => Assert.Equal(expectedOutput, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Uint_GetMaskedValue_Value0b1100_0000_1101_1111_Mask0b0010_1001_ReturnsExpectedResult()
    {
        uint testData = 0b1100_0000_1101_1111;
        uint mask = 0b0010_1001;

        uint expectedOutput = 0b1101_0110;

        Assertion(() => Assert.Equal(expectedOutput, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Uint_GetMaskedValue_Value0b1100_0000_1111_1111_Mask0b0010_1001_ReturnsExpectedResult()
    {
        uint testData = 0b1100_0000_1111_1111;
        uint mask = 0b0010_1001;

        uint expectedOutput = 0b1101_0110;

        Assertion(() => Assert.Equal(expectedOutput, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Uint_GetMaskedValue_Value0b1100_0000_1111_1111_Mask0b1010_1001_ReturnsExpectedResult()
    {
        uint testData = 0b1100_0000_1111_1111;
        uint mask = 0b1010_1001;

        uint expectedOutput = 0b0101_0110;

        Assertion(() => Assert.Equal(expectedOutput, testData.GetMaskedValue(mask)));
    }

    #endregion

    #region GetMostSignificantByte

    [Fact]
    public void Uint_GetMostSignificantByte_Returns2ndByte()
    {
        uint testData = 0b1000_0101_1001_0000;
        byte expected = 2;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Uint_GetMostSignificantByte_Returns1stByte()
    {
        uint testData = 0b1001_0000;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void Uint_GetMostSignificantByte_Returns3rdByte()
    {
        uint testData = 0b0001_1011_0111_1111_1011_1000;
        byte expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    #endregion
}