using System.Collections.Immutable;

using Play.Core;

namespace Play.Payroll.Contracts.Enums;

public record PaydayRecurrences : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, PaydayRecurrences> _ValueObjectMap;
    public static readonly PaydayRecurrences Empty;

    /// <summary>
    ///     Weekly pay schedules pay employees every week on a designated day of the week
    /// </summary>
    public static readonly PaydayRecurrences Weekly;

    /// <summary>
    ///     Bi-weekly pay schedules pay employees once every two weeks on a designated day of the week
    /// </summary>
    public static readonly PaydayRecurrences Biweekly;

    /// <summary>
    ///     Semi-monthly pay schedules pay employees two times a month on fixed dates, such as the 1st and the 15th
    /// </summary>
    public static readonly PaydayRecurrences SemiMonthly;

    /// <summary>
    ///     Monthly pay schedules pay employees every month on a designated date, such as the 1st of the month
    /// </summary>
    public static readonly PaydayRecurrences Monthly;

    #endregion

    #region Constructor

    private PaydayRecurrences(string value) : base(value)
    { }

    static PaydayRecurrences()
    {
        Empty = new PaydayRecurrences("");
        Weekly = new PaydayRecurrences(nameof(Weekly));
        Biweekly = new PaydayRecurrences(nameof(Biweekly));
        SemiMonthly = new PaydayRecurrences(nameof(SemiMonthly));
        Monthly = new PaydayRecurrences(nameof(Monthly));

        _ValueObjectMap = new Dictionary<string, PaydayRecurrences>
        {
            {Weekly, Weekly},
            {Biweekly, Biweekly},
            {SemiMonthly, SemiMonthly},
            {Monthly, Monthly}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override EnumObjectString[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out PaydayRecurrences? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}