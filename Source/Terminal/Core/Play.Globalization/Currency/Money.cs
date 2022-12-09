﻿using System;
using System.Collections.Generic;

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
    ///     Gets the major currency amount without the minor currency. For example, for USD we would return the dollar amount
    ///     without the cents
    /// </summary>
    /// <returns></returns>
    public ulong GetMajorCurrencyAmount() => (ulong) (_Amount % Math.Pow(10, _Currency.GetMinorUnitLength()));

    public ulong GetAmount() => _Amount;
    public bool IsPositiveNonZeroAmount() => _Amount > 0;

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

        return new(_Amount + value._Amount, _Currency);
    }

    /// <summary>
    ///     Formats the money value to string according to the local culture of this type
    /// </summary>
    public string AsLocalFormat(CultureProfile cultureProfile) => cultureProfile.GetFiatFormat(this);

    /// <summary>
    ///     This is a less precise way to format this Money object as a string
    /// </summary>
    /// <returns></returns>
    public string AsLocalFormat() => $"{_Currency.GetCurrencySymbol()}{_Currency.ToLocalDecimalAmount(_Amount)}";

    public static string AsLocalFormat(ulong amount, NumericCurrencyCode currencyCode)
    {
        Currency? currency = CurrencyCodeRepository.Get(currencyCode);

        return $"{currency.GetCurrencySymbol()}{currency.ToLocalDecimalAmount(amount)}";
    }

    public NumericCurrencyCode GetNumericCurrencyCode() => _Currency.GetNumericCode();

    public bool IsBaseAmount()
    {
        if ((_Amount / Math.Pow(10, _Currency.GetMinorUnitLength())) == 1)
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
    public override string ToString() => AsLocalFormat();

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

        return new(left._Amount + right._Amount, left._Currency);
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

        return new(left._Amount * right._Amount, left._Currency);
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

    /// <exception cref="InvalidOperationException"></exception>
    public static decimal operator /(Money left, uint right) => left._Amount / (decimal) right;

    public static decimal operator /(Money left, ulong right) => left._Amount / (decimal) right;
    public static decimal operator /(Money left, ushort right) => left._Amount / (decimal) right;
    public static decimal operator /(Money left, byte right) => left._Amount / (decimal) right;

    /// <exception cref="InvalidOperationException"></exception>
    public static Money operator *(Money left, uint right) => new(left._Amount * right, left._Currency);

    public static Money operator *(Money left, ulong right) => new(left._Amount * right, left._Currency);
    public static Money operator *(Money left, ushort right) => new(left._Amount * right, left._Currency);
    public static Money operator *(Money left, byte right) => new(left._Amount * right, left._Currency);

    /// <exception cref="InvalidOperationException" />
    public static Money operator -(Money left, Money right)
    {
        if (!left.IsCommonCurrency(right))
        {
            throw new InvalidOperationException(
                $"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left._Currency.GetNumericCode()} and the argument {nameof(right)} is {right._Currency.GetNumericCode()}");
        }

        return new(left._Amount - right._Amount, left._Currency);
    }

    #endregion
}