using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.Enums;

public sealed record ApplicationPriorityRankTypes : EnumObject<byte>
{
    #region Static Metadata

    public static readonly ApplicationPriorityRankTypes Empty = new();
    private static readonly ImmutableSortedDictionary<byte, ApplicationPriorityRankTypes> _ValueObjectMap;
    public static readonly ApplicationPriorityRankTypes Eighth;
    public static readonly ApplicationPriorityRankTypes Eleventh;
    public static readonly ApplicationPriorityRankTypes Fifteenth;
    public static readonly ApplicationPriorityRankTypes Fifth;
    public static readonly ApplicationPriorityRankTypes First;
    public static readonly ApplicationPriorityRankTypes Fourteenth;
    public static readonly ApplicationPriorityRankTypes Fourth;
    public static readonly ApplicationPriorityRankTypes Ninth;
    public static readonly ApplicationPriorityRankTypes Second;
    public static readonly ApplicationPriorityRankTypes Seventh;
    public static readonly ApplicationPriorityRankTypes Sixth;
    public static readonly ApplicationPriorityRankTypes Tenth;
    public static readonly ApplicationPriorityRankTypes Third;
    public static readonly ApplicationPriorityRankTypes Thirteenth;
    public static readonly ApplicationPriorityRankTypes Twelveth;
    public static readonly ApplicationPriorityRankTypes NotAvailable;

    #endregion

    #region Constructor

    public ApplicationPriorityRankTypes() : base()
    { }

    static ApplicationPriorityRankTypes()
    {
        NotAvailable = new ApplicationPriorityRankTypes(0);
        First = new ApplicationPriorityRankTypes(1);
        Second = new ApplicationPriorityRankTypes(2);
        Third = new ApplicationPriorityRankTypes(3);
        Fourth = new ApplicationPriorityRankTypes(4);
        Fifth = new ApplicationPriorityRankTypes(5);
        Sixth = new ApplicationPriorityRankTypes(6);
        Seventh = new ApplicationPriorityRankTypes(7);
        Eighth = new ApplicationPriorityRankTypes(8);
        Ninth = new ApplicationPriorityRankTypes(9);
        Tenth = new ApplicationPriorityRankTypes(10);
        Eleventh = new ApplicationPriorityRankTypes(11);
        Twelveth = new ApplicationPriorityRankTypes(12);
        Thirteenth = new ApplicationPriorityRankTypes(13);
        Fourteenth = new ApplicationPriorityRankTypes(14);
        Fifteenth = new ApplicationPriorityRankTypes(15);
        _ValueObjectMap = new Dictionary<byte, ApplicationPriorityRankTypes>
        {
            {1, First},
            {2, Second},
            {3, Third},
            {4, Fourth},
            {5, Fifth},
            {6, Sixth},
            {7, Seventh},
            {8, Eighth},
            {9, Ninth},
            {10, Tenth},
            {11, Eleventh},
            {12, Twelveth},
            {13, Thirteenth},
            {14, Fourteenth},
            {15, Fifteenth}
        }.ToImmutableSortedDictionary();
    }

    private ApplicationPriorityRankTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ApplicationPriorityRankTypes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out ApplicationPriorityRankTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static ApplicationPriorityRankTypes Get(byte value)
    {
        const byte bitMask = 0b11110000;

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public int CompareTo(ApplicationPriorityRankTypes other) => _Value.CompareTo(other._Value);

    #endregion

    #region Operator Overrides

    public static implicit operator ApplicationPriorityRank(ApplicationPriorityRankTypes enumObject) => new(enumObject._Value);

    #endregion
}