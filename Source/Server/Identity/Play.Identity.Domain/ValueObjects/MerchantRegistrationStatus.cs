using Play.Core;
using Play.Domain.ValueObjects;
using Play.Identity.Domain.Enums;

namespace Play.Identity.Domain.ValueObjects;

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

    public static implicit operator string(MerchantRegistrationStatus value) => value.Value;

    #endregion
}