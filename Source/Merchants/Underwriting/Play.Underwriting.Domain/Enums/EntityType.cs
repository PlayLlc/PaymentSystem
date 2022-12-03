using System.Collections.Immutable;

using Play.Core;

namespace Play.Underwriting.Domain.Enums;

public record EntityType : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, EntityType> _ValueObjectMap;
    public static EntityType Empty;
    public static EntityType Individual;

    #endregion

    #region Constructor

    public EntityType(string value) : base(value)
    { }

    static EntityType()
    {
        Empty = new("");
        Individual = new("individual");

        _ValueObjectMap = new Dictionary<string, EntityType>
        {
            {Empty, Empty},
            {Individual, Individual}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override EntityType[] GetAll() => _ValueObjectMap.Values.ToArray();

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