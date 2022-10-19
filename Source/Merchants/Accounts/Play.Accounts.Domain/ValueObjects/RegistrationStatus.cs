using Play.Accounts.Domain.Enums;
using Play.Core;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.ValueObjects;

public record RegistrationStatus : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public RegistrationStatus(string value) : base(value)
    {
        if (!RegistrationStatuses.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(RegistrationStatus)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(RegistrationStatus value)
    {
        return value.Value;
    }

    #endregion
}