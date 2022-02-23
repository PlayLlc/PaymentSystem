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
        _Value = PlayCodec.SignedIntegerCodec.DecodeToInt32(PlayCodec.UnicodeCodec.Encode(value));
    }

    #endregion

    #region Operator Overrides

    public static explicit operator int(Identity value) => value._Value;

    #endregion
}