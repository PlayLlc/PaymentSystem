using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Core;

namespace Play.Globalization.Time;

public record DaysOfTheWeek : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, DaysOfTheWeek> _ValueObjectMap;
    public static DaysOfTheWeek Empty;
    public static DaysOfTheWeek Sunday;
    public static DaysOfTheWeek Monday;
    public static DaysOfTheWeek Tuesday;
    public static DaysOfTheWeek Wednesday;
    public static DaysOfTheWeek Thursday;
    public static DaysOfTheWeek Friday;
    public static DaysOfTheWeek Saturday;

    #endregion

    #region Constructor

    private DaysOfTheWeek(byte value) : base(value)
    { }

    static DaysOfTheWeek()
    {
        Empty = new(0);
        Sunday = new(1);
        Monday = new(2);
        Tuesday = new(3);
        Wednesday = new(4);
        Thursday = new(5);
        Friday = new(6);
        Saturday = new(7);

        // ...
        _ValueObjectMap = new Dictionary<byte, DaysOfTheWeek>
        {
            {Sunday, Sunday},
            {Monday, Monday},
            {Tuesday, Tuesday},
            {Wednesday, Wednesday},
            {Thursday, Thursday},
            {Friday, Friday},
            {Saturday, Saturday}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override DaysOfTheWeek[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DaysOfTheWeek? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}