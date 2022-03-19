using System.Collections.Immutable;

using Play.Core.Extensions;

namespace Play.Emv.Ber;

public readonly struct AlternateInterfacePreferenceOutcome
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, AlternateInterfacePreferenceOutcome> _ValueObjectMap;
    public static readonly AlternateInterfacePreferenceOutcome NotAvailable;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static AlternateInterfacePreferenceOutcome()
    {
        const byte notAvailable = 15;

        NotAvailable = new AlternateInterfacePreferenceOutcome(notAvailable);
        _ValueObjectMap = new Dictionary<byte, AlternateInterfacePreferenceOutcome> {{notAvailable, NotAvailable}}
            .ToImmutableSortedDictionary();
    }

    private AlternateInterfacePreferenceOutcome(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static AlternateInterfacePreferenceOutcome Get(byte value)
    {
        const byte bitMask = 0b11110000;

        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(AlternateInterfacePreferenceOutcome)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) =>
        obj is AlternateInterfacePreferenceOutcome outcomeParameterSetAlternateInterfacePreference
        && Equals(outcomeParameterSetAlternateInterfacePreference);

    public bool Equals(AlternateInterfacePreferenceOutcome other) => _Value == other._Value;
    public bool Equals(AlternateInterfacePreferenceOutcome x, AlternateInterfacePreferenceOutcome y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 312701;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(AlternateInterfacePreferenceOutcome left, AlternateInterfacePreferenceOutcome right) =>
        left._Value == right._Value;

    public static bool operator ==(AlternateInterfacePreferenceOutcome left, byte right) => left._Value == right;
    public static bool operator ==(byte left, AlternateInterfacePreferenceOutcome right) => left == right._Value;
    public static explicit operator byte(AlternateInterfacePreferenceOutcome value) => value._Value;
    public static explicit operator short(AlternateInterfacePreferenceOutcome value) => value._Value;
    public static explicit operator ushort(AlternateInterfacePreferenceOutcome value) => value._Value;
    public static explicit operator int(AlternateInterfacePreferenceOutcome value) => value._Value;
    public static explicit operator uint(AlternateInterfacePreferenceOutcome value) => value._Value;
    public static explicit operator long(AlternateInterfacePreferenceOutcome value) => value._Value;
    public static explicit operator ulong(AlternateInterfacePreferenceOutcome value) => value._Value;
    public static bool operator !=(AlternateInterfacePreferenceOutcome left, AlternateInterfacePreferenceOutcome right) => !(left == right);
    public static bool operator !=(AlternateInterfacePreferenceOutcome left, byte right) => !(left == right);
    public static bool operator !=(byte left, AlternateInterfacePreferenceOutcome right) => !(left == right);

    #endregion
}