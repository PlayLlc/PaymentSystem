using System;
using System.Collections.Generic;

using Play.Core.Extensions;

namespace Play.Globalization.Currency;

/// <summary>
///     Agnostic fiat value of the currency type specified in the <see cref="CultureProfile" />
/// </summary>
public class Money : IEqualityComparer<Money>, IEquatable<Money>
{
    #region Instance Values

    private readonly ulong _Amount;
    private readonly CultureProfile _CultureProfile;

    #endregion

    #region Constructor

    public Money(ulong amount, CultureProfile cultureProfile)
    {
        _Amount = amount;
        _CultureProfile = cultureProfile;
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
        if (_CultureProfile != value._CultureProfile)
        {
            throw new
                InvalidOperationException($"The money could not be altered because the argument {nameof(value)} is of currency {value._CultureProfile} which is different than {_CultureProfile}");
        }

        return new Money(_Amount + value._Amount, _CultureProfile);
    }

    /// <summary>
    ///     Formats the money value to string according to the local culture of this type
    /// </summary>
    public string AsLocalFormat() => _CultureProfile.GetFiatFormat(this);

    public NumericCurrencyCode GetCurrencyCode() => _CultureProfile.GetNumericCurrencyCode();

    public bool IsBaseAmount()
    {
        if ((byte) _CultureProfile.GetMinorUnitLength() != _Amount.GetNumberOfDigits())
            return false;

        if ((_Amount / (byte) _CultureProfile.GetMinorUnitLength()) == 1)
            return true;

        return false;
    }

    public bool IsCurrencyEqual(Money other) => _CultureProfile.GetNumericCurrencyCode() == other._CultureProfile.GetNumericCurrencyCode();
    public bool IsCurrencyEqual(Alpha3CurrencyCode code) => _CultureProfile.GetAlpha3CurrencyCode() == code;
    public bool IsCurrencyEqual(NumericCurrencyCode code) => _CultureProfile.GetNumericCurrencyCode() == code;
    public bool IsZeroAmount() => _Amount == 0;

    /// <summary>
    ///     Subtract
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Money Subtract(Money value)
    {
        if (_CultureProfile != value._CultureProfile)
        {
            throw new
                InvalidOperationException($"The money could not be altered because the argument {nameof(value)} is of currency {value._CultureProfile} which is different than {_CultureProfile}");
        }

        return new Money(_Amount - value._Amount, _CultureProfile);
    }

    /// <summary>
    ///     Formats the money value to string according to the local culture of this type
    /// </summary>
    public override string ToString() => AsLocalFormat();

    #endregion

    #region Equality

    public bool Equals(Money? other)
    {
        if (other is null)
            return false;

        return (_Amount == other._Amount) && (GetCurrencyCode() == other.GetCurrencyCode());
    }

    public bool Equals(Money? x, Money? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals(object? obj) => obj is Money money && Equals(money);
    public int GetHashCode(Money obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 832399;

        return unchecked((hash * _Amount.GetHashCode()) + _CultureProfile.GetHashCode());
    }

    #endregion

    #region Operator Overrides

    public static Money operator +(Money left, Money right)
    {
        if (!left.IsCurrencyEqual(right))
        {
            throw new
                InvalidOperationException($"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left.GetCurrencyCode()} and the argument {nameof(right)} is {right.GetCurrencyCode()}");
        }

        return new Money(left._Amount + right._Amount, left._CultureProfile);
    }

    public static Money operator /(Money left, Money right)
    {
        if (right._Amount == 0)
            throw new ArgumentOutOfRangeException(nameof(right), $"The argument {nameof(right)} is invalid because it is equal to zero");

        if (!left.IsCurrencyEqual(right))
        {
            throw new
                InvalidOperationException($"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left.GetCurrencyCode()} and the argument {nameof(right)} is {right.GetCurrencyCode()}");
        }

        return new Money(left._Amount / right._Amount, left._CultureProfile);
    }

    public static bool operator ==(Money left, Money right) => left.Equals(right);
    public static explicit operator ulong(Money value) => value._Amount;

    public static bool operator >(Money left, Money right)
    {
        if (!left.IsCurrencyEqual(right))
        {
            throw new
                InvalidOperationException($"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left.GetCurrencyCode()} and the argument {nameof(right)} is {right.GetCurrencyCode()}");
        }

        return left._Amount > right._Amount;
    }

    public static bool operator >=(Money left, Money right)
    {
        if (!left.IsCurrencyEqual(right))
        {
            throw new
                InvalidOperationException($"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left.GetCurrencyCode()} and the argument {nameof(right)} is {right.GetCurrencyCode()}");
        }

        return left._Amount >= right._Amount;
    }

    public static bool operator !=(Money left, Money right) => !left.Equals(right);

    public static bool operator <(Money left, Money right)
    {
        if (!left.IsCurrencyEqual(right))
        {
            throw new
                InvalidOperationException($"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left.GetCurrencyCode()} and the argument {nameof(right)} is {right.GetCurrencyCode()}");
        }

        return left._Amount < right._Amount;
    }

    public static bool operator <=(Money left, Money right)
    {
        if (!left.IsCurrencyEqual(right))
        {
            throw new
                InvalidOperationException($"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left.GetCurrencyCode()} and the argument {nameof(right)} is {right.GetCurrencyCode()}");
        }

        return left._Amount >= right._Amount;
    }

    public static Money operator *(Money left, Money right)
    {
        if (!left.IsCurrencyEqual(right))
        {
            throw new
                InvalidOperationException($"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left.GetCurrencyCode()} and the argument {nameof(right)} is {right.GetCurrencyCode()}");
        }

        return new Money(left._Amount * right._Amount, left._CultureProfile);
    }

    public static Money operator -(Money left, Money right)
    {
        if (!left.IsCurrencyEqual(right))
        {
            throw new
                InvalidOperationException($"Currencies do not match. The numeric currency code of argument {nameof(left)} is {left.GetCurrencyCode()} and the argument {nameof(right)} is {right.GetCurrencyCode()}");
        }

        return new Money(left._Amount - right._Amount, left._CultureProfile);
    }

    #endregion
}