using Play.Core;
using Play.Domain.ValueObjects;
using Play.Identity.Domain.Enums;

namespace Play.Identity.Domain.ValueObjects;

public record TimeClockStatus : ValueObject<string>
{
    #region Constructor

    // Constructor for Entity Framework
    private TimeClockStatus()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public TimeClockStatus(string value) : base(value)
    {
        if (!TimeClockStatuses.Empty.TryGet(value, out EnumObjectString? result))
            throw new ValueObjectException($"The {nameof(TimeClockStatus)} provided was invalid: [{value}]");
    }

    #endregion

    #region Operator Overrides

    public static implicit operator string(TimeClockStatus value) => value.Value;

    #endregion
}