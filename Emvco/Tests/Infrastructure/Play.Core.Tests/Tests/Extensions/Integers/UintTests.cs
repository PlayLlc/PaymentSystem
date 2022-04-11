using Play.Core.Extensions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Integers;

public class UintTests : TestBase
{
    #region Instance Members

    [Fact]
    public void Uint_GetNumberOfDigits_ReturnsExpectedResult()
    {
        uint testData = 12345;
        int expected = 5;
        byte actual = testData.GetNumberOfDigits();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void Uint_GetMostSignificantBit_ReturnsExpectedResult()
    {
        uint testData = 12345;
        int expected = 14;
        int actual = testData.GetMostSignificantBit();
        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}