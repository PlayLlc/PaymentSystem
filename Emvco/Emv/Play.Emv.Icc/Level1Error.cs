using System.Collections.Immutable;

namespace Play.Emv.Icc;

public readonly struct Level1Error
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Level1Error> _ValueObjectMap;
    public static readonly Level1Error Ok;
    public static readonly Level1Error ProtocolError;
    public static readonly Level1Error TimeOutError;
    public static readonly Level1Error TransmissionError;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static Level1Error()
    {
        const byte ok = 0;
        const byte protocolError = 3;
        const byte timeOutError = 1;
        const byte transmissionError = 2;

        Ok = new Level1Error(ok);
        ProtocolError = new Level1Error(protocolError);
        TimeOutError = new Level1Error(timeOutError);
        TransmissionError = new Level1Error(transmissionError);
        _ValueObjectMap = new Dictionary<byte, Level1Error>
        {
            {ok, Ok}, {protocolError, ProtocolError}, {timeOutError, TimeOutError}, {transmissionError, TransmissionError}
        }.ToImmutableSortedDictionary();
    }

    private Level1Error(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static Level1Error Get(byte value) => _ValueObjectMap[value];

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is Level1Error l1 && Equals(l1);
    public bool Equals(Level1Error other) => _Value == other._Value;
    public bool Equals(Level1Error x, Level1Error y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 601423;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Level1Error left, Level1Error right) => left._Value == right._Value;
    public static bool operator ==(Level1Error left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Level1Error right) => left == right._Value;
    public static explicit operator byte(Level1Error value) => value._Value;
    public static explicit operator short(Level1Error value) => value._Value;
    public static explicit operator ushort(Level1Error value) => value._Value;
    public static explicit operator int(Level1Error value) => value._Value;
    public static explicit operator uint(Level1Error value) => value._Value;
    public static explicit operator long(Level1Error value) => value._Value;
    public static explicit operator ulong(Level1Error value) => value._Value;
    public static bool operator !=(Level1Error left, Level1Error right) => !(left == right);
    public static bool operator !=(Level1Error left, byte right) => !(left == right);
    public static bool operator !=(byte left, Level1Error right) => !(left == right);

    #endregion
}