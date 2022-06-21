using System;
using System.Numerics;

using Play.Core.Extensions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.ReadOnlySpan;

public class ReadonlySpanByteTests : TestBase
{
    #region AsBigInteger

    [Fact]
    public void ReadOnlySpanByte_AsBigInteger_Returns197121()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 1, 2, 3 };
            BigInteger expected = 197121;

            Assert.Equal(expected, testData.AsBigInteger());
        });
    }

    [Fact]
    public void ReadOnlySpanByte_AsBigInteger_Returns65793()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 1, 1, 1 };
            BigInteger expected = 65793;

            Assert.Equal(expected, testData.AsBigInteger());
        });
    }

    #endregion

    #region IsValueEqual

    [Fact]
    public void ReadOnlySpanByte_InvokesIsValueEqual_ReturnsTrue()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> actual = stackalloc byte[] { 1, 2, 3 };
            ReadOnlySpan<byte> expected = stackalloc byte[] { 1, 2, 3 };

            Assert.True(expected.IsValueEqual(actual));
        });
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesIsValueEqual_ReturnsFalse()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> actual = stackalloc byte[] { 1, 2, 3, 4 };
            ReadOnlySpan<byte> expected = stackalloc byte[] { 1, 2, 3 };

            Assert.False(expected.IsValueEqual(actual));
        });
    }

    #endregion

    #region ConcatArrays

    [Fact]
    public void ReadOnlySpanByte_InvokesConcatArrays_ReturnsExpectedResult()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 78, 32 };
            ReadOnlySpan<byte> other = stackalloc byte[] { 22, 13 };

            byte[] expected = new byte[] { 78, 32, 22, 13 };

            Assert.Equal(expected, testData.ConcatArrays(other));
        });
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesConcatArrays_ReturnsExpectedResultWithCorrectLength()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 78, 32 };
            ReadOnlySpan<byte> other = stackalloc byte[] { 22, 13 };

            byte[] expected = new byte[] { 78, 32, 22, 13 };

            Assert.Equal(expected, testData.ConcatArrays(other));
            Assert.Equal(expected.Length, testData.Length + other.Length); //maybe theorize this.
        });
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesConcatArrays_ExpectNotEqualResult()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 78, 32 };
            ReadOnlySpan<byte> other = stackalloc byte[] { 22, 13 };

            byte[] expected = new byte[] { 78, 32, 13 };

            Assert.NotEqual(expected, testData.ConcatArrays(other));
        });
    }

    #endregion

    #region LeftPaddedArray

    [Fact]
    public void ReadOnlySpanByte_InvokesLeftPaddedArray_ReturnsExpectedResult()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 11, 11, 78, 32 };

        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 78, 32 };
            Assert.Equal(expected, testData.LeftPaddedArray(padValue, padCount));
        });
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesLeftPaddedArray_ReturnsHasExpectedLength()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 11, 11, 78, 32 };

        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 78, 32 };
            byte[] actual = testData.LeftPaddedArray(padValue, padCount);
            Assert.Equal(expected.Length, testData.Length + padCount);
        });
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesLeftPaddedArray_ExpectNotEqualResult()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 11, 78, 32 };

        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 78, 32 };
            Assert.NotEqual(expected, testData.LeftPaddedArray(padValue, padCount));
        });
    }

    #endregion

    #region ReverseBits

    [Fact]
    public void ReadOnlySpanByte_InvokesReverseBits_ReturnsExpectedResult1()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1011, 0b1001 };
            byte[] expected = new byte[] { 0b11110100, 0b11110110 };

            Assert.Equal(expected, testData.ReverseBits());
        });
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesReverseBits_ReturnsExpectedResult2()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 0b10011011, 0b01101001 };
            byte[] expected = new byte[] { 0b01100100, 0b10010110 };

            Assert.Equal(expected, testData.ReverseBits());
        });
    }


    #endregion

    #region RightPaddedArray
    #endregion

    #region AsNibbleArray



    #endregion
}
