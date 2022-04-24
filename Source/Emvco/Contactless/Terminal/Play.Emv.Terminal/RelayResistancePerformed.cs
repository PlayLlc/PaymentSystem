using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core.Extensions;

namespace Play.Emv.Terminal;

public readonly struct RelayResistancePerformed
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, RelayResistancePerformed> _ValueObjectMap;
    public static readonly RelayResistancePerformed RelayResistanceProtocolNotPerformed;
    public static readonly RelayResistancePerformed RelayResistanceProtocolNotSupported;
    public static readonly RelayResistancePerformed RelayResistanceProtocolPerformed;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static RelayResistancePerformed()
    {
        const byte relayResistanceProtocolNotPerformed = 1;
        const byte relayResistanceProtocolNotSupported = 0;
        const byte relayResistanceProtocolPerformed = 2;

        RelayResistanceProtocolNotPerformed = new RelayResistancePerformed(relayResistanceProtocolNotPerformed);
        RelayResistanceProtocolNotSupported = new RelayResistancePerformed(relayResistanceProtocolNotSupported);
        RelayResistanceProtocolPerformed = new RelayResistancePerformed(relayResistanceProtocolPerformed);
        _ValueObjectMap = new Dictionary<byte, RelayResistancePerformed>
        {
            {relayResistanceProtocolNotPerformed, RelayResistanceProtocolNotPerformed},
            {relayResistanceProtocolNotSupported, RelayResistanceProtocolNotSupported},
            {relayResistanceProtocolPerformed, RelayResistanceProtocolPerformed}
        }.ToImmutableSortedDictionary();
    }

    private RelayResistancePerformed(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static RelayResistancePerformed Get(byte value)
    {
        const byte bitMask = 0b11111100;

        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(RelayResistancePerformed)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) =>
        obj is RelayResistancePerformed relayResistancePerformed && Equals(relayResistancePerformed);

    public bool Equals(RelayResistancePerformed other) => _Value == other._Value;
    public bool Equals(RelayResistancePerformed x, RelayResistancePerformed y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 580561;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(RelayResistancePerformed left, RelayResistancePerformed right) => left._Value == right._Value;
    public static bool operator ==(RelayResistancePerformed left, byte right) => left._Value == right;
    public static bool operator ==(byte left, RelayResistancePerformed right) => left == right._Value;
    public static explicit operator byte(RelayResistancePerformed value) => value._Value;
    public static explicit operator short(RelayResistancePerformed value) => value._Value;
    public static explicit operator ushort(RelayResistancePerformed value) => value._Value;
    public static explicit operator int(RelayResistancePerformed value) => value._Value;
    public static explicit operator uint(RelayResistancePerformed value) => value._Value;
    public static explicit operator long(RelayResistancePerformed value) => value._Value;
    public static explicit operator ulong(RelayResistancePerformed value) => value._Value;
    public static bool operator !=(RelayResistancePerformed left, RelayResistancePerformed right) => !(left == right);
    public static bool operator !=(RelayResistancePerformed left, byte right) => !(left == right);
    public static bool operator !=(byte left, RelayResistancePerformed right) => !(left == right);

    #endregion
}