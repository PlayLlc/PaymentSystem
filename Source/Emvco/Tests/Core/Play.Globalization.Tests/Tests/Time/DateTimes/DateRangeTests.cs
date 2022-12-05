using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Globalization.Tests.Tests.Time.DateTimes;

public class DateRangeTests : TestBase
{
    #region Instantiation

    [Fact]
    public void DateRange_Instantiate_ValidationErrorExceptionIsThrown()
    {
        ShortDate activationDate = new(DateTimeUtc.Now);
        ShortDate expirationDate = new(2006);

        Assertion(() =>
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                DateRange testData = new(activationDate, expirationDate);
            });
        });
    }

    [Fact]
    public void DateRange_Instantiate_CorrectInstantiation()
    {
        ShortDate activationDate = new(2006);
        ShortDate expirationDate = new(DateTimeUtc.Now);

        DateRange testData = new(activationDate, expirationDate);

        Assertion(() =>
        {
            Assert.NotNull(testData);
            Assert.Equal(expirationDate, testData.GetExpirationDate().AsShortDate());
        });
    }

    [Fact]
    public void DateRange_Initialize_CorrectActivationDateAndExpirationDate()
    {
        ShortDate activationDate = new(2206);
        ShortDate expirationDate = new(2407);

        DateRange dateRange = new(activationDate, expirationDate);

        Assertion(() =>
        {
            Assert.Equal(activationDate, dateRange.GetActivationDate().AsShortDate());
            Assert.Equal(expirationDate, dateRange.GetExpirationDate().AsShortDate());
        });
    }

    [Fact]
    public void DateRange_Initialize_IsActive()
    {
        ShortDate activationDate = new(2206);
        ShortDate expirationDate = new(2407);

        DateRange dateRange = new(activationDate, expirationDate);

        Assertion(() => { Assert.True(dateRange.IsActive()); });
    }

    [Fact]
    public void DateRange_Initialize_IsExpired()
    {
        ShortDate activationDate = new(1905);
        ShortDate expirationDate = new(2109);

        DateRange dateRange = new(activationDate, expirationDate);

        Assertion(() => { Assert.True(dateRange.IsExpired()); });
    }

    #endregion
}