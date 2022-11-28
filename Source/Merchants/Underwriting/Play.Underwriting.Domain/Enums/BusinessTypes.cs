using System.Collections.Immutable;

using Play.Core;

namespace Play.Underwriting.Domain.Enums;

public record BusinessTypes : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, BusinessTypes> _ValueObjectMap;
    public static readonly BusinessTypes Empty;
    public static readonly BusinessTypes SoleProprietorship;
    public static readonly BusinessTypes Partnership;
    public static readonly BusinessTypes LimitedLiability;
    public static readonly BusinessTypes NonProfit;
    public static readonly BusinessTypes Exempt;

    #endregion

    #region Constructor

    private BusinessTypes(string value) : base(value)
    { }

    static BusinessTypes()
    {
        Empty = new BusinessTypes("");
        SoleProprietorship = new BusinessTypes(nameof(SoleProprietorship));
        Partnership = new BusinessTypes(nameof(Partnership));
        LimitedLiability = new BusinessTypes(nameof(LimitedLiability));
        NonProfit = new BusinessTypes(nameof(NonProfit));
        Exempt = new BusinessTypes(nameof(Exempt));

        _ValueObjectMap = new Dictionary<string, BusinessTypes>
        {
            {SoleProprietorship, SoleProprietorship},
            {Partnership, Partnership},
            {LimitedLiability, LimitedLiability},
            {NonProfit, NonProfit},
            {Exempt, Exempt}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override BusinessTypes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out BusinessTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}