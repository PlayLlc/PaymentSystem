using System.Collections.Immutable;

using Play.Core;

namespace Play.Identity.Domain.Enumss;

public record CompensationTypes : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, CompensationTypes> _ValueObjectMap;
    public static readonly CompensationTypes Empty;
    public static readonly CompensationTypes Salary;
    public static readonly CompensationTypes Hourly; 

    #endregion

    #region Constructor

    private CompensationTypes(string value) : base(value)
    { }

    static CompensationTypes()
    {
        Empty = new CompensationTypes("");
        Salary = new CompensationTypes(nameof(Salary));
        Hourly = new CompensationTypes(nameof(Hourly)); 

        _ValueObjectMap = new Dictionary<string, CompensationTypes>
        {
            {Salary, Salary},
            {Hourly, Hourly} 
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override CompensationTypes[] GetAll()
    {
        return _ValueObjectMap.Values.ToArray();
    }

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