using Play.Core;
using Play.Domain.ValueObjects;
using Play.Identity.Domain.Enums;

namespace Play.Identity.Domain.ValueObjects;

public record UserRegistrationStatus : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public UserRegistrationStatus(string value) : base(value)
    {
        if (!UserRegistrationStatuses.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(UserRegistrationStatus)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(UserRegistrationStatus value) => value.Value;

    #endregion
}