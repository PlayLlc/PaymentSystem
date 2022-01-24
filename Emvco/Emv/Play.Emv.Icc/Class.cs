using System.Diagnostics.CodeAnalysis;

using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc;

/// <summary>
///     the class byte CLA of a command is used to indicate to what extent the command and the response comply with ISO/IEC
///     7816-4 and when applicable, the format of secure messaging and the logical channel number.
/// </summary>
internal readonly struct Class
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public Class(ProprietaryMessageIdentifier proprietary, SecureMessaging secureMessaging, LogicalChannel logicalChannel)
    {
        _Value = (byte) (proprietary | secureMessaging | logicalChannel);
    }

    public Class(ProprietaryMessageIdentifier proprietary, SecureMessaging secureMessaging)
    {
        _Value = (byte) (proprietary | secureMessaging | LogicalChannel.BasicChannel);
    }

    public Class(ProprietaryMessageIdentifier proprietary)
    {
        _Value = (byte) (proprietary | SecureMessaging.NotRecognized | LogicalChannel.BasicChannel);
    }

    public Class(SecureMessaging secureMessaging)
    {
        _Value = (byte) (secureMessaging | LogicalChannel.BasicChannel);
    }

    public Class(SecureMessaging secureMessaging, LogicalChannel logicalChannel)
    {
        _Value = (byte) (secureMessaging | logicalChannel);
    }

    #endregion

    #region Instance Members

    public LogicalChannel GetLogicalChannel() => LogicalChannel.Get(_Value);
    public SecureMessaging GetSecureMessagingType() => SecureMessaging.Get(_Value);

    #endregion

    #region Equality

    public override bool Equals([AllowNull] object obj) => obj is Class @class && Equals(@class);
    public bool Equals(Class other) => _Value == other._Value;
    public bool Equals(Class x, Class y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 658379;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Class left, Class right) => left._Value == right._Value;
    public static bool operator ==(Class left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Class right) => left == right._Value;

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information
    public static explicit operator sbyte(Class value) => (sbyte) value._Value;
    public static explicit operator short(Class value) => value._Value;
    public static explicit operator ushort(Class value) => value._Value;
    public static explicit operator int(Class value) => value._Value;
    public static explicit operator uint(Class value) => value._Value;
    public static explicit operator long(Class value) => value._Value;
    public static explicit operator ulong(Class value) => value._Value;
    public static implicit operator byte(Class value) => value._Value;
    public static bool operator !=(Class left, Class right) => !(left == right);
    public static bool operator !=(Class left, byte right) => !(left == right);
    public static bool operator !=(byte left, Class right) => !(left == right);

    #endregion
}