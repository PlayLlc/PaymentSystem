using Play.Core;
using Play.Domain.ValueObjects;
using Play.Payroll.Contracts.Enums;

namespace Play.Payroll.Domain.ValueObject;

public record PaydayRecurrence : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private PaydayRecurrence()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public PaydayRecurrence(string value) : base(value)
    {
        if (!PaydayRecurrences.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(PaydayRecurrence)} provided was not recognized: [{value}];");
    }

    #endregion
}