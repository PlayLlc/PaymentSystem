using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;

namespace Play.Emv.DataElements.Emv;

public sealed record ApplicationPriorityRankTypes : EnumObject<ApplicationPriorityRank>
{
    #region Static Metadata

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

    static ApplicationPriorityRankTypes()
    {
        NotAvailable = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(0));
        First = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(1));
        Second = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(2));
        Third = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(3));
        Fourth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(4));
        Fifth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(5));
        Sixth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(6));
        Seventh = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(7));
        Eighth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(8));
        Ninth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(9));
        Tenth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(10));
        Eleventh = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(11));
        Twelveth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(12));
        Thirteenth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(13));
        Fourteenth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(14));
        Fifteenth = new ApplicationPriorityRankTypes(new ApplicationPriorityRank(15));
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

    public ApplicationPriorityRankTypes(ApplicationPriorityRank value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo(ApplicationPriorityRankTypes other) => _Value.CompareTo(other._Value);

    public static ApplicationPriorityRankTypes Get(byte value)
    {
        const byte bitMask = 0b11110000;

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion
}