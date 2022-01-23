using System.Collections.Immutable;

namespace Play.Icc.Emv;

public readonly struct Level3Error
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Level3Error> _ValueObjectMap;
    public static readonly Level3Error AmountNotPresent;
    public static readonly Level3Error Ok;
    public static readonly Level3Error Stop;
    public static readonly Level3Error TimeOut;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static Level3Error()
    {
        const byte amountNotPresent = 3;
        const byte ok = 0;
        const byte stop = 2;
        const byte timeOut = 1;

        AmountNotPresent = new Level3Error(amountNotPresent);
        Ok = new Level3Error(ok);
        Stop = new Level3Error(stop);
        TimeOut = new Level3Error(timeOut);
        _ValueObjectMap =
            new Dictionary<byte, Level3Error> {{amountNotPresent, AmountNotPresent}, {ok, Ok}, {stop, Stop}, {timeOut, TimeOut}}
                .ToImmutableSortedDictionary();
    }

    private Level3Error(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static Level3Error Get(byte value)
    {
        return _ValueObjectMap[value];
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj)
    {
        return obj is Level3Error l3 && Equals(l3);
    }

    public bool Equals(Level3Error other)
    {
        return _Value == other._Value;
    }

    public bool Equals(Level3Error x, Level3Error y)
    {
        return x.Equals(y);
    }

    public bool Equals(byte other)
    {
        return _Value == other;
    }

    public override int GetHashCode()
    {
        const int hash = 138113;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Level3Error left, Level3Error right)
    {
        return left._Value == right._Value;
    }

    public static bool operator ==(Level3Error left, byte right)
    {
        return left._Value == right;
    }

    public static bool operator ==(byte left, Level3Error right)
    {
        return left == right._Value;
    }

    public static explicit operator byte(Level3Error value)
    {
        return value._Value;
    }

    public static explicit operator short(Level3Error value)
    {
        return value._Value;
    }

    public static explicit operator ushort(Level3Error value)
    {
        return value._Value;
    }

    public static explicit operator int(Level3Error value)
    {
        return value._Value;
    }

    public static explicit operator uint(Level3Error value)
    {
        return value._Value;
    }

    public static explicit operator long(Level3Error value)
    {
        return value._Value;
    }

    public static explicit operator ulong(Level3Error value)
    {
        return value._Value;
    }

    public static bool operator !=(Level3Error left, Level3Error right)
    {
        return !(left == right);
    }

    public static bool operator !=(Level3Error left, byte right)
    {
        return !(left == right);
    }

    public static bool operator !=(byte left, Level3Error right)
    {
        return !(left == right);
    }

    #endregion
}