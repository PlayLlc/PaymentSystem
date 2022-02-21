using System;

using Play.Codecs;

namespace Play.Emv.Identifiers;

public record struct Identity
{
    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public Identity(int value)
    {
        _Value = value;
    }

    public Identity(ReadOnlySpan<char> value)
    {
        _Value = PlayEncoding.SignedInteger.GetInt32(PlayEncoding.Unicode.GetBytes(value));
    }

    #endregion

    #region Operator Overrides

    public static explicit operator int(Identity value) => value._Value;

    #endregion
}