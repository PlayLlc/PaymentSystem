using System;

using Play.Core.Extensions;
using Play.Core.Tests.TestData.Fixtures;
using Play.Testing.Infrastructure.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Extensions.Arrays;

public class ByteArrayExtensionsTests : TestBase
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Members

    [Fact]
    public void ByteArray_InvokesCopyValue_CorrectlyCopiesValue()
    {
        byte[] expected = {0xFF, 0xFF, 0xF0, 0x01};
        byte[] actual = expected.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(expected, actual);
            Assert.Equal(expected, actual);
        }, Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandom), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomByteArray_InvokesCopyValue_CorrectlyCopiesValue(byte[] testValue)
    {
        byte[] actual = testValue.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(testValue, actual);
            Assert.Equal(testValue, actual);
        }, Build.Equals.Message(testValue, actual));
    }

    [Fact]
    public void ByteArray_InvokesCopyValue_CreatesValueCopyWithCorrectLength()
    {
        byte[] testValue = {0xFF, 0xFF, 0xF0, 0x01};

        byte[] actual = testValue.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(testValue, actual);
            Assert.Equal(testValue.Length, actual.Length);
        }, Build.Equals.Message(testValue, actual));
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandom), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomByteArray_InvokesCopyValue_CreatesValueCopyWithCorrectLength(byte[] testValue)
    {
        byte[] actual = testValue.CopyValue();

        Assertion(() =>
        {
            Assert.NotSame(testValue, actual);
            Assert.Equal(testValue.Length, actual.Length);
        }, Build.Equals.Message(testValue, actual));
    }

    [Fact]
    public void ByteArray_InvokesConcatArrays_CorrectlyConcatenatesArrays()
    {
        byte[] testValue1 = {0xFF, 0xFF, 0xF0, 0x01};
        byte[] testValue2 = {0xFF, 0xFF, 0xF0, 0x01};
        byte[] expected = {0xFF, 0xFF, 0xF0, 0x01, 0xFF, 0xFF, 0xF0, 0x01};

        byte[] actual = testValue1.ConcatArrays(testValue2);

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandom), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomByteArray_InvokesConcatArrays_CorrectlyConcatenatesArrays(byte[] testValue)
    {
        byte[] actual = testValue.ConcatArrays(testValue);

        for (int i = 0; i < testValue.Length; i++)
            Assertion(() => Assert.Equal(testValue[i], actual[i]), Build.Equals.Message(testValue[i], actual[i]));

        for (int i = 0, j = testValue.Length; i < testValue.Length; i++, j++)
            Assertion(() => Assert.Equal(testValue[i], actual[i]), Build.Equals.Message(testValue[i], actual[i]));
    }

    [Fact]
    public void ByteArray_InvokesConcatArrays_CreatesValueCopyWithCorrectLength()
    {
        byte[] testValue1 = {0xFF, 0xFF, 0xF0, 0x01};
        byte[] testValue2 = {0xFF, 0xFF, 0xF0, 0x05, 0x0D};
        int expected = testValue1.Length + testValue2.Length;

        int actual = testValue1.ConcatArrays(testValue2).Length;

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandom), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomByteArray_InvokesConcatArrays_CreatesValueCopyWithCorrectLength(byte[] testValue)
    {
        int actual = testValue.ConcatArrays(testValue).Length;
        int expected = testValue.Length * 2;

        Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
    }

    #endregion
}