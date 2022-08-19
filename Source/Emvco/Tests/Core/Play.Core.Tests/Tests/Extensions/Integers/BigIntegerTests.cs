using System;
using System.Numerics;

using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class BigIntegerTests : TestBase
{
    #region Encode Then Decode

    [Theory]
    [MemberData(nameof(IntFixture.ForBigInteger), 50, MemberType = typeof(IntFixture))]
    public void RandomBigInteger_EncodingThenDecoding_ReturnsExpectedResult(BigInteger testData)
    {
        Span<byte> buffer = stackalloc byte[testData.GetByteCount()];
        testData.AsSpan(buffer);

        BigInteger actual = new(buffer);
        Assertion(() => Assert.Equal(testData, actual));
    }

    [Fact]
    public void BigInteger12345_EncodingThenDecoding_ReturnsExpectedResult()
    {
        BigInteger testData = 12345;
        Span<byte> buffer = stackalloc byte[testData.GetByteCount()];
        testData.AsSpan(buffer);

        BigInteger actual = new(buffer);
        Assertion(() => Assert.Equal(testData, actual));
    }

    #endregion

    #region AsSpan

    [Fact]
    public void BigInteger_AsSpan12345_ReturnsExpectedResult()
    {
        BigInteger testData = 12345;
        Span<byte> buffer = stackalloc byte[testData.GetByteCount()];
        testData.AsSpan(buffer);

        Span<byte> expectedBuffer = new byte[] {57, 48};

        Assert.Equal(expectedBuffer.ToArray(), buffer.ToArray());
    }

    [Fact]
    public void BigInteger_AsSpan9893412345_ReturnsExpectedResult()
    {
        BigInteger testData = 9893412345;
        Span<byte> buffer = stackalloc byte[testData.GetByteCount()];
        testData.AsSpan(buffer);

        Span<byte> expectedBuffer = new byte[] {249, 125, 177, 77, 2};

        Assert.Equal(expectedBuffer.ToArray(), buffer.ToArray());
    }

    [Fact]
    public void BigInteger_AsSpan9893412345_ThrowsArgumentOutOfRangeException()
    {
        BigInteger testData = 9893412345;

        Assertion(() => Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Span<byte> buffer = stackalloc byte[testData.GetByteCount() - 3];
            testData.AsSpan(buffer);
        }));
    }

    [Theory]
    [MemberData(nameof(IntFixture.ForBigInteger), 50, MemberType = typeof(IntFixture))]
    public void RandomBigInteger_AsSpan_ReturnsExpectedResult(BigInteger testData)
    {
        Span<byte> buffer = stackalloc byte[testData.GetByteCount()];
        testData.AsSpan(buffer);

        Span<byte> expectedBuffer = testData.ToByteArray();

        Assert.Equal(expectedBuffer.ToArray(), buffer.ToArray());
    }

    #endregion

    #region AreBitsSet

    [Fact]
    public void BigInteger_AreBitsSet0b1100_0111_ReturnsTrue()
    {
        BigInteger testData = 0b1100_0111;
        BigInteger valueToCheck = 0b1000_0000;

        Assertion(() => Assert.True(testData.AreBitsSet(valueToCheck)));
    }

    [Fact]
    public void BigInteger_AreBitsSet0b1100_0111_ReturnsFalse()
    {
        BigInteger testData = 0b1100_0111;
        BigInteger valueToCheck = 0b0011_0000;

        Assertion(() => Assert.False(testData.AreBitsSet(valueToCheck)));
    }

    [Fact]
    public void BigInteger_AreBitsSet0b1100_1100_0111_ReturnsTrue()
    {
        BigInteger testData = 0b1100_1100_0111;
        BigInteger valueToCheck = 0b0100_0011_0000;

        Assertion(() => Assert.True(testData.AreBitsSet(valueToCheck)));
    }

    #endregion

    #region ClearBits

    [Fact]
    public void BigInteger_ClearBits0b1011_0110_1001_1111_0000_ReturnsExpectedResult()
    {
        BigInteger testData = 0b1011_0110_1001_1111_0000;
        BigInteger bitsToClear = 0b0100_0100_0000_1001_1111;
        BigInteger expected = 0b1011_0010_1001_0110_0000;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void BigInteger_ClearBits0_ReturnsExpectedResult()
    {
        BigInteger testData = 0;
        BigInteger bitsToClear = 0b0100_0100_0000_1001_1111;
        BigInteger expected = 0;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    [Fact]
    public void BigInteger_ClearBits0b11111111111111111111111111111111111111_ReturnsExpectedResult()
    {
        BigInteger testData = 0b11111111111111111111111111111111111111;
        BigInteger bitsToClear = 0b11111111111111111111111111111111111111;
        BigInteger expected = 0;

        Assertion(() => Assert.Equal(expected, testData.ClearBits(bitsToClear)));
    }

    #endregion

    #region GetMostSignificantBit

    [Fact]
    public void BigInteger_GetMostSignificantBit_Returns0()
    {
        BigInteger testData = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantBit_Returns1()
    {
        BigInteger testData = 0b10;
        byte expected = 2;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantBit_ReturnsUShortMaxValueMaxBitCount()
    {
        BigInteger testData = ushort.MaxValue;
        byte expected = Specs.Integer.UInt16.BitCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantBit_ReturnsUIntMaxValueBitCount()
    {
        BigInteger testData = uint.MaxValue;
        byte expected = Specs.Integer.UInt32.BitCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantBit_ReturnsULongMaxValueBitCount()
    {
        BigInteger testData = ulong.MaxValue;
        byte expected = Specs.Integer.UInt64.BitCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantBit_ReturnsShortMaxValueBitCountWithoutSignedBit()
    {
        BigInteger testData = short.MaxValue;
        byte expected = Specs.Integer.UInt16.BitCount - 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantBit_ReturnsIntMaxValueBitCountWithoutSignedBit()
    {
        BigInteger testData = int.MaxValue;
        byte expected = Specs.Integer.UInt32.BitCount - 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantBit_ReturnsLongMaxValueBitCountWithoutSignedBit()
    {
        BigInteger testData = long.MaxValue;
        byte expected = Specs.Integer.Int64.BitCount - 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantBit()));
    }

    #endregion

    #region GetMostSignificantByte

    [Fact]
    public void BigInteger_GetMostSignificantByte_Returns0()
    {
        BigInteger testData = 0;
        byte expected = 0;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantByte_Returns1()
    {
        BigInteger testData = 0b10;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantByte_Returns2()
    {
        BigInteger testData = 0b0110_1100_0000_1111;
        byte expected = 2;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantByte_ReturnsUShortMaxValueByteCount()
    {
        BigInteger testData = ushort.MaxValue;
        byte expected = Specs.Integer.UInt16.ByteCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantByte_ReturnsShortMaxValueByteCount()
    {
        BigInteger testData = short.MaxValue;
        byte expected = Specs.Integer.UInt16.ByteCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantByte_ReturnsUIntMaxValueByteCount()
    {
        BigInteger testData = uint.MaxValue;
        byte expected = Specs.Integer.UInt32.ByteCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    [Fact]
    public void BigInteger_GetMostSignificantByte_ReturnsIntMaxValueByteCount()
    {
        BigInteger testData = int.MaxValue;
        byte expected = Specs.Integer.Int32.ByteCount;

        Assertion(() => Assert.Equal(expected, testData.GetMostSignificantByte()));
    }

    #endregion

    #region GetNumberOfDigits

    [Fact]
    public void BigInteger_GetNumberOfDigits_Returns1()
    {
        BigInteger testData = 0;
        byte expected = 1;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void BigInteger_GetNumberOfDigits_Returns3()
    {
        BigInteger testData = 231;
        byte expected = 3;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void BigInteger_GetNumberOfDigits_UShortMaxValueNumberOfDigits()
    {
        BigInteger testData = ushort.MaxValue;
        byte expected = Specs.Integer.UInt16.MaxDigits;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    [Fact]
    public void BigInteger_GetNumberOfDigits_ULongMaxValueNumberOfDigits()
    {
        BigInteger testData = ulong.MaxValue;
        byte expected = Specs.Integer.UInt64.MaxDigits;

        Assertion(() => Assert.Equal(expected, testData.GetNumberOfDigits()));
    }

    #endregion

    #region IsBitSet

    [Fact]
    public void BigInteger_IsBitSet0_ReturnsFalse()
    {
        BigInteger testData = 0;
        byte bitPositionToCheck = 1;

        Assertion(() => Assert.False(testData.IsBitSet(bitPositionToCheck)));
    }

    [Fact]
    public void BigInteger_IsBitSet5_ReturnsTrue()
    {
        BigInteger testData = 0b1101_1001;
        byte bitPositionToCheck = 5;

        Assertion(() => Assert.True(testData.IsBitSet(bitPositionToCheck)));
    }

    [Fact]
    public void BigInteger_IsBitSet11_ReturnsTrue()
    {
        BigInteger testData = 0b1101_1101_1101_1001;
        byte bitPositionToCheck = 11;

        Assertion(() => Assert.True(testData.IsBitSet(bitPositionToCheck)));
    }

    [Fact]
    public void BigInteger_IsBitSet11_ReturnsFalse()
    {
        BigInteger testData = 0b1101_1001_1101_1001;
        byte bitPositionToCheck = 11;

        Assertion(() => Assert.False(testData.IsBitSet(bitPositionToCheck)));
    }

    [Fact]
    public void BigInteger_IsBitSet38_ReturnsTrue()
    {
        BigInteger testData = 0b1101_1001_1111_1001_1101_1001_1101_1001_1101_1001_1101_1001;
        byte bitPositionToCheck = 38;

        Assertion(() => Assert.True(testData.IsBitSet(bitPositionToCheck)));
    }

    [Fact]
    public void BigInteger_IsBitSet38_ReturnsFalse()
    {
        BigInteger testData = 0b1101_1001_1101_1001_1001_1001_1101_1001_1101_1001_1101_1001;
        byte bitPositionToCheck = 38;

        Assertion(() => Assert.False(testData.IsBitSet(bitPositionToCheck)));
    }

    #endregion

    #region SetBit

    [Fact]
    public void BigInteger_SetBit1_ReturnsExpectedResult()
    {
        BigInteger testData = 0;
        Bits bitToSet = Bits.One;
        byte bytePosition = 1;
        BigInteger expected = 1;

        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet, bytePosition)));
    }

    [Fact]
    public void BigInteger_SetBit8_ReturnsExpectedResult()
    {
        BigInteger testData = 0;
        Bits bitToSet = Bits.Eight;
        byte bytePosition = 1;
        BigInteger expected = 0b10000000;

        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet, bytePosition)));
    }

    [Fact]
    public void BigInteger_SetBit27_ReturnsExpectedResult()
    {
        BigInteger testData = 0;
        Bits bitToSet = Bits.Three;
        byte bytePosition = 4;
        BigInteger expected = 0b100000000000000000000000000;

        Assertion(() => Assert.Equal(expected, testData.SetBit(bitToSet, bytePosition)));
    }

    #endregion

    #region TryGetRemainder

    [Fact]
    public void BigInteger_TryGetRemainderFor127_ReturnsExpectedResult()
    {
        BigInteger testData = 127;
        int divisor = 3;

        int remainder = testData.TryGetRemainder(divisor, out BigInteger resultWithoutRemainder);

        Assertion(() => Assert.Equal(1, remainder));
        Assertion(() => Assert.Equal(42, resultWithoutRemainder));
    }

    [Fact]
    public void BigInteger_TryGetRemainderFor27_ReturnsExpectedResult()
    {
        BigInteger testData = 27;
        int divisor = 3;

        int remainder = testData.TryGetRemainder(divisor, out BigInteger resultWithoutRemainder);

        Assertion(() => Assert.Equal(0, remainder));
        Assertion(() => Assert.Equal(9, resultWithoutRemainder));
    }

    #endregion
}