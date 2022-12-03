﻿using Play.Core;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

namespace Play.Payroll.Domain.ValueObject;

public record DayOfTheMonth : ValueObject<byte>
{
    #region Constructor

    // Constructor for Entity Framework
    private DayOfTheMonth()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public DayOfTheMonth(byte value) : base(value)
    {
        if (!DaysOfTheWeek.Empty.TryGet(value, out EnumObject<byte>? result))
            throw new ValueObjectException($"The {nameof(DaysOfTheMonth)} provided was not recognized: [{value}];");
    }

    public DayOfTheMonth(int value) : base((byte) value)
    {
        checked
        {
            if (!DaysOfTheMonth.Empty.TryGet((byte) value, out EnumObject<byte>? result))
                throw new ValueObjectException($"The {nameof(DaysOfTheMonth)} provided was not recognized: [{value}];");
        }
    }

    #endregion
}