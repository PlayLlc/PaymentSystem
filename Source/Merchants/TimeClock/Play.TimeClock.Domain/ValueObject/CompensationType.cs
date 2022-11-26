using Play.Core;
using Play.Domain.ValueObjects;
using Play.Identity.Domain.Enumss;

namespace Play.TimeClock.Domain.ValueObject;

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
            throw new ValueObjectException($"The {nameof(CompensationType)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(CompensationType value) => value.Value;

    #endregion
}