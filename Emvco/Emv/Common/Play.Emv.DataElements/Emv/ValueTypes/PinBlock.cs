﻿using Play.Codecs;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     The encrypted PIN Block encoded as specified in EMV Book 3 Table 24
/// </summary>
public readonly struct PinBlock
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public PinBlock(ReadOnlySpan<byte> value)
    {
        _Value = PlayEncoding.UnsignedIntegerCodec.GetUInt64(value);
    }

    public PinBlock(ulong value)
    {
        _Value = value;
    }

    #endregion
}