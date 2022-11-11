using Play.Core;
using System.Collections.Immutable;

namespace Play.Underwriting.Domain.Enums;

public record EntityType : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, EntityType> _ValueObjectMap;
    public static EntityType Empty;
    public static EntityType Individual;

    #endregion

    #region Constructor

    public EntityType(string value) : base(value) { }

    static EntityType()
    {
        Empty = new EntityType("");
        Individual = new EntityType("individual");

        _ValueObjectMap = new Dictionary<string, EntityType>
        {
            {Empty, Empty },
            {Individual, Individual }
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override EntityType[] GetAll()
    {
        return _ValueObjectMap.Values.ToArray();
    }

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out EntityType? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}
