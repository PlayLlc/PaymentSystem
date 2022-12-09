﻿using Play.Core;
using Play.Domain.ValueObjects;
using Play.Identity.Domain.Enums;

namespace Play.Identity.Domain.ValueObjects;

public record BusinessType : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private BusinessType()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public BusinessType(string value) : base(value)
    {
        if (!BusinessTypes.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(BusinessType)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(BusinessType value) => value.Value;

    #endregion
}