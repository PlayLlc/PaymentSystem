using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;

namespace Play.Emv.Ber.Extensions.Arrays;

public static partial class PrimitiveValues
{
    #region Instance Members

    public static byte[] Encode(this PrimitiveValue[] value)
    {
        return value.SelectMany(a => a.EncodeValue(EmvCodec.GetBerCodec())).ToArray();
    }

    #endregion
}