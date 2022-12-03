using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Core;

namespace Play.Globalization.Time;

public record DaysOfTheMonth : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, DaysOfTheMonth> _ValueObjectMap;
    public static DaysOfTheMonth Empty;
    public static DaysOfTheMonth First;
    public static DaysOfTheMonth Second;
    public static DaysOfTheMonth Third;
    public static DaysOfTheMonth Fourth;
    public static DaysOfTheMonth Fifth;
    public static DaysOfTheMonth Sixth;
    public static DaysOfTheMonth Seventh;
    public static DaysOfTheMonth Eighth;
    public static DaysOfTheMonth Ninth;
    public static DaysOfTheMonth Tenth;
    public static DaysOfTheMonth Eleventh;
    public static DaysOfTheMonth Twelfth;
    public static DaysOfTheMonth Thirteenth;
    public static DaysOfTheMonth Fourteenth;
    public static DaysOfTheMonth Fifteenth;
    public static DaysOfTheMonth Sixteenth;
    public static DaysOfTheMonth Seventeenth;
    public static DaysOfTheMonth Eighteenth;
    public static DaysOfTheMonth Nineteenth;
    public static DaysOfTheMonth Twentieth;
    public static DaysOfTheMonth TwentyFirst;
    public static DaysOfTheMonth TwentySecond;
    public static DaysOfTheMonth TwentyThird;
    public static DaysOfTheMonth TwentyFourth;
    public static DaysOfTheMonth TwentyFifth;
    public static DaysOfTheMonth TwentySixth;
    public static DaysOfTheMonth TwentySeventh;
    public static DaysOfTheMonth TwentyEighth;
    public static DaysOfTheMonth TwentyNinth;
    public static DaysOfTheMonth Thirtieth;
    public static DaysOfTheMonth ThirtyFirst;

    #endregion

    #region Constructor

    private DaysOfTheMonth(byte value) : base(value)
    { }

    static DaysOfTheMonth()
    {
        Empty = new DaysOfTheMonth(0);
        First = new DaysOfTheMonth(1);
        Second = new DaysOfTheMonth(2);
        Third = new DaysOfTheMonth(3);
        Fourth = new DaysOfTheMonth(4);
        Fifth = new DaysOfTheMonth(5);
        Sixth = new DaysOfTheMonth(6);
        Seventh = new DaysOfTheMonth(7);
        Eighth = new DaysOfTheMonth(8);
        Ninth = new DaysOfTheMonth(9);
        Tenth = new DaysOfTheMonth(10);
        Eleventh = new DaysOfTheMonth(11);
        Twelfth = new DaysOfTheMonth(12);
        Thirteenth = new DaysOfTheMonth(13);
        Fourteenth = new DaysOfTheMonth(14);
        Fifteenth = new DaysOfTheMonth(15);
        Sixteenth = new DaysOfTheMonth(16);
        Seventeenth = new DaysOfTheMonth(17);
        Eighteenth = new DaysOfTheMonth(18);
        Nineteenth = new DaysOfTheMonth(19);
        Twentieth = new DaysOfTheMonth(20);
        TwentyFirst = new DaysOfTheMonth(21);
        TwentySecond = new DaysOfTheMonth(22);
        TwentyThird = new DaysOfTheMonth(23);
        TwentyFourth = new DaysOfTheMonth(24);
        TwentyFifth = new DaysOfTheMonth(25);
        TwentySixth = new DaysOfTheMonth(26);
        TwentySeventh = new DaysOfTheMonth(27);
        TwentyEighth = new DaysOfTheMonth(28);
        TwentyNinth = new DaysOfTheMonth(29);
        Thirtieth = new DaysOfTheMonth(30);
        ThirtyFirst = new DaysOfTheMonth(31);

        // ...
        _ValueObjectMap = new Dictionary<byte, DaysOfTheMonth>
        {
            {First, First},
            {Second, Second},
            {Third, Third},
            {Fourth, Fourth},
            {Fifth, Fifth},
            {Sixth, Sixth},
            {Seventh, Seventh},
            {Eighth, Eighth},
            {Ninth, Ninth},
            {Tenth, Tenth},
            {Eleventh, Eleventh},
            {Twelfth, Twelfth},
            {Thirteenth, Thirteenth},
            {Fourteenth, Fourteenth},
            {Fifteenth, Fifteenth},
            {Sixteenth, Sixteenth},
            {Seventeenth, Seventeenth},
            {Eighteenth, Eighteenth},
            {Nineteenth, Nineteenth},
            {Twentieth, Twentieth},
            {TwentyFirst, TwentyFirst},
            {TwentySecond, TwentySecond},
            {TwentyThird, TwentyThird},
            {TwentyFourth, TwentyFourth},
            {TwentyFifth, TwentyFifth},
            {TwentySixth, TwentySixth},
            {TwentySeventh, TwentySeventh},
            {TwentyEighth, TwentyEighth},
            {TwentyNinth, TwentyNinth},
            {Thirtieth, Thirtieth},
            {ThirtyFirst, ThirtyFirst}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public DaysOfTheMonth GetMinDayOfTheMonth() => ThirtyFirst;
    public DaysOfTheMonth GetMaxDayOfTheMonth() => ThirtyFirst;
    public override DaysOfTheMonth[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DaysOfTheMonth? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}