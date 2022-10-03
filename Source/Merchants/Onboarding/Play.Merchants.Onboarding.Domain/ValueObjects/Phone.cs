using System.ComponentModel.DataAnnotations;

using Play.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.ValueObjects;

public record Phone : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Phone(string value) : base(value)
    {
        if (!new PhoneAttribute().IsValid(Value))
            throw new ValueObjectException($"The {nameof(Phone)} provided was invalid: [{value}]");
    }

    #endregion
}