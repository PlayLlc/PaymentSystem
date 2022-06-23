using System;

using Play.Core.Extensions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Spans;
public class SpanByteExtensionTests : TestBase
{
    #region ConcatArrays

    [Fact]
    public void SpanByte_InvokesConcatArrays_CorectResultIsReturned()
    {
        Span<byte> testData = stackalloc byte[] { 1, 2, 3 };
        ReadOnlySpan<byte> other = stackalloc byte[] { 1, 1 };

        byte[] expected = new byte[] { 1, 2, 3, 1, 1 };
        byte[] actual = testData.ConcatArrays(other);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void SpanByte_InvokesConcatArrays_ResultHasCorrectLength()
    {
        Span<byte> testData = stackalloc byte[] { 1, 2, 3 };
        ReadOnlySpan<byte> other = stackalloc byte[] { 1, 1 };

        int expectedLength = testData.Length + other.Length;
        byte[] actual = testData.ConcatArrays(other);

        Assertion(() => Assert.Equal(expectedLength, actual.Length));
    }

    [Fact]
    public void SpanByte_InvokesConcatArrays_ExpectIncorectResult()
    {
        Span<byte> testData = stackalloc byte[] { 1, 2, 3 };
        ReadOnlySpan<byte> other = stackalloc byte[] { 1, 1 };

        byte[] expectedWrong = new byte[] { 1, 2, 1 };
        byte[] actual = testData.ConcatArrays(other);

        Assertion(() => Assert.NotEqual(expectedWrong, actual));
    }

    #endregion

    #region RemoveLeftPadding

    [Fact]
    public void SpanByte_RemoveLeftPaddingPaddingValueNotFound_ExpectNothingToChange()
    {
        Span<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110 };
        Nibble paddingValue = 0b0101;

        byte[] expected = new byte[] { 0b1010_0101, 0b1111_0110 };
        byte[] actual = testData.RemoveLeftPadding(paddingValue);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void SpanByte_RemoveLeftPadding1_ReturnsExpectedResult()
    {
        Span<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110 };
        Nibble paddingValue = 0b1010;

        byte[] expected = new byte[] { 0b0101, 0b1111_0110 };
        byte[] actual = testData.RemoveLeftPadding(paddingValue);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void SpanByte_RemoveLeftPadding_Returns2()
    {
        Span<byte> testData = stackalloc byte[] { 0b1010_1010, 0b1111_0110 };
        Nibble paddingValue = 0b1010;

        byte[] expected = new byte[] { 0b1111_0110 };
        byte[] actual = testData.RemoveLeftPadding(paddingValue);

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion

    #region RightPaddedArray

    [Fact]
    public void SpanByte_InvokesRightPaddedArray_ReturnsExpectedResult()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 78, 32, 11, 11 };

        Assertion(() =>
        {
            Span<byte> testData = stackalloc byte[] { 78, 32 };
            Assert.Equal(expected, testData.RightPaddedArray(padValue, padCount));
        });
    }

    [Fact]
    public void SpanByte_InvokesRightPaddedArray_ReturnsHasExpectedLength()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 78, 32, 11, 11 };

        Assertion(() =>
        {
            Span<byte> testData = stackalloc byte[] { 78, 32 };
            byte[] actual = testData.RightPaddedArray(padValue, padCount);
            Assert.Equal(expected.Length, testData.Length + padCount);
        });
    }

    [Fact]
    public void SpanByte_InvokesRightPaddedArray_ExpectNotEqualResult()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 78, 32, 11 };

        Assertion(() =>
        {
            Span<byte> testData = stackalloc byte[] { 78, 32 };
            Assert.NotEqual(expected, testData.RightPaddedArray(padValue, padCount));
        });
    }

    #endregion

    #region LeftPaddedArray

    [Fact]
    public void SpanByte_InvokesLeftPaddedArray_ReturnsExpectedResult()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 11, 11, 78, 32 };

        Assertion(() =>
        {
            Span<byte> testData = stackalloc byte[] { 78, 32 };
            Assert.Equal(expected, testData.LeftPaddedArray(padValue, padCount));
        });
    }

    [Fact]
    public void SpanByte_InvokesLeftPaddedArray_ReturnsHasExpectedLength()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 11, 11, 78, 32 };

        Assertion(() =>
        {
            Span<byte> testData = stackalloc byte[] { 78, 32 };
            byte[] actual = testData.LeftPaddedArray(padValue, padCount);
            Assert.Equal(expected.Length, testData.Length + padCount);
        });
    }

    [Fact]
    public void SpanByte_InvokesLeftPaddedArray_ExpectNotEqualResult()
    {
        byte padValue = 11;
        int padCount = 2;
        byte[] expected = { 11, 78, 32 };

        Assertion(() =>
        {
            Span<byte> testData = stackalloc byte[] { 78, 32 };
            Assert.NotEqual(expected, testData.LeftPaddedArray(padValue, padCount));
        });
    }

    #endregion

    #region ShiftRightOneNibble

    [Fact]
    public void SpanByte_InvokesShiftRightOneNibble_CorrectIsReturned1()
    {
        Span<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110 };

        byte[] expected = new byte[] { 0b1010, 0b0101_1111 };
        byte[] actual = testData.ShiftRightOneNibble();

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void SpanByte_InvokesShiftRightOneNibble_CorrectResultIsReturned2()
    {
        Span<byte> testData = stackalloc byte[] { 0b1010, 0b1111, 0b0101_1001, 0b1100_1100 };

        byte[] expected = new byte[] { 0b0, 0b1010_0000, 0b1111_0101, 0b1001_1100 };
        byte[] actual = testData.ShiftRightOneNibble();

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void SpanByte_InvokesShiftRightOneNibble_ExpectWrongResult()
    {
        Span<byte> testData = stackalloc byte[] { 0b1010, 0b1111, 0b0101_1001, 0b1100_1100 };

        byte[] expected = new byte[] { 0b1010_0000, 0b1111_0101, 0b1001_1100 };
        byte[] actual = testData.ShiftRightOneNibble();

        Assertion(() => Assert.NotEqual(expected, actual));
    }

    #endregion

    #region AsNibbleArray

    [Fact]
    public void SpanByte_InvokesAsNibbleArrayHasEventLength_CorrectNibbleArrayIsReturned()
    {
        Assertion(() =>
        {
            Span<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110 };
            Nibble[] expected = new Nibble[] { 0b1010, 0b0101, 0b1111, 0b0110 };

            Assert.Equal(expected, testData.AsNibbleArray());
        });
    }

    [Fact]
    public void SpanByte_InvokesAsNibbleArrayHasOddLength_CorrectNibbleArrayIsReturned()
    {
        Span<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111_0110, 0b1111 };
        Nibble[] expected = new Nibble[] { 0b1010, 0b0101, 0b1111, 0b0110, 0b0, 0b1111 };
        Nibble[] actual = testData.AsNibbleArray();

        Assertion(() =>
        {
            Assert.Equal(expected, actual);
            Assert.Equal(expected.Length, actual.Length);
        }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void SpanByte_InvokesAsNibbleArray_ResultedNibbleHasDoubleLengthOfInput()
    {
        Span<byte> testData = stackalloc byte[] { 0b1010_0101, 0b1111 };
        Nibble[] nibble = testData.AsNibbleArray();
        int expected = testData.Length * 2;
        int actual = nibble.Length;

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion

    #region SetBits

    [Fact]
    public void Byte_SetBitsForGivenSpan_Returns0b101()
    {
        Assertion(() =>
        {
            Span<byte> testData = stackalloc byte[] { 0 };
            ReadOnlySpan<byte> bitsToSet = stackalloc byte[] { 0b101 };

            testData.SetBits(bitsToSet);

            byte expected = 0b101;

            Assert.Equal(expected, testData[0]);
        });
    }

    [Fact]
    public void Byte_SetBitsForGivenSpan_ReturnsExpectedResult1()
    {
        Span<byte> testData = stackalloc byte[] { 0b10101010, 0 };
        ReadOnlySpan<byte> bytesToSet = stackalloc byte[] { 0b101, 0b101 };

        byte[] expected = new byte[] { 0b10101111, 0b101 } ;
        testData.SetBits(bytesToSet);
        byte[] actual = testData.ToArray();

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion
}
