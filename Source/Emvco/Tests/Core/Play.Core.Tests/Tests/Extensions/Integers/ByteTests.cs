using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class ByteTests : TestBase
{
    #region Instance Members
    #endregion

    #region GetNumberOfDigits

    [Fact]
    public void Byte_GetNumberOfDigits3_ReturnsExpectedResult()
    {
        byte testData = byte.MaxValue;
        int expected = Specs.Integer.UInt8.MaxDigits;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Byte_GetNumberOfDigits2_127_ReturnsExpectedResult()
    {
        byte testData = 127;
        int expected = 3;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Byte_GetNumberOfDigits2_75_ReturnsExpectedResult()
    {
        byte testData = 75;
        int expected = 2;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Byte_GetNumberOfDigits2_ReturnsExpectedResult()
    {
        byte testData = 12;
        int expected = 2;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Byte_GetNumberOfDigits0_ReturnsExpectedResult()
    {
        byte testData = 0;
        int expected = 1;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion

    #region GetMostSignificantBit

    [Fact]
    public void Byte_GetMostSignificantBit0_ReturnsExpectedResult()
    {
        byte testData = 0;
        int expected = 0;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Byte_GetMostSignificantBit14_ReturnsExpectedResult()
    {
        byte testData = 12;
        int expected = 4;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Byte_GetMostSignificantBit16_ReturnsExpectedResult()
    {
        byte testData = byte.MaxValue;
        int expected = Specs.Integer.UInt8.BitCount;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(IntFixture.MostSignificantBit.ForByte), 50, MemberType = typeof(IntFixture.MostSignificantBit))]
    public void Byte_GetMostSignificantBit_ReturnsExpectedResult(int actual, byte testData)
    {
        int expected = testData.GetMostSignificantBit();

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion

    #region AreBitsSet

    [Fact]
    public void Byte_AreBitsSetBit1_ReturnsTrue()
    {
        byte testData = 0b0101;
        byte bitsToCompare = 1;

        Assertion(() => Assert.True(testData.AreBitsSet(bitsToCompare)));
    }

    [Fact]
    public void Byte_AreBitsSetBit1_ReturnsFalse()
    {
        byte testData = 0b0100;
        byte bitsToCompare = 1;

        Assertion(() => Assert.False(testData.AreBitsSet(bitsToCompare)));
    }

    [Fact]
    public void Byte_AreBitsSet0b0011_1100_ReturnsTrue()
    {
        byte testData = 0b1111_1100;
        byte bitsToCompare = 0b0011_1100;

        Assertion(() => Assert.True(testData.AreBitsSet(bitsToCompare)));
    }

    [Fact]
    public void Byte_AreBitsSet0b0011_1100_ReturnsFalse()
    {
        byte testData = 0b1111_0100;
        byte bitsToCompare = 0b0011_1100;

        Assertion(() => Assert.False(testData.AreBitsSet(bitsToCompare)));
    }

    #endregion

    #region AreAnyBitsSet

    [Fact]
    public void Byte_AreAnyBitsSetFor0_ReturnsFalse()
    {
        byte testData = 0;
        byte bitsToCompare = 0b110;

        Assertion(() => Assert.False(testData.AreAnyBitsSet(bitsToCompare)));
    }

    [Fact]
    public void Byte_AreAnyBitsSet_BitsToCompare0b110_ReturnsTrue()
    {
        byte testData = 0b1110_1110;
        byte bitsToCompare = 0b110;

        Assertion(() => Assert.True(testData.AreAnyBitsSet(bitsToCompare)));
    }

    [Fact]
    public void Byte_AreAnyBitsSet_BitsToCompare_ReturnsFalse()
    {
        byte testData = 0b1110_1110;
        byte bitsToCompare = 0b10001;

        Assertion(() => Assert.False(testData.AreAnyBitsSet(bitsToCompare)));
    }

    #endregion

    #region GetSetBitCount

    [Fact]
    public void ULong_GetSetBitCount1_ReturnsExpectedResult()
    {
        byte testData = 0b0001;
        int expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void ULong_GetSetBitCount3_ReturnsExpectedResult()
    {
        byte testData = 0b1100_0001;
        int expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void Byte_GetSetBitCount0_ReturnsExpectedResult()
    {
        byte testData = 0;
        int expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    [Fact]
    public void Byte_GetSetBitCountByteMax_ReturnsExpectedResult()
    {
        byte testData = byte.MaxValue;
        int expected = Play.Core.Specifications.Specs.Integer.UInt8.BitCount;

        Assertion(() => Assert.Equal(expected, testData.GetSetBitCount()));
    }

    #endregion

    #region ClearBits

    [Fact]
    public void Byte_ClearBits_BitsToClear0b0001_ReturnsExpectedResult()
    {
        byte testData = 0b0101_0001;
        byte bitsToClear = 0b0001;

        byte expected = 0b0101_0000;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void Byte_ClearBits_BitsToClear0b01010001_ReturnsExpectedResult()
    {
        byte testData = 0b0101_0001;
        byte bitsToClear = 0b0101_0001;

        byte expected = 0b0000_0000;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void Byte_ClearBitsBitsBitsToClear0b01000001_ReturnsExpectedResult()
    {
        byte testData = 0b1100_0101;
        byte bitsToClear = 0b0100_0001;
        
        byte expected = 0b1000_0100;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Theory]
    [MemberData(nameof(IntFixture.ForByte), 50, MemberType = typeof(IntFixture))]
    public void Byte_ClearBitsSameValue_AllBitsAreAlwaysCleared(byte testData)
    {
        Assertion(() => Assert.Equal((byte)0, testData.ClearBits(testData)));
    }

    #endregion

    #region GetLeftNibble

    [Fact]
    public void Byte_GetLeftNibble_Returns0b11110000()
    {
        byte testData = byte.MaxValue;
        byte expected = 0b11110000;

        Assertion(() => Assert.Equal(expected, testData.GetLeftNibble()));
    }

    [Fact]
    public void Byte_GetLeftNibble_Returns0()
    {
        byte testData = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetLeftNibble()));
    }

    [Fact]
    public void Byte_GetLeftNibble_Returns0b10100000()
    {
        byte testData = 0b1010_0110;
        byte expected = 0b1010_0000;

        Assertion(() => Assert.Equal(expected, testData.GetLeftNibble()));
    }

    #endregion

    #region GetRightNibble

    [Fact]
    public void Byte_GetRightNibble_Returns0b00001111()
    {
        byte testData = byte.MaxValue;
        byte expected = 0b00001111;

        Assertion(() => Assert.Equal(expected, testData.GetRightNibble()));
    }

    [Fact]
    public void Byte_GetRightNibble_Returns0()
    {
        byte testData = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetRightNibble()));
    }

    [Fact]
    public void Byte_GetRightNibble_Returns0b00000110()
    {
        byte testData = 0b1010_0110;
        byte expected = 0b0000_0110;

        Assertion(() => Assert.Equal(expected, testData.GetRightNibble()));
    }

    #endregion

    #region ShiftNibbleLeft

    [Fact]
    public void Byte_ShiftNibbleLeftMaxValue_Returns0b1111()
    {
        byte testData = byte.MaxValue;
        byte rightNibble = 0b1111;
        byte expected = 0b1111;

        Assertion(() => Assert.Equal(expected, testData.ShiftNibbleLeft(rightNibble)));
    }

    [Fact]
    public void Byte_ShiftNibbleLeft0b10101010_Returns0b1111()
    {
        byte testData = 0b10101010;
        byte rightNibble = 0b1111;
        byte expected = 0b1111;

        Assertion(() => Assert.Equal(expected, testData.ShiftNibbleLeft(rightNibble)));
    }

    [Fact]
    public void Byte_ShiftNibbleLeft0b10101010_Returns0b101110()
    {
        byte testData = 0b10101010;
        byte rightNibble = 0b101110;
        byte expected = 0b101110;

        Assertion(() => Assert.Equal(expected, testData.ShiftNibbleLeft(rightNibble)));
    }

    #endregion

    //TODO ShiftNibbleRight

    #region GetMaskedValue

    [Fact]
    public void Byte_GetMaskedValueMask0_Returns0()
    {
        byte testData = 0;
        byte mask = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Byte_GetMaskedValueMask0b01_Returns0b0100()
    {
        byte testData = 0b0101;
        byte mask = 0b01;
        byte expected = 0b0100;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Byte_GetMaskedValueMask0b0101_Returns0b11000000()
    {
        byte testData = 0b1100_0101;
        byte mask = 0b0101;
        byte expected = 0b1100_0000;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Byte_GetMaskedValueMask0b11110000_Returns0b00001111()
    {
        byte testData = byte.MaxValue;
        byte mask = 0b1111_0000;
        byte expected = 0b0000_1111;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(mask)));
    }

    [Fact]
    public void Byte_GetMaskedValueBitsOne_Returns0b11111110()
    {
        byte testData = byte.MaxValue;
        Bits maskBit = Bits.One;
        byte expected = 0b11111110;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(maskBit)));
    }

    [Fact]
    public void Byte_GetMaskedValueBitsEight_Returns0b1111111()
    {
        byte testData = byte.MaxValue;
        Bits maskBit = Bits.Eight;
        byte expected = 0b1111111;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(maskBit)));
    }

    [Fact]
    public void Byte_GetMaskedValueBitsOneTwo_Returns0b11111100()
    {
        byte testData = byte.MaxValue;
        byte expected = 0b11111100;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(Bits.One, Bits.Two)));
    }

    [Fact]
    public void Byte_GetMaskedValueBitsFourFive_Returns0b11100111()
    {
        byte testData = byte.MaxValue;
        byte expected = 0b11100111;

        Assertion(() => Assert.Equal(expected, testData.GetMaskedValue(Bits.Four, Bits.Five)));
    }

    #endregion

    #region IsBitSet

    [Fact]
    public void Byte_IsBitSet1_ReturnsTrue()
    {
        byte testData = 0b1100_0001;
        byte bitPosition = 1;

        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Byte_IsBitSet1_ReturnsFalse()
    {
        byte testData = 0b1100_0010;
        byte bitPosition = 1;

        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Byte_IsBitSet7_ReturnsTrue()
    {
        byte testData = 0b0100_0010;
        byte bitPosition = 7;

        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Byte_IsBitSet7_ReturnsFalse()
    {
        byte testData = 0b0100_0010;
        byte bitPosition = 8;

        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Byte_IsBitSetBitOne_ReturnsTrue()
    {
        byte testData = 0b0100_0001;
        Bits bitPosition = Bits.One;

        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Byte_IsBitSetBitOne_ReturnsFalse()
    {
        byte testData = 0b0100_0010;
        Bits bitPosition = Bits.One;

        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Byte_IsBitSetBitFour_ReturnsTrue()
    {
        byte testData = 0b1001_1010;
        Bits bitPosition = Bits.Four;

        Assertion(() => Assert.True(testData.IsBitSet(bitPosition)));
    }

    [Fact]
    public void Byte_IsBitSetBitFour_ReturnsFalse()
    {
        byte testData = 0b1001_0110;
        Bits bitPosition = Bits.Four;

        Assertion(() => Assert.False(testData.IsBitSet(bitPosition)));
    }

    #endregion

    #region ReverseBits

    [Fact]
    public void Byte_ReverseBits0b10010110_ReturnsExpectedResult()
    {
        byte testData = 0b1001_0110;
        byte expected = 0b0110_1001;

        Assertion(() => Assert.Equal(expected, testData.ReverseBits()));
    }

    [Fact]
    public void Byte_ReverseBits0b1010_1101_ReturnsExpectedResult()
    {
        byte testData = 0b1010_1101;
        byte expected = 0b1011_0101;

        Assertion(() => Assert.Equal(expected, testData.ReverseBits()));
    }

    [Fact]
    public void Byte_ReverseBits0b1110_1101_ReturnsExpectedResult()
    {
        byte testData = 0b1110_1101;
        byte expected = 0b1011_0111;

        Assertion(() => Assert.Equal(expected, testData.ReverseBits()));
    }

    [Fact]
    public void Byte_ReverseBits1_ReturnsExpectedResult()
    {
        byte testData = 1;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.ReverseBits()));
    }

    #endregion

    #region SetBit

    [Fact]
    public void Byte_SetBit_Returns0b1000()
    {
        byte testData = 0;
        Bits bitToSet = Bits.Four;
        byte expected = 0b1000;
        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void Byte_SetBit_Returns0b10000()
    {
        byte testData = 0;
        Bits bitToSet = Bits.Five;
        byte expected = 0b10000;
        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void Byte_SetBit_Returns0b00000()
    {
        byte testData = 0;
        Bits bitToSet = Bits.One;
        byte expected = 1;
        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet)));
    }

    [Fact]
    public void Byte_SetBit_Returns0b10101111()
    {
        byte testData = 0b10001111;
        Bits bitToSet = Bits.Six;
        byte expected = 0b10101111;
        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet)));
    }

    #endregion

    #region SetBits

    [Fact]
    public void Byte_SetBitsOneTwo_Returns0b11()
    {
        byte testData = 0;

        byte expected = 0b11;
        Assertion(() => Assert.Equal(expected, testData.SetBits(Bits.One, Bits.Two)));
    }

    [Fact]
    public void Byte_SetBitsOneTwoFour_Returns0b1011()
    {
        byte testData = 0;

        byte expected = 0b1011;
        Assertion(() => Assert.Equal(expected, testData.SetBits(Bits.One, Bits.Two, Bits.Four)));
    }

    [Fact]
    public void Byte_SetBits6_Returns0b10101001()
    {
        byte testData = 0b10001001;

        byte expected = 0b10101001;
        Assertion(() => Assert.Equal(expected, testData.SetBits(Bits.Six)));
    }

    [Fact]
    public void Byte_SetBits0b101_Returns0b101()
    {
        byte testData = 0;
        byte bitsToSet = 0b101;

        byte expected = 0b101;
        Assertion(() => Assert.Equal(expected, testData.SetBits(bitsToSet)));
    }

    [Fact]
    public void Byte_SetBits0b0101_Returns0b10101111()
    {
        byte testData = 0b10101010;
        byte bitsToSet = 0b101;

        byte expected = 0b10101111;
        Assertion(() => Assert.Equal(expected, testData.SetBits(bitsToSet)));
    }

    #endregion

    #region ClearBits

    [Fact]
    public void Byte_ClearBits0_Returns0()
    {
        byte testData = 0;
        byte bitsToClear = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void Byte_ClearBits0b1011_Returns0()
    {
        byte testData = 0;
        byte bitsToClear = 0b1011;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void Byte_ClearBits0b1011From0b1011_Returns0()
    {
        byte testData = 0b1011;
        byte bitsToClear = 0b1011;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void Byte_ClearBits0b1011_Returns0b1011_0000()
    {
        byte testData = 0b1011_1011;
        byte bitsToClear = 0b1011;
        byte expected = 0b1011_0000;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void Byte_ClearBits0b1011_Returns0b1000_0011()
    {
        byte testData = 0b1011_1011;
        byte bitsToClear = 0b0011_1100;
        byte expected = 0b1000_0011;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void Byte_ClearBitsBitOne_Returns0()
    {
        byte testData = 0;
        Bits bitToClear = Bits.One;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitToClear)));
    }

    [Fact]
    public void Byte_ClearBitsBitFive_Returns0b1011()
    {
        byte testData = 0b11011;
        Bits bitToClear = Bits.Five;
        byte expected = 0b1011;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitToClear)));
    }

    #endregion
}