using System.Collections.Immutable;

using Play.Core;

namespace Play.Payroll.Contracts.Enums;

public record RecurrenceTypes : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, RecurrenceTypes> _ValueObjectMap;
    public static readonly RecurrenceTypes Empty;

    /// <summary>
    ///     Weekly pay schedules pay employees every week on a designated day of the week
    /// </summary>
    public static readonly RecurrenceTypes Weekly;

    /// <summary>
    ///     Bi-weekly pay schedules pay employees once every two weeks on a designated day of the week
    /// </summary>
    public static readonly RecurrenceTypes Biweekly;

    /// <summary>
    ///     Semi-monthly pay schedules pay employees two times a month on fixed dates, such as the 1st and the 15th
    /// </summary>
    public static readonly RecurrenceTypes SemiMonthly;

    /// <summary>
    ///     Monthly pay schedules pay employees every month on a designated date, such as the 1st of the month
    /// </summary>
    public static readonly RecurrenceTypes Monthly;

    #endregion

    #region Constructor

    private RecurrenceTypes(string value) : base(value)
    { }

    static RecurrenceTypes()
    {
        Empty = new RecurrenceTypes("");
        Weekly = new RecurrenceTypes(nameof(Weekly));
        Biweekly = new RecurrenceTypes(nameof(Biweekly));
        SemiMonthly = new RecurrenceTypes(nameof(SemiMonthly));
        Monthly = new RecurrenceTypes(nameof(Monthly));

        _ValueObjectMap = new Dictionary<string, RecurrenceTypes>
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
        if (_ValueObjectMap.TryGetValue(value, out RecurrenceTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}