using System;
using System.Numerics;

using Play.Core.Extensions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.ReadOnlySpan;

public class ReadonlySpanByteExtensionTests : TestBase
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

    [Fact]
    public void ReadOnlySpanByte_InvokesRightPaddedArray_ReturnsExpectedResult()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 78, 32, 11, 11 };

        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 78, 32 };
            Assert.Equal(expected, testData.RightPaddedArray(padValue, padCount));
        });
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesRightPaddedArray_ReturnsHasExpectedLength()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 78, 32, 11, 11 };

        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 78, 32 };
            byte[] actual = testData.RightPaddedArray(padValue, padCount);
            Assert.Equal(expected.Length, testData.Length + padCount);
        });
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesRightPaddedArray_ExpectNotEqualResult()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 78, 32, 11 };

        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 78, 32 };
            Assert.NotEqual(expected, testData.RightPaddedArray(padValue, padCount));
        });
    }

    #endregion

    #region AsNibbleArray

    [Fact]
    public void ReadOnlySpanByte_InvokesAsNibbleArrayHasEventLength_CorrectNibbleArrayIsReturned()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110 };
            Nibble[] expected = new Nibble[] { 0b1010, 0b0101, 0b1111, 0b0110 };

            Assert.Equal(expected, testData.AsNibbleArray());
        });
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesAsNibbleArrayHasOddLength_CorrectNibbleArrayIsReturned()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110, 0b1111 };
        Nibble[] expected = new Nibble[] { 0b1010, 0b0101, 0b1111, 0b0110, 0b0, 0b1111 };
        Nibble[] actual = testData.AsNibbleArray();

        Assertion(() =>
        {
            Assert.Equal(expected, actual);
            Assert.Equal(expected.Length, actual.Length);
        }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesAsNibbleArray_ResultedNibbleHasDoubleLengthOfInput()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111 };
        Nibble[] nibble = testData.AsNibbleArray();
        int expected = testData.Length * 2;
        int actual = nibble.Length;

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion

    #region ShiftRightOneNibble

    [Fact]
    public void ReadOnlySpanByte_InvokesShiftRightOneNibble_CorrectIsReturned1()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110 };

        byte[] expected = new byte[] { 0b1010, 0b0101_1111 };
        byte[] actual = testData.ShiftRightOneNibble();

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesShiftRightOneNibble_CorrectResultIsReturned2()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1010, 0b1111, 0b0101_1001, 0b1100_1100 };

        byte[] expected = new byte[] { 0b0, 0b1010_0000, 0b1111_0101, 0b1001_1100  };
        byte[] actual = testData.ShiftRightOneNibble();

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ReadOnlySpanByte_InvokesShiftRightOneNibble_ExpectWrongResult()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1010, 0b1111, 0b0101_1001, 0b1100_1100 };

        byte[] expected = new byte[] { 0b1010_0000, 0b1111_0101, 0b1001_1100 };
        byte[] actual = testData.ShiftRightOneNibble();

        Assertion(() => Assert.NotEqual(expected, actual));
    }

    #endregion

    #region RemoveLeftPadding

    [Fact]
    public void ReadOnlySpanByte_RemoveLeftPaddingPaddingValueNotFound_ExpectNothingToChange()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110 };
        Nibble paddingValue = 0b0101;

        byte[] expected = new byte[] { 0b1010_0101, 0b1111_0110 };
        byte[] actual = testData.RemoveLeftPadding(paddingValue);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ReadOnlySpanByte_RemoveLeftPadding1_ReturnsExpectedResult()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110 };
        Nibble paddingValue = 0b1010;

        byte[] expected = new byte[] { 0b0101, 0b1111_0110 };
        byte[] actual = testData.RemoveLeftPadding(paddingValue);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ReadOnlySpanByte_RemoveLeftPadding_Returns2()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1010_1010, 0b1111_0110 };
        Nibble paddingValue = 0b1010;

        byte[] expected = new byte[] { 0b1111_0110 };
        byte[] actual = testData.RemoveLeftPadding(paddingValue);

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion

    #region GetLeftPaddedZeroCount

    [Fact]
    public void ReadOnlySpanByte_GetLeftPaddedZeroCount_Returns1()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b0000_1010, 0b0011, 0b1 };

        int expected = 1;
        int actual = testData.GetLeftPaddedZeroCount();
        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ReadOnlySpanByte_GetLeftPaddedZeroCount_Returns0()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b1100_1010, 0b0011, 0b1 };

        int expected = 0;
        int actual = testData.GetLeftPaddedZeroCount();
        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ReadOnlySpanByte_GetLeftPaddedZeroCount_Returns3()
    {
        ReadOnlySpan<byte> testData = stackalloc byte[] { 0b0, 0b0011, 0b1 };

        int expected = 3;
        int actual = testData.GetLeftPaddedZeroCount();
        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion
}
