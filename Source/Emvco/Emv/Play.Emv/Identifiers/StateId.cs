using System;

using Play.Codecs;

namespace Play.Emv.Identifiers;

public readonly record struct StateId
{
    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    public StateId(ReadOnlySpan<char> value)
    {
        _Value = PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(PlayCodec.UnicodeCodec.Encode(value));
    }

    #endregion

    #region Operator Overrides

    public static explicit operator uint(StateId value) => value._Value;

    #endregion
}