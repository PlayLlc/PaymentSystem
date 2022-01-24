using System;

using Play.Core.Extensions;
using Play.Core.Tests.TestData.Fixtures;

using Xunit;

namespace Play.Core.Tests.Extensions.Arrays;

public class ByteArrayExtensionsTests
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Members

    [Fact]
    public void ByteArray_InvokesCopyValue_CorrectlyCopiesValue()
    {
        byte[] testValue = {0xFF, 0xFF, 0xF0, 0x01};
        byte[] expectedResult = {0xFF, 0xFF, 0xF0, 0x01};

        byte[] result = testValue.CopyValue();

        Assert.NotSame(testValue, result);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandom), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomByteArray_InvokesCopyValue_CorrectlyCopiesValue(byte[] testValue)
    {
        byte[] result = testValue.CopyValue();

        Assert.NotSame(testValue, result);
        Assert.Equal(testValue, result);
    }

    [Fact]
    public void ByteArray_InvokesCopyValue_CreatesValueCopyWithCorrectLength()
    {
        byte[] testValue = {0xFF, 0xFF, 0xF0, 0x01};
        int expectedLength = testValue.Length;

        byte[] result = testValue.CopyValue();

        Assert.Equal(expectedLength, result.Length);
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandom), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomByteArray_InvokesCopyValue_CreatesValueCopyWithCorrectLength(byte[] testValue)
    {
        byte[] result = testValue.CopyValue();
        int expectedLength = testValue.Length;

        Assert.Equal(expectedLength, result.Length);
    }

    [Fact]
    public void ByteArray_InvokesConcatArrays_CorrectlyConcatenatesArrays()
    {
        byte[] testValue1 = {0xFF, 0xFF, 0xF0, 0x01};
        byte[] testValue2 = {0xFF, 0xFF, 0xF0, 0x01};
        byte[] expectedResult = {0xFF, 0xFF, 0xF0, 0x01, 0xFF, 0xFF, 0xF0, 0x01};

        byte[] result = testValue1.ConcatArrays(testValue2);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandom), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomByteArray_InvokesConcatArrays_CorrectlyConcatenatesArrays(byte[] testValue)
    {
        byte[] result = testValue.ConcatArrays(testValue);

        for (int i = 0; i < testValue.Length; i++)
            Assert.Equal(testValue[i], result[i]);

        for (int i = 0, j = testValue.Length; i < testValue.Length; i++, j++)
            Assert.Equal(testValue[i], result[j]);
    }

    [Fact]
    public void ByteArray_InvokesConcatArrays_CreatesValueCopyWithCorrectLength()
    {
        byte[] testValue1 = {0xFF, 0xFF, 0xF0, 0x01};
        byte[] testValue2 = {0xFF, 0xFF, 0xF0, 0x05, 0x0D};
        int expectedResult = testValue1.Length + testValue2.Length;

        byte[] result = testValue1.ConcatArrays(testValue2);

        Assert.Equal(expectedResult, result.Length);
    }

    [Theory]
    [MemberData(nameof(ByteArrayFixture.GetRandom), 100, 0, 300, MemberType = typeof(ByteArrayFixture))]
    public void RandomByteArray_InvokesConcatArrays_CreatesValueCopyWithCorrectLength(byte[] testValue)
    {
        byte[] result = testValue.ConcatArrays(testValue);
        int expectedResult = testValue.Length * 2;

        Assert.Equal(expectedResult, result.Length);
    }

    #endregion
}