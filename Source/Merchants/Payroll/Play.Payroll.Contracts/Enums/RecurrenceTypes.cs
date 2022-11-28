using System.Collections.Immutable;

using Play.Core;

namespace Play.Payroll.Contracts.Enums;

public record RecurrenceTypes : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, RecurrenceTypes> _ValueObjectMap;
    public static readonly RecurrenceTypes Empty;
    public static readonly RecurrenceTypes Weekly;
    public static readonly RecurrenceTypes Biweekly;
    public static readonly RecurrenceTypes SemiMonthly;
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