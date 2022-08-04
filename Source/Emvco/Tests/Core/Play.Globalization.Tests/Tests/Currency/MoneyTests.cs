using Play.Globalization.Currency;

using Xunit;

namespace Play.Globalization.Tests.Tests.Currency;

public class MoneyTests
{
    //ResolveCurrency(840, "USD", formatMap)

    #region IsBaseAmount

    [Fact]
    public void Money_IsBaseAmount_ReturnsFalse()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'U', 'S', 'D' };
        Alpha3CurrencyCode alpha3CurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 1234;

        Money sut = new Money(amount, alpha3CurrencyCode);

        Assert.False(sut.IsBaseAmount());
    }

    [Fact]
    public void Money_IsBaseAmount_ReturnsTrue()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'U', 'S', 'D' };
        Alpha3CurrencyCode alpha3CurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 100;

        Money sut = new Money(amount, alpha3CurrencyCode);

        Assert.True(sut.IsBaseAmount());
    }

    [Fact]
    public void Money_IsBaseAmountForGivenValue_ReturnsFalse()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode alpha3CurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 200;

        Money sut = new Money(amount, alpha3CurrencyCode);

        Assert.False(sut.IsBaseAmount());
    }

    [Fact]
    public void Money_IsBaseAmountForGivenValue_ReturnsTrue()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode alpha3CurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 100;

        Money sut = new Money(amount, alpha3CurrencyCode);

        Assert.True(sut.IsBaseAmount());
    }

    #endregion

    #region IsZeroAmount

    [Fact]
    public void Money_IsZeroAmount_ReturnsTrue()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode alpha3CurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 0;

        Money sut = new Money(amount, alpha3CurrencyCode);

        Assert.True(sut.IsZeroAmount());
    }

    [Fact]
    public void Money_IsZeroAmount_ReturnsFalse()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode alpha3CurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 100;

        Money sut = new Money(amount, alpha3CurrencyCode);

        Assert.False(sut.IsZeroAmount());
    }

    #endregion

    #region IsCommonCurrency

    [Fact]
    public void Money_IsCommonCurrency_ReturnsFalse()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 100;

        Money sut = new Money(amount, eurCurrencyCode);

        ReadOnlySpan<char> currencyCode2 = stackalloc char[] { 'U', 'S', 'D' };
        Alpha3CurrencyCode usdCurrencyCode = new Alpha3CurrencyCode(currencyCode2);
        ulong otherAmount = 100;

        Money other = new Money(otherAmount, usdCurrencyCode);

        Assert.False(sut.IsCommonCurrency(other));
    }

    [Fact]
    public void Money_IsCommonCurrency_ReturnsTrue()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 100;

        Money sut = new Money(amount, eurCurrencyCode);

        ReadOnlySpan<char> sameCurrencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode otherCurrency = new Alpha3CurrencyCode(sameCurrencyCode);
        ulong otherAmount = 100;

        Money other = new Money(otherAmount, otherCurrency);

        Assert.True(sut.IsCommonCurrency(other));
    }

    #endregion

    #region Add

    [Fact]
    public void Money_AddMore_ReturnsExpectedResult1()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 100;

        Money sut = new Money(amount, eurCurrencyCode);

        ulong otherAmount = 100;
        Money other = new Money(otherAmount, eurCurrencyCode);

        Money sum = sut.Add(other);

        Assert.Equal((ulong)200, (ulong)sum);
    }

    [Fact]
    public void Money_AddMore_ReturnsExpectedResult2()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 125;

        Money sut = new Money(amount, eurCurrencyCode);

        ulong otherAmount = 200;
        Money other = new Money(otherAmount, eurCurrencyCode);

        Money sum = sut.Add(other);

        Assert.Equal((ulong)325, (ulong)sum);
    }

    [Fact]
    public void Money_AddMoreHasDifferentCurrency_ThrowsException()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 100;

        Money sut = new Money(amount, eurCurrencyCode);

        ReadOnlySpan<char> sameCurrencyCode = stackalloc char[] { 'U', 'S', 'D' };
        Alpha3CurrencyCode otherCurrency = new Alpha3CurrencyCode(sameCurrencyCode);
        ulong otherAmount = 100;
        Money other = new Money(otherAmount, otherCurrency);

        Assert.Throws<InvalidOperationException>(() =>
        {
            Money sum = sut.Add(other);
        });
    }

    #endregion

    #region Operators

    [Fact]
    public void Money_IsGreaterThanOther_ReturnsTrue()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 325;

        Money sut = new Money(amount, eurCurrencyCode);
        
        ulong otherAmount = 200;
        Money other = new Money(otherAmount, eurCurrencyCode);

        Assert.True(sut > other);
    }

    [Fact]
    public void Money_IsGreaterThanOther_ReturnsFalse()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 200;

        Money sut = new Money(amount, eurCurrencyCode);

        ulong otherAmount = 420;
        Money other = new Money(otherAmount, eurCurrencyCode);

        Assert.False(sut > other);
    }

    [Fact]
    public void Money_IsLesserThanOther_ReturnsTrue()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 200;

        Money sut = new Money(amount, eurCurrencyCode);

        ulong otherAmount = 420;
        Money other = new Money(otherAmount, eurCurrencyCode);

        Assert.True(sut < other);
    }

    [Fact]
    public void Money_IsLesserThanOther_ReturnsTFalse()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 550;

        Money sut = new Money(amount, eurCurrencyCode);

        ulong otherAmount = 420;
        Money other = new Money(otherAmount, eurCurrencyCode);

        Assert.False(sut < other);
    }

    [Fact]
    public void Money_AddOther_ReturnsExpectedResult()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 550;

        Money sut = new Money(amount, eurCurrencyCode);

        ulong otherAmount = 420;
        Money other = new Money(otherAmount, eurCurrencyCode);

        Assert.Equal((ulong) 970, (ulong)(sut + other));
    }

    [Fact]
    public void Money_SubstractOther_ReturnsExpectedResult()
    {
        ReadOnlySpan<char> currencyCode = stackalloc char[] { 'E', 'U', 'R' };
        Alpha3CurrencyCode eurCurrencyCode = new Alpha3CurrencyCode(currencyCode);
        ulong amount = 550;

        Money sut = new Money(amount, eurCurrencyCode);

        ulong otherAmount = 420;
        Money other = new Money(otherAmount, eurCurrencyCode);

        Assert.Equal((ulong)130, (ulong)(sut - other));
    }

    #endregion
}
