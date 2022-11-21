using System.Collections.Immutable;

using Play.Core;

namespace Play.Loyalty.Domain.Enums;

public record RewardTypes : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, RewardTypes> _ValueObjectMap;
    public static readonly RewardTypes Empty;
    public static readonly RewardTypes AmountOff;
    public static readonly RewardTypes PercentageOff;

    #endregion

    #region Constructor

    private RewardTypes(string value) : base(value)
    { }

    static RewardTypes()
    {
        Empty = new RewardTypes("");
        AmountOff = new RewardTypes(nameof(AmountOff));
        PercentageOff = new RewardTypes(nameof(PercentageOff));

        _ValueObjectMap = new Dictionary<string, RewardTypes>
        {
            {AmountOff, AmountOff},
            {PercentageOff, PercentageOff}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override RewardTypes[] GetAll()
    {
        return _ValueObjectMap.Values.ToArray();
    }

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out RewardTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}