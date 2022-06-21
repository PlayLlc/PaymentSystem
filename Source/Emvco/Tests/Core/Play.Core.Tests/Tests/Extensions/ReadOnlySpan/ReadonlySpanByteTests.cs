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
    public void ReadOnlySpanByte_AsBigInteger_ReturnsExpectedResult()
    {
        Assertion(() =>
        {
            ReadOnlySpan<byte> testData = stackalloc byte[] { 1, 2, 3 };

            BigInteger testx = 197121;
            Span<byte> huh = stackalloc byte[6];
            testx.AsSpan(huh);

            BigInteger expected = 0b101001000;

            Assert.Equal(expected, testData.AsBigInteger());
        });
    }

    #endregion

    #region IsValueEqual
    #endregion

    #region ConcatArrays
    #endregion

    #region LeftPaddedArray
    #endregion

    #region ReverseBits
    #endregion

    #region RightPaddedArray
    #endregion

    #region AsNibbleArray



    #endregion
}
