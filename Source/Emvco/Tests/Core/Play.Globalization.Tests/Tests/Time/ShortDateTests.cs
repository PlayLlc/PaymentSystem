using System;

using Play.Core;
using Play.Core.Exceptions;
using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time;
public class ShortDateTests : TestBase
{
    [Fact]
    public void ShortDate_InstantiateWithUshortInvalidLength_ExceptionIsThrow()
    {
        Assertion(() =>
        {
            Assert.Throws<PlayInternalException>(() =>
            {
                ushort input = 123;
                ShortDate testData = new ShortDate(input);
            });
        });
    }

    [Fact]
    public void ShortDate_InstantiateWithUshortValidLength_ShortDateIsInstantiated()
    {
        Assertion(() =>
        {
            ushort input = 1911;
            ShortDate testData = new ShortDate(input);

            //Assert.Equal(input, testData.AsYyMm());
        });
    }

    [Fact]
    public void ShortDate_InstantiateWithUshortValidLengthInvalidMonth_ArgumentOutOfRangeExceptionIsThrown()
    {
        Assertion(() =>
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() =>
            {
                ushort input = 2026;
                ShortDate testData = new ShortDate(input);
            });
        });
    }

    [Fact]
    public void ShortDate_InstantiateWithUIntInvalidLength_ExceptionIsThrow()
    {
        Assertion(() =>
        {
            Assert.Throws<PlayInternalException>(() =>
            {
                uint input = 12345;
                ShortDate testData = new ShortDate(input);
            });
        });
    }

    [Fact]
    public void ShortDate_InstantiateWithUIntValidLength_ShortDateIsInstantiated()
    {
        Assertion(() =>
        {
            uint input = 201106;
            ShortDate testData = new ShortDate(input);

            //Assert.Equal(input, testData.AsYyMmDd());
        });
    }

    [Fact]
    public void ShortDate_InstantiateWithUIntValidLengthInvalidMonth_ArgumentOutOfRangeExceptionIsThrown()
    {
        Assertion(() =>
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() =>
            {
                uint input = 2026;
                ShortDate testData = new ShortDate(input);
            });
        });
    }

    [Fact]
    public void ShortDate_InstantiateWithDateTimeUtc_ShortDateIsInstantiated()
    {
        DateTimeUtc input = new DateTimeUtc(new DateTime(2022, 06, 23, 0, 0, 0, DateTimeKind.Utc));
        ShortDate testData = new ShortDate(input);

        Assertion(() =>
        {
            Assert.Equal((ushort)2206, testData.AsYyMm());
            Assert.Equal((uint)220623, testData.AsYyMmDd());
        });
    }

    [Fact]
    public void ShortDate_AsNibbleArray_CorrectValueIsReturned()
    {
        DateTimeUtc input = new DateTimeUtc(new DateTime(2022, 06, 23, 0, 0, 0, DateTimeKind.Utc));
        ShortDate testData = new ShortDate(input);

        Nibble[] expected = new Nibble[] { 0b0010, 0b0010, 0b0, 0b0110, 0b0010, 0b0011 };
        Nibble[] actual = testData.AsNibbleArray();

        Assertion(() =>
        {
            Assert.Equal(expected, actual);
        },Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ShortDate_Equals_ReturnsTrue()
    {
        DateTime dateInput = new DateTime(2022, 06, 23, 0, 0, 0, DateTimeKind.Utc);
        DateTimeUtc input = new DateTimeUtc(dateInput);
        ShortDate testData = new ShortDate(input);
        ShortDate expected = new ShortDate(input);

        Assertion(() =>
        {
            Assert.True(testData.Equals(expected));
            Assert.True(testData.Equals(dateInput));
            Assert.True(ShortDate.Equals(expected, testData));
        });
    }

    [Fact]
    public void ShortDate_Operators()
    {
        DateTime dateInput = new DateTime(2022, 06, 17, 0, 0, 0, DateTimeKind.Utc);
        DateTimeUtc input = new DateTimeUtc(dateInput);

        DateTime dateInput2 = new DateTime(2022, 06, 23, 0, 0, 0, DateTimeKind.Utc);
        DateTimeUtc input2 = new DateTimeUtc(dateInput2);

        ShortDate testDate1 = new ShortDate(input);
        ShortDate testDate2 = new ShortDate(input2);

        Assertion(() =>
        {
            Assert.True(testDate1 < testDate2);
            Assert.True(testDate2 > testDate1);
            Assert.False(testDate1 == testDate2);
            Assert.True(testDate1 != testDate2);
            Assert.True(testDate1 <= testDate2);
            Assert.True(testDate2 >= testDate1);
        });
    }
}
