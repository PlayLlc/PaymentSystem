using System.Collections.Immutable;

using Play.Core;

namespace Play.Payroll.Contracts.Enums;

public record CompensationTypes : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, CompensationTypes> _ValueObjectMap;
    public static readonly CompensationTypes Empty;
    public static readonly CompensationTypes Hourly;
    public static readonly CompensationTypes Salary;

    #endregion

    #region Constructor

    private CompensationTypes(string value) : base(value)
    { }

    static CompensationTypes()
    {
        Empty = new CompensationTypes("");
        Hourly = new CompensationTypes(nameof(Hourly));
        Salary = new CompensationTypes(nameof(Salary));

        _ValueObjectMap = new Dictionary<string, CompensationTypes>
        {
            {Hourly, Hourly},
            {Salary, Salary}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override EnumObjectString[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out CompensationTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}