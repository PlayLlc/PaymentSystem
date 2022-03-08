using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;

namespace Play.Ber.Codecs;

public partial class BerCodec
{
    #region Instance Members

    /// <summary>
    ///     AsSetOf
    /// </summary>
    /// <param name="value"></param>
    /// <param name="berCodec"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public byte[] AsSetOf<T>(SetOf<T> value, BerCodec berCodec) where T : PrimitiveValue
    {
        List<byte> buffer = new();

        for (int i = 0; i < value.Count; i++)
            buffer.AddRange(EncodeTagLengthValue(value[i]));

        return buffer.ToArray();
    }

    #endregion

    #region Serialization

    private byte[] EncodeValue(SetOf value) => value.EncodeValue(this);

    #endregion
}