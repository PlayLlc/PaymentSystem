using System;

using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Arrays;

public class NibbleArrayExtensionsTests : TestBase
{
    #region NibbbleInitialization

    [Fact]
    public void Nibble_Initialization_ArgutmentOutOfRangeExceptionIsThrownWhenInitializingNibble()
    {
        Assertion(() => Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Nibble testData = new Nibble(0xFF);
        }));
    }

    [Fact]
    public void NibbleArray_Initialization_ArgutmentOutOfRangeExceptionIsThrownWhenInitializingNibble()
    {
        Assertion(() => Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Nibble[] testData = new Nibble[] { 0b01, 0b10, 0b11001100, 0xF0 };
        }));
    }

    #endregion

    #region AsByteArray

    [Fact]
    public void NibbleArray_InvokesAsByteArrayHasEvenLength_CorrectResultIsReturned()
    {
        Nibble[] testData = new Nibble[] { 0b01, 0b11, 0b1100, 0b1001 };

        byte[] expected = new byte[] { 0b010011, 0b11001001 };

        Assertion(() => Assert.Equal(expected, testData.AsByteArray()));
    }

    [Fact]
    public void NibbleArray_InvokesAsByteArrayHasOddLengthLastByteHas0AsLeftNibble_CorrectResultIsReturned()
    {
        Nibble[] testData = new Nibble[] { 0b01, 0b10, 0b1100 };

        byte[] expected = new byte[] { 0b10010, 0b11000000 };

        Assertion(() => Assert.Equal(expected, testData.AsByteArray()));
    }

    #endregion


    #region CopyValue

    [Fact]
    public void NibbleArray_InvokesCopyValue_CorrectlyCopiesValue()
    {
        Nibble[] expected = new Nibble[] { 0b01, 0b10, 0b110 };
        Nibble[] actual = expected.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(expected, actual);
            Assert.Equal(expected, actual);
        }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void NibbleArray_InvokesCopyValue_CopiesValueWithCorrectLength()
    {
        Nibble[] expected = new Nibble[]
        {
            0b1010,
            0b1111,
            0b01,
            0b1110
        };
        Nibble[] actual = expected.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(expected, actual);
            Assert.Equal(expected.Length, actual.Length);
        }, Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandomNibble), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomNibbleArray_InvokesCopyValue_CorrectlyCopiesValue(Nibble[] testValue)
    {
        Nibble[] actual = testValue.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(testValue, actual);
            Assert.Equal(testValue, actual);
        }, Build.Equals.Message(testValue, actual));
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandomNibble), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomNibbleArray_InvokesCopyValue_CopiesValueWithCorrectLength(Nibble[] testValue)
    {
        Nibble[] actual = testValue.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(testValue, actual);
            Assert.Equal(testValue.Length, actual.Length);
        }, Build.Equals.Message(testValue, actual));
    }

    #endregion

    #region CopyTo

    [Fact]
    public void NibbleArray_InvokesCopyToBufferLengthSmallerThenNibble_PlayInternalExceptionIsThrown()
    {
        Nibble[] expected = { 0b10, 0b01, 0b11, 0b110, 0b10, 0b01, 0b11, 0b1011 };

        Assertion(() => Assert.Throws<PlayInternalException>(() =>
        {
            Span<byte> buffer = stackalloc byte[3];

            expected.CopyTo(buffer);
        }));
    }

    [Fact]
    public void NibbleArray_InvokesCopyToBufferHasEvenLength_ValueIsCopiedToBuffer()
    {
        Nibble[] testData = { 0b10, 0b0101, 0b1111, 0b1111 };

        Assertion(() =>
        {
            Span<byte> expected = stackalloc byte[] { 0b100101, 0b11111111 };
            Span<byte> actual = stackalloc byte[(testData.Length / 2) + (testData.Length % 2)];
            testData.CopyTo(actual);

            Assert.Equal(expected.ToArray(), actual.ToArray());
        });
    }

    [Fact]
    public void NibbleArray_InvokesCopyToBufferHasOddLength_ValueIsCopiedToBufferLastValueHas0AsRightNibble()
    {
        Nibble[] testData = { 0b10, 0b1101, 0b11, 0b1100, 0b1001 };

        Assertion(() =>
        {
            Span<byte> expected = stackalloc byte[] { 0b101101, 0b111100, 0b10010000 };
            Span<byte> actual = stackalloc byte[(testData.Length / 2) + (testData.Length % 2)];
            testData.CopyTo(actual);

            Assert.Equal(expected.ToArray(), actual.ToArray());
        });
    }

    #endregion
}