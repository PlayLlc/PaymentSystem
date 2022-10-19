using Play.Accounts.Domain.Enums;
using Play.Core;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.ValueObjects;

public record MerchantRegistrationStatus : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public MerchantRegistrationStatus(string value) : base(value)
    {
        if (!MerchantRegistrationStatuses.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(MerchantRegistrationStatus)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(MerchantRegistrationStatus value)
    {
        return value.Value;
    }

    #endregion
}

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

    public static implicit operator string(UserRegistrationStatus value)
    {
        return value.Value;
    }

    #endregion
}