﻿using Play.Accounts.Domain.Enums;
using Play.Core;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.ValueObjects;

public record MerchantCategoryCode : ValueObject<ushort>
{
    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public MerchantCategoryCode(ushort value) : base(value)
    {
        if (!MerchantCategoryCodes.Empty.TryGet(value, out EnumObject<ushort>? result))
            throw new ValueObjectException($"The {nameof(BusinessType)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(MerchantCategoryCode value)
    {
        return value.Value;
    }

    #endregion
}