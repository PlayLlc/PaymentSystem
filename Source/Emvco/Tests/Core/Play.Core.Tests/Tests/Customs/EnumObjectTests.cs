using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Extensions;
using Play.Core.Tests.Data.TestDoubles;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Core.Tests.Tests.Customs;

public class EnumObjectTests : TestBase
{
    #region Instance Members

    [Fact]
    public void ValidRawEnumValue_EqualityOnCorrectObject_ReturnsTrue()
    {
        byte testData = 1;

        Assertion(() => Assert.True(testData == TestEnumObject.First));
    }

    [Fact]
    public void ValidRawEnumValue_EqualityOnIncorrectObject_ReturnsFalse()
    {
        byte testData = 2;

        Assertion(() => Assert.False(testData == TestEnumObject.First));
    }

    [Fact]
    public void ValidRawEnumValue_InequalityOnIncorrectObject_ReturnsTrue()
    {
        byte testData = 2;

        Assertion(() => Assert.True(testData != TestEnumObject.First));
    }

    [Fact]
    public void ValidRawEnumValue_InequalityOnIncorrectObject_ReturnsFalse()
    {
        byte testData = 1;

        Assertion(() => Assert.False(testData != TestEnumObject.First));
    }

    #endregion

    #region GetAll

    [Fact]
    public void DefaultEnumObject_GetAll_ReturnsExpectedValue()
    {
        TestEnumObject testData = TestEnumObject.Empty;
        TestEnumObject[] expected = new[] {TestEnumObject.First, TestEnumObject.Second};

        TestEnumObject[] actual = testData.GetAll();

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void DefaultEnumObject_GetAll_IsNotNull()
    {
        TestEnumObject testData = TestEnumObject.Empty;
        TestEnumObject[] expected = new[] {TestEnumObject.First, TestEnumObject.Second};

        TestEnumObject[] actual = testData.GetAll();

        Assertion(() => Assert.NotNull(expected));
    }

    #endregion

    #region TryGet

    [Fact]
    public void ValidRawEnumValue_InstanceTryGet_ReturnsTrue()
    {
        byte testData = 1;

        bool expected = true;
        bool actual = TestEnumObject.Empty.TryGet(testData, out EnumObject<byte>? result);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidRawEnumValue_InstanceTryGet_ReturnsExpectedResult()
    {
        byte testData = 1;

        EnumObject<byte> expected = TestEnumObject.First;
        TestEnumObject.Empty.TryGet(testData, out EnumObject<byte>? actual);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidRawEnumValue_StaticTryGet_ReturnsTrue()
    {
        byte testData = 1;

        bool expected = true;
        bool actual = TestEnumObject.TryGet(testData, out TestEnumObject? result);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void InvalidRawEnumValue_StaticTryGet_ReturnsFalse()
    {
        byte testData = 3;

        bool actual = TestEnumObject.TryGet(testData, out TestEnumObject? result);

        Assertion(() => Assert.False(actual));
    }

    [Fact]
    public void ValidRawEnumValue_StaticTryGet_ReturnsExpectedValue()
    {
        byte testData = 1;
        TestEnumObject expected = TestEnumObject.First;

        TestEnumObject.TryGet(testData, out TestEnumObject? actual);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void InvalidRawEnumValue_StaticTryGet_ReturnsExpectedValue()
    {
        byte testData = 3;
        TestEnumObject? expected = null;

        TestEnumObject.TryGet(testData, out TestEnumObject? actual);

        Assertion(() => Assert.Equal(expected, actual));
    }

    #endregion
}