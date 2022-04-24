using System;
using System.Collections.Generic;

using Play.Core;
using Play.Core.Extensions;

namespace Play.Globalization.Currency;

// WARNING:==========================================================================================================================================================================================================================================
// TODO: Re-read the section about the money pattern in 'Patterns of Enterprise Application Architecture'. There are a lot of bugs that commonly occur when processing money so let's make sure we do our best to prevent the known issues beforehand
// WARNING:==========================================================================================================================================================================================================================================
/// <summary>
///     Agnostic fiat value of the currency type specified in the <see cref="CultureProfile" />
/// </summary>
public record Money : IEqualityComparer<Money>
{
    #region Instance Values

    // BUG: What about negative values my bro?
    private readonly ulong _Amount;
    private readonly Currency _Currency;

    #endregion

    #region Constructor

    public Money(ulong amount, Alpha3CurrencyCode currencyCode)
    {
        _Amount = amount;
        _Currency = CurrencyCodeRepository.Get(currencyCode);
    }

    public Money(ulong amount, NumericCurrencyCode currencyCode)
    {
        _Amount = amount;
        _Currency = CurrencyCodeRepository.Get(currencyCode);
    }

    private Money(ulong amount, Currency currency)
    {
        _Amount = amount;
        _Currency = currency;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Add
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Money Add(Money value)
    {
        if (_Currency != value._Currency)
        {
            throw new InvalidOperationException(
                $"The money could not be altered because the argument {nameof(value)} has a numeric currency code of: [{value._Currency}] which is different than: [{_Currency}]");
        }

        return new Money(_Amount + value._Amount, _Currency);
    }

    /// <summary>
    ///     Formats the money value to string according to the local culture of this type
    /// </summary>
    public string AsLocalFormat(CultureProfile cultureProfile) => cultureProfile.GetFiatFormat(this);

    public NumericCurrencyCode GetCurrencyCode(CultureProfile cultureProfile) => cultureProfile.GetNumericCurrencyCode();

    public bool IsBaseAmount()
    {
        if ((byte) _Currency.GetMinorUnitLength() != _Amount.GetNumberOfDigits())
            return false;

        if ((_Amount / (byte) _Currency.GetMinorUnitLength()) == 1)
            return true;

        return false;
    }

    /// <summary>
    ///     Returns true if the <see cref="Money" /> objects share a common currency between one another
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsCommonCurrency(Money other) => _Currency == other._Currency;

    public bool IsZeroAmount() => _Amount == 0;

    /// <summary>
    ///     Formats the money value to string according to the local culture of this type
    /// </summary>
    public override string ToString()
    {
        int precision = _Currency.GetMinorUnitLength();
        string yourValue = $"{_Currency.GetCurrencySymbol()}{_Amount / Math.Pow(10, precision)}";

        return yourValue;
    }

    public string ToString(CultureProfile profile) => profile.GetFiatFormat(this);

    /// <summary>
    ///     Splits currency amounts based on the percentage provided by the argument
    /// </summary>
    /// <param name="value"></param>
    /// <param name="probabilitySplit"></param>
    /// <returns></returns>
    public static (Money Remaining, Money Split) Split(Money value, Probability probabilitySplit)
    {
        ulong remaining = value._Amount / (byte) probabilitySplit;
        ulong split = value._Amount - remaining;

        return (Remaining: new Money(remaining, value._Currency), new Money(split, value._Currency));
    }

    #endregion

    #region Equality

    public bool Equals(Money? x, Money? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(Money obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    /// <exception cref="InvalidOperationException"></exception>
    public static Money operator +(Money left, Money right)
    {
        if (!left.IsCommonCurrency(right))
        {
            throw new InvalidOperationException(
                $"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left._Currency.GetNumericCode()} and the argument {nameof(right)} is {right._Currency.GetNumericCode()}");
        }

        return new Money(left._Amount + right._Amount, left._Currency);
    }

    public static explicit operator ulong(Money value) => value._Amount;

    /// <exception cref="InvalidOperationException"></exception>
    public static bool operator >(Money left, Money right)
    {
        if (!left.IsCommonCurrency(right))
        {
            throw new InvalidOperationException(
                $"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left._Currency.GetNumericCode()} and the argument {nameof(right)} is {right._Currency.GetNumericCode()}");
        }

        return left._Amount > right._Amount;
    }

    /// <exception cref="InvalidOperationException"></exception>
    public static bool operator >=(Money left, Money right)
    {
        if (!left.IsCommonCurrency(right))
        {
            throw new InvalidOperationException(
                $"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left._Currency.GetNumericCode()} and the argument {nameof(right)} is {right._Currency.GetNumericCode()}");
        }

        return left._Amount >= right._Amount;
    }

    /// <exception cref="InvalidOperationException"></exception>
    public static bool operator <(Money left, Money right)
    {
        if (!left.IsCommonCurrency(right))
        {
            throw new InvalidOperationException(
                $"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left._Currency.GetNumericCode()} and the argument {nameof(right)} is {right._Currency.GetNumericCode()}");
        }

        return left._Amount < right._Amount;
    }

    /// <exception cref="InvalidOperationException"></exception>
    public static bool operator <=(Money left, Money right)
    {
        if (!left.IsCommonCurrency(right))
        {
            throw new InvalidOperationException(
                $"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left._Currency.GetNumericCode()} and the argument {nameof(right)} is {right._Currency.GetNumericCode()}");
        }

        return left._Amount >= right._Amount;
    }

    /// <exception cref="InvalidOperationException"></exception>
    public static Money operator *(Money left, Money right)
    {
        if (!left.IsCommonCurrency(right))
        {
            throw new InvalidOperationException(
                $"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left._Currency.GetNumericCode()} and the argument {nameof(right)} is {right._Currency.GetNumericCode()}");
        }

        return new Money(left._Amount * right._Amount, left._Currency);
    }

    /// <exception cref="InvalidOperationException"></exception>
    public static decimal operator /(Money left, Money right)
    {
        if (!left.IsCommonCurrency(right))
        {
            throw new InvalidOperationException(
                $"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left._Currency.GetNumericCode()} and the argument {nameof(right)} is {right._Currency.GetNumericCode()}");
        }

        return left._Amount / (decimal) right._Amount;
    }

    /// <exception cref="InvalidOperationException" />
    public static Money operator -(Money left, Money right)
    {
        if (!left.IsCommonCurrency(right))
        {
            throw new InvalidOperationException(
                $"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left._Currency.GetNumericCode()} and the argument {nameof(right)} is {right._Currency.GetNumericCode()}");
        }

        return new Money(left._Amount - right._Amount, left._Currency);
    }

    #endregion
}