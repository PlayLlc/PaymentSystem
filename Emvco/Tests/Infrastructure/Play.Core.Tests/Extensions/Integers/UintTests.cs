using System;

using Play.Core.Extensions;

using Xunit;

namespace Play.Core.Tests.Extensions.Integers;

public class UintTests
{
    #region Instance Members

    [Fact]
    public void Uint_GetNumberOfDigits_ReturnsExpectedResult()
    {
        uint testData = 12345;
        byte a = testData.GetNumberOfDigits();
        Console.WriteLine("HI");
    }

    [Fact]
    public void Uint_GetMostSignificantBit_ReturnsExpectedResult()
    {
        uint testData = 12345;
        int a = testData.GetMostSignificantBit();
        Console.WriteLine("HI");
    }

    #endregion
}