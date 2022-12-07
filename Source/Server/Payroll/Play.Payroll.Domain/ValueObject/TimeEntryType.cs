using Play.Core;
using Play.Domain.ValueObjects;
using Play.Payroll.Contracts.Enums;

namespace Play.Payroll.Domain.ValueObject;

public record TimeEntryType : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private TimeEntryType()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public TimeEntryType(string value) : base(value)
    {
        if (!TimeEntryTypes.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(TimeEntryType)} provided was not recognized: [{value}];");
    }

    #endregion
}