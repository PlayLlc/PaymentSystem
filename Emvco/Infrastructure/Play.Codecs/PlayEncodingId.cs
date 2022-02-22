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
        _Value = PlayCodec.SignedIntegerCodec.GetInt32(PlayCodec.UnicodeCodec.Encode(value));
    }

    public PlayEncodingId(Type value)
    {
        _Value = PlayCodec.SignedIntegerCodec.GetInt32(PlayCodec.UnicodeCodec.Encode(value.FullName));
    }

    #endregion

    #region Operator Overrides

    public static explicit operator int(PlayEncodingId value) => value._Value;

    #endregion
}