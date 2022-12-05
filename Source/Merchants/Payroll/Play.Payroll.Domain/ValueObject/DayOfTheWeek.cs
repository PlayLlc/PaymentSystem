using Play.Core;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

namespace Play.Payroll.Domain.ValueObject;

public record DayOfTheWeek : ValueObject<byte>
{
    #region Constructor

    // Constructor for Entity Framework
    private DayOfTheWeek()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public DayOfTheWeek(byte value) : base(value)
    {
        if (!DaysOfTheWeek.Empty.TryGet(value, out EnumObject<byte>? result))
            throw new ValueObjectException($"The {nameof(DayOfTheWeek)} provided was not recognized: [{value}];");
    }

    public DayOfTheWeek(int value) : base((byte) value)
    {
        checked
        {
            if (!DaysOfTheWeek.Empty.TryGet((byte) value, out EnumObject<byte>? result))
                throw new ValueObjectException($"The {nameof(DayOfTheWeek)} provided was not recognized: [{value}];");
        }
    }

    public DayOfTheWeek(DayOfWeek value) : base((byte) value)
    {
        checked
        {
            if (!DaysOfTheWeek.Empty.TryGet((byte) value, out EnumObject<byte>? result))
                throw new ValueObjectException($"The {nameof(DayOfTheWeek)} provided was not recognized: [{value}];");
        }
    }

    #endregion

    #region Operator Overrides

    public static implicit operator DaysOfTheWeek(DayOfTheWeek value)
    {
        DaysOfTheWeek.Empty.TryGet(value, out EnumObject<byte>? result);

        return (DaysOfTheWeek) result!;
    }

    public static implicit operator DayOfTheWeek(DayOfWeek value) => new((byte) value);

    #endregion
}