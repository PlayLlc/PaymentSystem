using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Core;
using Play.Core.Extensions;

namespace Play.Icc.Messaging.Apdu;

/// <summary>
///     The goal of secure messaging (SM) is to protect [part of] the messages to and from a card by
///     ensuring two basic security functions: data authentication and data confidentiality.
///     Secure messaging is achieved by applying one or more security mechanisms. Each security mechanism
///     involves an algorithm, a key, an argument and often, initial data.
/// </summary>
/// <remarks>
///     <see cref="ISO7816.Part4" /> Section 5.4.1 Table 9/>
/// </remarks>
public record SecureMessaging : EnumObject<byte>
{
    #region Static Metadata

    public static readonly SecureMessaging Empty = new();
    private static readonly ImmutableSortedDictionary<byte, SecureMessaging> _ValueObjectMap;

    /// <value>Decimal: 12; Hexadecimal: 0xC</value>
    public static readonly SecureMessaging Authenticated;

    /// <value>Decimal: 8; Hexadecimal: 0x8</value>
    public static readonly SecureMessaging NotAuthenticated;

    /// <value>Decimal: 0; Hexadecimal: 0x0</value>
    public static readonly SecureMessaging NotRecognized;

    /// <value>Decimal: 4; Hexadecimal: 0x4</value>
    public static readonly SecureMessaging Proprietary;

    #endregion

    #region Constructor

    public SecureMessaging()
    { }

    static SecureMessaging()
    {
        const byte notRecognized = 0;
        const byte proprietary = (byte) Bits.Three;
        const byte notAuthenticated = (byte) Bits.Four;
        const byte authenticated = 0b1100;

        NotRecognized = new SecureMessaging(notRecognized);
        Proprietary = new SecureMessaging(proprietary);
        NotAuthenticated = new SecureMessaging(notAuthenticated);
        Authenticated = new SecureMessaging(authenticated);

        _ValueObjectMap = new Dictionary<byte, SecureMessaging>
        {
            {notRecognized, NotRecognized}, {proprietary, Proprietary}, {notAuthenticated, NotAuthenticated}, {authenticated, Authenticated}
        }.ToImmutableSortedDictionary();
    }

    private SecureMessaging(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static SecureMessaging Get(byte value)
    {
        const byte secureMessagingMask = (byte) (Bits.Eight | Bits.Seven | Bits.Six | Bits.Five | Bits.Two | Bits.One);

        return _ValueObjectMap[value.GetMaskedValue(secureMessagingMask)];
    }

    public static bool IsValid(byte value) => _ValueObjectMap.ContainsKey(value);
    public override SecureMessaging[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out SecureMessaging? innerResult))
        {
            result = innerResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Equality

    public bool Equals(SecureMessaging x, SecureMessaging y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 10544431;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(SecureMessaging left, byte right) => left._Value == right;
    public static bool operator ==(byte left, SecureMessaging right) => left == right._Value;

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information
    public static explicit operator sbyte(SecureMessaging value) => (sbyte) value._Value;
    public static explicit operator short(SecureMessaging value) => value._Value;
    public static explicit operator ushort(SecureMessaging value) => value._Value;
    public static explicit operator int(SecureMessaging value) => value._Value;
    public static explicit operator uint(SecureMessaging value) => value._Value;
    public static explicit operator long(SecureMessaging value) => value._Value;
    public static explicit operator ulong(SecureMessaging value) => value._Value;
    public static implicit operator byte(SecureMessaging value) => value._Value;
    public static bool operator !=(SecureMessaging left, byte right) => !(left == right);
    public static bool operator !=(byte left, SecureMessaging right) => !(left == right);

    #endregion
}