﻿using Play.Codecs;

namespace Play.Interchange.Messages.Header;

/// <summary>
///     The message type indicator is a four-digit numeric field which indicates the overall function of the message. A
///     message type indicator includes the ISO 8583 version, the Message Class, the Message Function and the Message
///     Origin, as described below.
/// </summary>
public readonly struct MessageTypeIndicator
{
    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public MessageTypeIndicator(ushort value)
    {
        _Value = value;
    }

    public MessageTypeIndicator(Version version, Class @class, Function function, Origin origin)
    {
        byte result = (byte) (version * 1000);
        result |= (byte) (@class * 100);
        result |= (byte) (function * 10);
        result |= origin;
        _Value = result;
    }

    #endregion

    #region Instance Members

    internal void CopyTo(Span<byte> buffer)
    {
        buffer[0] = (byte) (_Value >> 8);
        buffer[1] = (byte) _Value;
    }

    public override string ToString() => PlayEncoding.Numeric.GetString(PlayEncoding.Numeric.GetBytes(_Value));
    public byte GetFirstByte() => (byte) (_Value >> 8);
    public byte GetSecondByte() => (byte) _Value;

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(MessageTypeIndicator value) => value._Value;
    public static implicit operator MessageTypeIndicator(ushort value) => new(value);

    #endregion
}