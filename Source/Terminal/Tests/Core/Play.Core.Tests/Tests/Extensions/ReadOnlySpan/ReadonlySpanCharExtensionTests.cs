using System;

using Play.Core.Extensions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.ReadOnlySpan;

public class ReadonlySpanCharExtensionTests : TestBase
{
    #region IsValueEqual

    [Fact]
    public void ReadOnlySpanChar_IsValueEqual_ReturnsExpectedResult()
    {
        Assertion(() =>
        {
            ReadOnlySpan<char> expected = stackalloc char[] { 'a', 'A', '1', '[' };
            ReadOnlySpan<char> actual = expected;

            Assert.True(actual.IsValueEqual(expected));
        });
    }

    [Fact]
    public void ReadOnlySpanChar_IsValueEqual_ExpectNotEqualResult()
    {
        Assertion(() =>
        {
            ReadOnlySpan<char> expected = stackalloc char[] { 'a', 'A', '1', '[' };
            ReadOnlySpan<char> actual = stackalloc char[] { 'a', 'A', '[' };

            Assert.False(actual.IsValueEqual(expected));
        });
    }

    #endregion

    #region CopyValue

    [Fact]
    public void ReadOnlySpanChar_InvokesCopyValue_CorrectlyCopiesValue()
    {
        ReadOnlySpan<char> testData = stackalloc char[] { 'a', 'A', '1', '[' };

        char[] expected = testData.ToArray();
        char[] actual = testData.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(expected, actual);
            Assert.Equal(expected, actual);
        }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ReadOnlySpanChar_InvokesCopyValue_CreatesValueCopyWithCorrectLength()
    {
        ReadOnlySpan<char> testData = stackalloc char[] { 'a', 'A', '1', '[' };

        char[] expected = testData.ToArray();
        char[] actual = testData.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(expected, actual);
            Assert.Equal(expected.Length, actual.Length);
        }, Build.Equals.Message(expected, actual));
    }

    #endregion
}

