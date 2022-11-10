using System;
using System.Numerics;

using Play.Codecs;

namespace Play.Emv.Identifiers;

public readonly record struct StateId
{
    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public StateId(ReadOnlySpan<char> value)
    {
        _Value = PlayCodec.UnsignedIntegerCodec.DecodeToBigInteger(PlayCodec.UnicodeCodec.Encode(value));
    }

    #endregion

    #region Operator Overrides

    public static explicit operator BigInteger(StateId value) => value._Value;

    #endregion
}