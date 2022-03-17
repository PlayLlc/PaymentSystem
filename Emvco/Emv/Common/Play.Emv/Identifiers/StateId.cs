using System;

using Play.Codecs;

namespace Play.Emv.Sessions;

public readonly record struct StateId
{
    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public StateId(ReadOnlySpan<char> value)
    {
        _Value = PlayCodec.SignedIntegerCodec.DecodeToInt32(PlayCodec.UnicodeCodec.Encode(value));
    }

    #endregion

    #region Operator Overrides

    public static explicit operator int(StateId value) => value._Value;

    #endregion
}