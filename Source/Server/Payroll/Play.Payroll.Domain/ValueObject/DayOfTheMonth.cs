using Play.Core;
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

    /// <exception cref="ValueObjectException"></exception>
    public DayOfTheMonth(int value) : base((byte) value)
    {
        checked
        {
            if (!DaysOfTheMonth.Empty.TryGet((byte) value, out EnumObject<byte>? result))
                throw new ValueObjectException($"The {nameof(DaysOfTheMonth)} provided was not recognized: [{value}];");
        }
    }

    #endregion

    #region Operator Overrides

    public static implicit operator DaysOfTheMonth(DayOfTheMonth value)
    {
        DaysOfTheMonth.Empty.TryGet(value, out EnumObject<byte>? result);

        return (DaysOfTheMonth) result!;
    }

    #endregion
}