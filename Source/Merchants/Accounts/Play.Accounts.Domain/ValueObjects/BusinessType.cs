using Play.Core;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.Enums;

public record BusinessType : ValueObject<string>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public BusinessType(string value) : base(value)
    {
        if (!BusinessTypes.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(BusinessType)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(BusinessType value)
    {
        return value.Value;
    }

    #endregion
}