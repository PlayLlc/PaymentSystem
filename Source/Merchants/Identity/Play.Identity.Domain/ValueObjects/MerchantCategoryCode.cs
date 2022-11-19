using Play.Core;
using Play.Domain.ValueObjects;
using Play.Identity.Domain.Enums;

namespace Play.Identity.Domain.ValueObjects;

public record MerchantCategoryCode : ValueObject<ushort>
{
    #region Instance Values

    public readonly string Name;

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public MerchantCategoryCode(ushort value) : base(value)
    {
        if (!MerchantCategoryCodes.Empty.TryGet(value, out EnumObject<ushort>? result))
            throw new ValueObjectException($"The {nameof(BusinessType)} provided was invalid: [{value}]");

        Name = ((MerchantCategoryCodes) result!).Name;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(MerchantCategoryCode value)
    {
        return value.Value;
    }

    #endregion
}