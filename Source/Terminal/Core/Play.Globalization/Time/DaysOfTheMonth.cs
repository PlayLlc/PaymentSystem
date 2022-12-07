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
        Empty = new(0);
        First = new(1);
        Second = new(2);
        Third = new(3);
        Fourth = new(4);
        Fifth = new(5);
        Sixth = new(6);
        Seventh = new(7);
        Eighth = new(8);
        Ninth = new(9);
        Tenth = new(10);
        Eleventh = new(11);
        Twelfth = new(12);
        Thirteenth = new(13);
        Fourteenth = new(14);
        Fifteenth = new(15);
        Sixteenth = new(16);
        Seventeenth = new(17);
        Eighteenth = new(18);
        Nineteenth = new(19);
        Twentieth = new(20);
        TwentyFirst = new(21);
        TwentySecond = new(22);
        TwentyThird = new(23);
        TwentyFourth = new(24);
        TwentyFifth = new(25);
        TwentySixth = new(26);
        TwentySeventh = new(27);
        TwentyEighth = new(28);
        TwentyNinth = new(29);
        Thirtieth = new(30);
        ThirtyFirst = new(31);

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