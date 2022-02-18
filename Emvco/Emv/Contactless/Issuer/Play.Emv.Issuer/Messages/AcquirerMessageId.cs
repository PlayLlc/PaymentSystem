using System;

using Play.Codecs;

namespace Play.Emv.Issuer.Messages;

public record struct AcquirerMessageId
{
    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public AcquirerMessageId(int value)
    {
        _Value = value;
    }

    public AcquirerMessageId(ReadOnlySpan<char> value)
    {
        _Value = PlayEncoding.SignedInteger.GetInt32(PlayEncoding.Unicode.GetBytes(value));
    }

    #endregion

    #region Operator Overrides

    public static explicit operator int(AcquirerMessageId value) => value._Value;

    #endregion
}