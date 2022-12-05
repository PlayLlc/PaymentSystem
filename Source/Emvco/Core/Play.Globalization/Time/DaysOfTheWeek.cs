using System;
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
        Empty = new DaysOfTheWeek(0);
        Sunday = new DaysOfTheWeek(1);
        Monday = new DaysOfTheWeek(2);
        Tuesday = new DaysOfTheWeek(3);
        Wednesday = new DaysOfTheWeek(4);
        Thursday = new DaysOfTheWeek(5);
        Friday = new DaysOfTheWeek(6);
        Saturday = new DaysOfTheWeek(7);

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

    #region Operator Overrides

    public static implicit operator DayOfWeek(DaysOfTheWeek enumObject) => (DayOfWeek) enumObject._Value;

    #endregion
}