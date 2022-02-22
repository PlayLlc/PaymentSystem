using System.Collections.Immutable;

namespace Play.Emv.DataElements.Emv;

public readonly struct OnlineResponseOutcome
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, OnlineResponseOutcome> _ValueObjectMap;

    /// <value>Decimal: 0; HexadecimalCodec: 0x00</value>
    public static readonly OnlineResponseOutcome NotAvailable;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static OnlineResponseOutcome()
    {
        const byte notAvailable = 0;

        NotAvailable = new OnlineResponseOutcome(notAvailable);
        _ValueObjectMap = new Dictionary<byte, OnlineResponseOutcome> {{notAvailable, NotAvailable}}.ToImmutableSortedDictionary();
    }

    private OnlineResponseOutcome(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static OnlineResponseOutcome Get(byte value) => _ValueObjectMap[value];

    #endregion

    #region Equality

    public override bool Equals(object? obj) =>
        obj is OnlineResponseOutcome outcomeParameterSetOnlineResponseData && Equals(outcomeParameterSetOnlineResponseData);

    public bool Equals(OnlineResponseOutcome other) => _Value == other._Value;
    public bool Equals(OnlineResponseOutcome x, OnlineResponseOutcome y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 106297;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(OnlineResponseOutcome left, OnlineResponseOutcome right) => left._Value == right._Value;
    public static bool operator ==(OnlineResponseOutcome left, byte right) => left._Value == right;
    public static bool operator ==(byte left, OnlineResponseOutcome right) => left == right._Value;
    public static explicit operator byte(OnlineResponseOutcome value) => value._Value;
    public static explicit operator short(OnlineResponseOutcome value) => value._Value;
    public static explicit operator ushort(OnlineResponseOutcome value) => value._Value;
    public static explicit operator int(OnlineResponseOutcome value) => value._Value;
    public static explicit operator uint(OnlineResponseOutcome value) => value._Value;
    public static explicit operator long(OnlineResponseOutcome value) => value._Value;
    public static explicit operator ulong(OnlineResponseOutcome value) => value._Value;
    public static bool operator !=(OnlineResponseOutcome left, OnlineResponseOutcome right) => !(left == right);
    public static bool operator !=(OnlineResponseOutcome left, byte right) => !(left == right);
    public static bool operator !=(byte left, OnlineResponseOutcome right) => !(left == right);

    #endregion
}