using System;

using Play.Core.Extensions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Spans;
public class SpanCharExtensionTests : TestBase
{
    #region CopyValue

    [Fact]
    public void ReadOnlySpanChar_InvokesCopyValue_CorrectlyCopiesValue()
    {
        Span<char> testData = stackalloc char[] { 'a', 'A', '1', '[' };

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
        Span<char> testData = stackalloc char[] { 'a', 'A', '1', '[' };

        char[] expected = testData.ToArray();
        char[] actual = testData.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(expected, actual);
            Assert.Equal(expected.Length, actual.Length);
        }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ReadOnlySpanChar_InvokesCopyValue_ExpectInvalidResponse()
    {
        Span<char> testData = stackalloc char[] { 'a', 'A', '1', '[' };

        char[] expected = new char[] { 'a', 'A' };
        char[] actual = testData.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(expected, actual);
            Assert.NotEqual(expected.Length, actual.Length);
        }, Build.Equals.Message(expected, actual));
    }

    #endregion
}
