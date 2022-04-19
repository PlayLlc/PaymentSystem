using Play.Core.Extensions;

namespace Play.Emv.Ber;

public readonly record struct ApplicationPriorityRank
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public ApplicationPriorityRank(byte value)
    {
        // Applications can be ranked between 1 and 15
        _Value = value > 15 ? value.GetMaskedValue(0b11110000) : value;
    }

    #endregion

    #region Equality

    public bool Equals(ApplicationPriorityRank other) => _Value == other._Value;
    public bool Equals(ApplicationPriorityRank x, ApplicationPriorityRank y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 297581;

        return hash + _Value.GetHashCode();
    }

    public int CompareTo(ApplicationPriorityRank other) => _Value.CompareTo(other._Value);

    #endregion

    #region Operator Overrides

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
    public static bool operator !=(ApplicationPriorityRank left, byte right) => !(left == right);
    public static bool operator !=(byte left, ApplicationPriorityRank right) => !(left == right);
    public static bool operator <(ApplicationPriorityRank left, ApplicationPriorityRank right) => left._Value < right._Value;
    public static bool operator <=(ApplicationPriorityRank left, ApplicationPriorityRank right) => left._Value <= right._Value;

    #endregion
}