using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs;
using Play.Codecs.Integers;
using Play.Core.Specifications;
using Play.Emv.Ber;

namespace Play.Emv.DataElements.ValueTypes.ValueTypes;

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
        _Value = PlayEncoding.UnsignedInteger.GetUInt64(value);
    }

    public PinBlock(ulong value)
    {
        _Value = value;
    }

    #endregion
}