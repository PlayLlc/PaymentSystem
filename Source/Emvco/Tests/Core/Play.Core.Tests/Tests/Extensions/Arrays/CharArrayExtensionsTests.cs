using Play.Core.Extensions;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Arrays;

public class CharArrayExtensionsTests : TestBase
{
    #region CopyValue

    [Fact]
    public void CharArray_InvokesCopyValue_CorrectlyCopiesValue()
    {
        char[] expected = {'a', 'b', 'c'};
        char[] actual = expected.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(expected, actual);
            Assert.Equal(expected, actual);
        }, Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(CharArrayFixture.GetRandom), 50, MemberType = typeof(CharArrayFixture))]
    public void RandomCharArray_InvokesCopyValue_CorrectlyCopiesValue(char[] testData)
    {
        char[] actual = testData.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(testData, actual);
            Assert.Equal(testData, actual);
        }, Build.Equals.Message(testData, actual));
    }

    [Fact]
    public void CharArray_InvokesCopyValue_CreatesValueCopyWithCorrectLength()
    {
        char[] expected = {'a', 'A', '0', '9', '1', 'z', 'Z'};
        char[] actual = expected.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(expected, actual);
            Assert.Equal(expected.Length, actual.Length);
        }, Build.Equals.Message(expected, actual));
    }

    #endregion
}