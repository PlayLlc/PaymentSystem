using Play.Core;
using Play.Core.Exceptions;
using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time;

public class ShortDateTests : TestBase
{
    #region Instance Members

    [Fact]
    public void ValidShortDate2011_EqualityWithDateTimeUtc_ReturnsTrue()
    {
        ShortDate expected = new(2011);
        DateTimeUtc testData = new(2020, 11, 1);
        ShortDate actual = new(testData);

        Assertion(() => Assert.Equal(expected, actual));
    }

    [Fact]
    public void ValidShortDate0001_EqualityWithDateTime_ReturnsTrue()
    {
        ShortDate sut = new(0001);
        DateTime dateTime = new(2000, 1, 1);
        Assertion(() => Assert.Equal(sut, dateTime));
    }

    [Fact]
    public void TwoDifferentShortDates_EqualityWithDateTime_ReturnsFalse()
    {
        ShortDate sut = new(0001);
        DateTime dateTime = new(2000, 3, 1);
        Assertion(() => Assert.NotEqual(sut, dateTime));
    }

    [Fact]
    public void ShortDate_InstantiateWithDateTimeUtc_ShortDateIsInstantiated()
    {
        DateTimeUtc input = new(new DateTime(2022, 06, 23, 0, 0, 0, DateTimeKind.Utc));
        ShortDate testData = new(input);

        Assertion(() => { Assert.Equal((ushort) 2206, testData.AsYyMm()); });
    }

    [Fact]
    public void ShortDate_AsNibbleArray_CorrectValueIsReturned()
    {
        DateTimeUtc input = new(new DateTime(2022, 06, 23, 0, 0, 0, DateTimeKind.Utc));
        ShortDate testData = new(input);

        Nibble[] expected = {0b0010, 0b0010, 0b0, 0b0110};
        Nibble[] actual = testData.AsNibbleArray();

        Assertion(() => { Assert.Equal(expected, actual); }, Build.Equals.Message(expected, actual));
    }

    [Fact]
    public void ShortDate_Equals_ReturnsTrue()
    {
        DateTime dateInput = new(2022, 06, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTimeUtc input = new(dateInput);
        ShortDate testData = new(input);
        ShortDate expected = new(input);

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
        DateTime dateInput = new(2022, 07, 17, 0, 0, 0, DateTimeKind.Utc);
        DateTimeUtc input = new(dateInput);

        DateTime dateInput2 = new(2022, 09, 23, 0, 0, 0, DateTimeKind.Utc);
        DateTimeUtc input2 = new(dateInput2);

        ShortDate testDate1 = new(input);
        ShortDate testDate2 = new(input2);

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

    #endregion

    #region Instantiation

    [Fact]
    public void ShortDate_InstantiateWithUshortValidLength_ShortDateIsInstantiated()
    {
        ShortDate sut = new(1911);

        Assertion(() =>
        {
            ushort input = 1911;
            ShortDate testData = new(input);
        });
    }

    [Fact]
    public void ShortDate_InstantiateWithUIntInvalidLength_ExceptionIsThrow()
    {
        Assertion(() =>
        {
            Assert.Throws<PlayInternalException>(() =>
            {
                ushort input = 12345;
                ShortDate testData = new(input);
            });
        });
    }

    [Fact]
    public void ShortDate_InstantiateWithUshortValidLengthInvalidMonth_ArgumentOutOfRangeExceptionIsThrown()
    {
        Assertion(() =>
        {
            Assert.Throws<PlayInternalException>(() =>
            {
                ushort input = 2026;
                ShortDate testData = new(input);
            });
        });
    }

    #endregion
}