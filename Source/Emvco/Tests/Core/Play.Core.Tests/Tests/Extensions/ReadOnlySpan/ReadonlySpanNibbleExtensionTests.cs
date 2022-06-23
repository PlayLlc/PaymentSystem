using System;

using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.ReadOnlySpan;

public class ReadonlySpanNibbleExtensionTests : TestBase
{
    #region AsByteArray

    [Fact]
    public void ReadOnlySpanNibble_InvokeAsByteArrayOddLength_ReturnsCorrectResultLastNibbleIs0()
    {
        Assertion(() =>
        {
            ReadOnlySpan<Nibble> testData = stackalloc Nibble[] { 0b1010, 0b1100, 0b0101 };
            byte[] expected = new byte[] { 0b10101100, 0b01010000 };

            Assert.Equal(expected, testData.AsByteArray());
        });
    }

    [Fact]
    public void ReadOnlySpanNibble_InvokeAsByteArrayEvenLength_ReturnsCorrectResult()
    {
        Assertion(() =>
        {
            ReadOnlySpan<Nibble> testData = stackalloc Nibble[] { 0b1010, 0b1100, 0b0101, 0b1001 };
            byte[] expected = new byte[] { 0b10101100, 0b01011001 };

            Assert.Equal(expected, testData.AsByteArray());
            Assert.Equal(expected.Length, testData.Length / 2);
        });
    }

    #endregion

    #region CopyTo

    [Fact]
    public void ReadOnlySpanNibble_InvokeCopyTo_ValueIsCopiedCorrectly()
    {
        Assertion(() =>
        {
            ReadOnlySpan<Nibble> testData = stackalloc Nibble[] { 0b1010, 0b1100, 0b0101, 0b1001 };
            Span<byte> buffer = stackalloc byte[testData.Length / 2];
            byte[] expected = new byte[] { 0b10101100, 0b01011001 };
            testData.CopyTo(buffer);

            Assert.Equal(expected, buffer.ToArray());
        });
    }

    [Fact]
    public void ReadOnlySpanNibble_InvokeCopyToBufferLengthTooSmall_ExceptionIsThrown()
    {
        Assertion(() =>
        {
            Assert.Throws<PlayInternalException>(() =>
            {
                ReadOnlySpan<Nibble> testData = stackalloc Nibble[] { 0b1010, 0b1100, 0b0101, 0b1001 };
                Span<byte> buffer = stackalloc byte[testData.Length / 2 - 1];

                testData.CopyTo(buffer);
            });
        });
    }

    #endregion
}

