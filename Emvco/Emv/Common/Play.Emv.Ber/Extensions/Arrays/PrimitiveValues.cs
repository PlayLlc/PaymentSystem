using Play.Ber.DataObjects;

namespace Play.Emv.Ber.Extensions.Arrays;

public static partial class PrimitiveValues
{
    #region Instance Members

    public static byte[] Encode(this PrimitiveValue[] value)
    {
        return value.SelectMany(a => a.EncodeValue(EmvCodec.GetCodec())).ToArray();
    }

    #endregion
}