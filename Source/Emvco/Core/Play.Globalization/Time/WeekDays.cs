using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Core;

namespace Play.Globalization.Time;

public record Weekdays : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Weekdays> _ValueObjectMap;
    public static Weekdays Empty;
    public static Weekdays Sunday;
    public static Weekdays Monday;
    public static Weekdays Tuesday;
    public static Weekdays Wednesday;
    public static Weekdays Thursday;
    public static Weekdays Friday;
    public static Weekdays Saturday;

    #endregion

    #region Constructor

    private Weekdays(byte value) : base(value)
    { }

    static Weekdays()
    {
        Empty = new Weekdays(0);
        Sunday = new Weekdays(1);
        Monday = new Weekdays(2);
        Tuesday = new Weekdays(3);
        Wednesday = new Weekdays(4);
        Thursday = new Weekdays(5);
        Friday = new Weekdays(6);
        Saturday = new Weekdays(7);

        // ...
        _ValueObjectMap = new Dictionary<byte, Weekdays>
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

    public override Weekdays[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out Weekdays? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}