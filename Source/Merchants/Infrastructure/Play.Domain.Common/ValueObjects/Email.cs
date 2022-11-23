using System.ComponentModel.DataAnnotations;

using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;

namespace Play.Domain.Common.ValueObjects;

public record Email : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Email(string value) : base(value)
    {
        if (!IsValid(value))
            throw new ValueObjectException($"The {nameof(Email)} provided was invalid: [{value}]");
    }

    #endregion

    #region Instance Members

    public static bool IsValid(string value)
    {
        if (!new EmailAddressAttribute().IsValid(value))
            return false;

        return true;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(Email value) => value.Value;

    #endregion
}

public record MoneyValueObject : ValueObject<Money>
{
    #region Instance Values

    public readonly ulong Amount;
    public readonly NumericCurrencyCode NumericCurrencyCode;

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public MoneyValueObject(Money value) : base(value)
    {
        Amount = value.GetAmount();
        NumericCurrencyCode = value.GetNumericCurrencyCode();
    }

    #endregion

    #region Instance Members

    public MoneyDto AsDto() => new(Value);

    public static bool IsValid(string value)
    {
        if (!new EmailAddressAttribute().IsValid(value))
            return false;

        return true;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator Money(MoneyValueObject value) => value.Value;
    public static implicit operator MoneyValueObject(Money value) => new(value);

    #endregion
}