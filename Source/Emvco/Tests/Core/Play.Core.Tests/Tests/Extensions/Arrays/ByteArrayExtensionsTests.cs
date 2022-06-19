using System;

using Play.Core.Extensions;
using Play.Core.Tests.Data.Fixtures;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Extensions.Arrays;

public class ByteArrayExtensionsTests : TestBase
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region AsNibbleArray

    [Fact]
    public void MaxByteArray_InvokesAsNibbleArray_ReturnsExpectedResult()
    {
        byte[] testData = new byte[] {0xFF, 0xFF, 0xFF};
        Nibble[] expected = new Nibble[] {0xF, 0xF, 0xF, 0xF, 0xF, 0xF};
        Nibble[] actual = testData.AsNibbleArray();
        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ByteArray_InvokesAsNibbleArray_ReturnsExpectedResult()
    {
        byte[] testData = new byte[] {0x01, 0x10, 0xF1};
        Nibble[] expected = new Nibble[] {0x0, 0x1, 0x1, 0x0, 0xF, 0x1};
        Nibble[] actual = testData.AsNibbleArray();
        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void MinByteArray_InvokesAsNibbleArray_ReturnsExpectedResult()
    {
        byte[] testData = new byte[] {0x00, 0x00, 0x00};
        Nibble[] expected = new Nibble[] {0x0, 0x0, 0x0, 0x0, 0x0, 0x0};
        Nibble[] actual = testData.AsNibbleArray();
        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion

    #region RemoveLeftPadding

    [Fact]
    public void PaddedByteArray_InvokesRemovePadding_ReturnsExpectedValue()
    {
        byte[] testValue = new byte[] {0xFF, 0xFF, 0xF1, 0x23, 0x44};
        byte[] expected = new byte[] {0x01, 0x23, 0x44};

        byte[] actual = testValue.RemoveLeftPadding(0xF);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ByteArrayWithoutPadding_InvokesRemovePadding_ReturnsExpectedValue()
    {
        byte[] testValue = new byte[] {0xFF, 0xFF, 0xF1, 0x23, 0x44};
        byte[] expected = new byte[] {0xFF, 0xFF, 0xF1, 0x23, 0x44};

        byte[] actual = testValue.RemoveLeftPadding(0);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void Padding_InvokesRemovePadding_ReturnsExpectedValue()
    {
        byte[] testValue = new byte[] {0xFF, 0xFF};
        byte[] expected = new byte[] { };

        byte[] actual = testValue.RemoveLeftPadding(0xF);

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion

    #region CopyValue

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

    #endregion

    #region ConcatArrays

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