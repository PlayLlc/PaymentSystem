using Play.Core;
using Play.Domain.ValueObjects;
using Play.Payroll.Contracts.Enums;

namespace Play.Payroll.Domain.ValueObject;

public record CompensationType : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private CompensationType()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public CompensationType(string value) : base(value)
    {
        if (!CompensationTypes.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(CompensationType)} provided was not recognized: [{value}];");
    }

    #endregion
}