using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Core;

namespace Play.Globalization.Time;

public record Months : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Months> _ValueObjectMap;
    public static Months Empty;
    public static Months January;
    public static Months February;
    public static Months March;
    public static Months April;
    public static Months May;
    public static Months June;
    public static Months July;
    public static Months August;
    public static Months September;
    public static Months October;
    public static Months November;
    public static Months December;

    #endregion

    #region Constructor

    private Months(byte value) : base(value)
    { }

    static Months()
    {
        Empty = new Months(0);
        January = new Months(1);
        February = new Months(2);
        March = new Months(3);
        April = new Months(4);
        May = new Months(5);
        June = new Months(6);
        July = new Months(7);
        August = new Months(8);
        September = new Months(9);
        October = new Months(10);
        November = new Months(11);
        December = new Months(12);

        // ...
        _ValueObjectMap = new Dictionary<byte, Months>
        {
            {January, January},
            {February, February},
            {March, March},
            {April, April},
            {June, June},
            {July, July},
            {August, August},
            {September, September},
            {October, October},
            {November, November},
            {December, December}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override Months[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out Months? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}