using System.ComponentModel.DataAnnotations;

using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.ValueObjects;

public record Email : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Email(string value) : base(value)
    {
        if (!new EmailAddressAttribute().IsValid(Value))
            throw new ValueObjectException($"The {nameof(Email)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(Email value)
    {
        return value.Value;
    }

    #endregion
}