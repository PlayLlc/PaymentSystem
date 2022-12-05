using Play.Core.Exceptions;
using Play.Globalization.Extensions;
using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time.DateTimes;

public class DateTimeUtcTests : TestBase
{
    [Fact]
    public void DateTimeUtcExtension()
    {
        // Implement the DateTimeUtc.Now.GetBusinessDays() extension method tests
        throw new NotImplementedException();

        Assert.True(1 == 2);
    }

    [Fact]
    public void GetNextWeek()
    {
        // Implement the DateTimeUtc.Now.GetBusinessDays() extension method tests
        throw new NotImplementedException();

        Assert.True(1 == 2);
    }

    [Fact]
    public void GetLastWeek()
    {
        // Implement the DateTimeUtc.Now.GetBusinessDays() extension method tests
        throw new NotImplementedException();

        Assert.True(1 == 2);
    }

    [Fact]
    public void GetNextMonth()
    {
        // Implement the DateTimeUtc.Now.GetBusinessDays() extension method tests
        throw new NotImplementedException();

        Assert.True(1 == 2);
    }

    [Fact]
    public void GetLastMonth()
    {
        // Implement the DateTimeUtc.Now.GetBusinessDays() extension method tests
        throw new NotImplementedException();

        Assert.True(1 == 2);
    }

    [Fact]
    public void DateTimeUtc_InitializeWithCorrectDateTimeKind_DateTimeUtcInitialized()
    {
        DateTime input = new(2019, 06, 11, 0, 0, 0, DateTimeKind.Local);

        Assertion(() =>
        {
            Assert.Throws<PlayInternalException>(() =>
            {
                DateTimeUtc test = new(input);
            });
        });
    }

    [Fact]
    public void DateTimeUtc_InitializeWithCorrectDateTimeKind_DateTimeUtcInitiliazed()
    {
        DateTime input = new(2019, 06, 11, 0, 0, 0, DateTimeKind.Utc);

        Assertion(() =>
        {
            DateTimeUtc test = new(input);
            Assert.NotNull(test);
            Assert.Equal(2019, test.Year);
            Assert.Equal(06, test.Month);
            Assert.Equal(11, test.Day);
        });
    }

    [Fact]
    public void DateTimeUtc_InstantiateFromLongValue_DateTimeUtcInstantiated()
    {
        long input = 20190615;
        DateTimeUtc testData = new(input);

        Assertion(() => Assert.Equal(01, testData.Year));
    }

    [Fact]
    public void DateTimeUtc_InstiateFromIntValue_DateTimeUtcInstantiated()
    {
        int input = 637915392;
        DateTimeUtc testData = new(input);

        Assertion(() => Assert.Equal(01, testData.Year));
    }

    [Fact]
    public void DateTimeUtc_InstantiateFromDate_InstantiatedCorrectly()
    {
        DateTime input = new(2020, 07, 11, 11, 36, 10, DateTimeKind.Utc);
        DateTimeUtc testData = new(input);

        Assertion(() =>
        {
            Assert.Equal(2020, testData.Year);
            Assert.Equal(07, testData.Month);
            Assert.Equal(11, testData.Day);
            Assert.Equal(11, testData.Hour);
            Assert.Equal(36, testData.Minute);
            Assert.Equal(10, testData.Second);
        });
    }

    [Fact]
    public void DateTimeUtc_StaticPropertyNow_CorrectValue()
    {
        DateTimeUtc utcNow = DateTimeUtc.Now;

        Assertion(() =>
        {
            Assert.Equal(DateTime.UtcNow.Year, utcNow.Year);
            Assert.Equal(DateTime.UtcNow.Month, utcNow.Month);
            Assert.Equal(DateTime.UtcNow.Day, utcNow.Day);
            Assert.Equal(0, utcNow.Hour);
            Assert.Equal(0, utcNow.Minute);
            Assert.Equal(0, utcNow.Second);
        });
    }

    [Fact]
    public void DateTimeUtc_StaticPropertyToday_CorrectValue()
    {
        DateTimeUtc today = DateTimeUtc.Today;

        Assertion(() =>
        {
            Assert.Equal(DateTime.UtcNow.Year, today.Year);
            Assert.Equal(DateTime.UtcNow.Month, today.Month);
            Assert.Equal(DateTime.UtcNow.Day, today.Day);
        });
    }

    [Fact]
    public void DateTimeUtc_EqualsDateTime_AreEqual()
    {
        DateTime input = new(2020, 07, 11, 11, 36, 10, DateTimeKind.Utc);
        DateTimeUtc testData = new(input);
        DateTimeUtc compareTo = new(input);

        Assertion(() => Assert.True(testData.Equals(input)));
    }

    [Fact]
    public void DateTimeUtc_CompareToDateTime_AreEqual()
    {
        DateTime input = new(2020, 07, 11, 11, 36, 10, DateTimeKind.Utc);
        DateTimeUtc testData = new(input);
        DateTimeUtc compareTo = new(input);

        Assertion(() => Assert.Equal(0, testData.CompareTo(compareTo)));
    }

    [Fact]
    public void DateTimeUtc_CompareToDateTime_IsLesserThan()
    {
        DateTime input = new(2020, 07, 11, 11, 36, 10, DateTimeKind.Utc);
        DateTime compareToInput = new(2019, 07, 11, 11, 36, 10, DateTimeKind.Utc);
        DateTimeUtc testData = new(input);
        DateTimeUtc compareTo = new(compareToInput);

        Assertion(() => Assert.True(compareTo.CompareTo(testData) < 0));
    }

    [Fact]
    public void DateTimeUtc_CompareToDateTime_IsGreaterThan()
    {
        DateTime input = new(2020, 07, 11, 11, 36, 10, DateTimeKind.Utc);
        DateTime compareToInput = new(2019, 07, 11, 11, 36, 10, DateTimeKind.Utc);
        DateTimeUtc testData = new(input);
        DateTimeUtc compareTo = new(compareToInput);

        Assertion(() => Assert.True(testData.CompareTo(compareTo) > 0));
    }
}