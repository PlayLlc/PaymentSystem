using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Dtos;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;

namespace Play.Domain.Common.ValueObjects;

public class MoneyValueObject
{
    #region Instance Values

    public readonly ulong Amount;
    public readonly NumericCurrencyCode NumericCurrencyCode;

    #endregion

    #region Constructor

    // private constructor for EF only
    private MoneyValueObject()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public MoneyValueObject(Money value)
    {
        Amount = value.GetAmount();
        NumericCurrencyCode = value.GetNumericCurrencyCode();
    }

    #endregion

    #region Instance Members

    public MoneyDto AsDto() =>
        new(new MoneyDto
        {
            Amount = Amount,
            NumericCurrencyCode = NumericCurrencyCode
        });

    public static bool IsValid(string value)
    {
        if (!new EmailAddressAttribute().IsValid(value))
            return false;

        return true;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator Money(MoneyValueObject value) => new(value.Amount, value.NumericCurrencyCode);
    public static implicit operator MoneyValueObject(Money value) => new(value);

    #endregion
}