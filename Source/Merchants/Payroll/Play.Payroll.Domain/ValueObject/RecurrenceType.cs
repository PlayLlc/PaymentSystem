using Play.Core;
using Play.Domain.ValueObjects;
using Play.Payroll.Contracts.Enums;

namespace Play.Payroll.Domain.ValueObject;

public record RecurrenceType : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private RecurrenceType()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public RecurrenceType(string value) : base(value)
    {
        if (!RecurrenceTypes.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(RecurrenceType)} provided was not recognized: [{value}];");
    }

    #endregion
}