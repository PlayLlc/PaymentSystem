using System.Collections.Immutable;

using Play.Core;

namespace Play.Inventory.Contracts.Enums;

public record StockActions : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, StockActions> _ValueObjectMap;
    public static readonly StockActions Empty;
    public static readonly StockActions Restock;
    public static readonly StockActions Return;
    public static readonly StockActions Sold;
    public static readonly StockActions Shrinkage;

    #endregion

    #region Constructor

    private StockActions(string value) : base(value)
    { }

    static StockActions()
    {
        Empty = new("");
        Restock = new(nameof(Restock));
        Return = new(nameof(Return));
        Sold = new(nameof(Sold));
        Shrinkage = new(nameof(Shrinkage));

        _ValueObjectMap = new Dictionary<string, StockActions>
        {
            {Restock, Restock},
            {Return, Return},
            {Sold, Sold},
            {Shrinkage, Shrinkage}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override EnumObjectString[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out StockActions? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}