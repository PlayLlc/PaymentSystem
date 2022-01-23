using System.Collections.Immutable;

using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public readonly struct ApplicationPriorityRank
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, ApplicationPriorityRank> _ValueObjectMap;
    public static readonly ApplicationPriorityRank Eighth;
    public static readonly ApplicationPriorityRank Eleventh;
    public static readonly ApplicationPriorityRank Fifteenth;
    public static readonly ApplicationPriorityRank Fifth;
    public static readonly ApplicationPriorityRank First;
    public static readonly ApplicationPriorityRank Fourteenth;
    public static readonly ApplicationPriorityRank Fourth;
    public static readonly ApplicationPriorityRank Ninth;
    public static readonly ApplicationPriorityRank Second;
    public static readonly ApplicationPriorityRank Seventh;
    public static readonly ApplicationPriorityRank Sixth;
    public static readonly ApplicationPriorityRank Tenth;
    public static readonly ApplicationPriorityRank Third;
    public static readonly ApplicationPriorityRank Thirteenth;
    public static readonly ApplicationPriorityRank Twelveth;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static ApplicationPriorityRank()
    {
        const byte first = 1;
        const byte second = 2;
        const byte third = 3;
        const byte fourth = 4;
        const byte fifth = 5;
        const byte sixth = 6;
        const byte seventh = 7;
        const byte eighth = 8;
        const byte ninth = 9;
        const byte tenth = 10;
        const byte eleventh = 11;
        const byte twelveth = 12;
        const byte thirteenth = 13;
        const byte fourteenth = 14;
        const byte fifteenth = 15;

        First = new ApplicationPriorityRank(first);
        Second = new ApplicationPriorityRank(second);
        Third = new ApplicationPriorityRank(third);
        Fourth = new ApplicationPriorityRank(fourth);
        Fifth = new ApplicationPriorityRank(fifth);
        Sixth = new ApplicationPriorityRank(sixth);
        Seventh = new ApplicationPriorityRank(seventh);
        Eighth = new ApplicationPriorityRank(eighth);
        Ninth = new ApplicationPriorityRank(ninth);
        Tenth = new ApplicationPriorityRank(tenth);
        Eleventh = new ApplicationPriorityRank(eleventh);
        Twelveth = new ApplicationPriorityRank(twelveth);
        Thirteenth = new ApplicationPriorityRank(thirteenth);
        Fourteenth = new ApplicationPriorityRank(fourteenth);
        Fifteenth = new ApplicationPriorityRank(fifteenth);
        _ValueObjectMap = new Dictionary<byte, ApplicationPriorityRank>
        {
            {first, First},
            {second, Second},
            {third, Third},
            {fourth, Fourth},
            {fifth, Fifth},
            {sixth, Sixth},
            {seventh, Seventh},
            {eighth, Eighth},
            {ninth, Ninth},
            {tenth, Tenth},
            {eleventh, Eleventh},
            {twelveth, Twelveth},
            {thirteenth, Thirteenth},
            {fourteenth, Fourteenth},
            {fifteenth, Fifteenth}
        }.ToImmutableSortedDictionary();
    }

    private ApplicationPriorityRank(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static ApplicationPriorityRank Get(byte value)
    {
        const byte bitMask = 0b11110000;

        if (!_ValueObjectMap.ContainsKey(value.GetMaskedValue(bitMask)))
            return Fifteenth;

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is ApplicationPriorityRank applicationPriorityRank && Equals(applicationPriorityRank);
    public bool Equals(ApplicationPriorityRank other) => _Value == other._Value;
    public bool Equals(ApplicationPriorityRank x, ApplicationPriorityRank y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 297581;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(ApplicationPriorityRank left, ApplicationPriorityRank right) => left._Value == right._Value;
    public static bool operator ==(ApplicationPriorityRank left, byte right) => left._Value == right;
    public static bool operator ==(byte left, ApplicationPriorityRank right) => left == right._Value;
    public static explicit operator byte(ApplicationPriorityRank value) => value._Value;
    public static explicit operator short(ApplicationPriorityRank value) => value._Value;
    public static explicit operator ushort(ApplicationPriorityRank value) => value._Value;
    public static explicit operator int(ApplicationPriorityRank value) => value._Value;
    public static explicit operator uint(ApplicationPriorityRank value) => value._Value;
    public static explicit operator long(ApplicationPriorityRank value) => value._Value;
    public static explicit operator ulong(ApplicationPriorityRank value) => value._Value;
    public static bool operator >(ApplicationPriorityRank left, ApplicationPriorityRank right) => left._Value > right._Value;
    public static bool operator >=(ApplicationPriorityRank left, ApplicationPriorityRank right) => left._Value >= right._Value;
    public static bool operator !=(ApplicationPriorityRank left, ApplicationPriorityRank right) => !(left == right);
    public static bool operator !=(ApplicationPriorityRank left, byte right) => !(left == right);
    public static bool operator !=(byte left, ApplicationPriorityRank right) => !(left == right);
    public static bool operator <(ApplicationPriorityRank left, ApplicationPriorityRank right) => left._Value < right._Value;
    public static bool operator <=(ApplicationPriorityRank left, ApplicationPriorityRank right) => left._Value <= right._Value;

    #endregion
}