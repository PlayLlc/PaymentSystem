using System;

namespace Play.Codecs;

public readonly record struct PlayEncodingId
{
    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public PlayEncodingId(int value)
    {
        _Value = value;
    }

    public PlayEncodingId(ReadOnlySpan<char> value)
    {
        _Value = PlayEncoding.SignedInteger.GetInt32(PlayEncoding.Unicode.GetBytes(value));
    }

    #endregion

    #region Operator Overrides

    public static explicit operator int(PlayEncodingId value) => value._Value;

    #endregion
}