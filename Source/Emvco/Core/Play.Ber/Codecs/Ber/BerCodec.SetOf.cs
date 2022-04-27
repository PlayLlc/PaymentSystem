using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;

namespace Play.Ber.Codecs;

public partial class BerCodec
{
    #region Serialization

    private byte[] EncodeValue(SetOf value) => value.EncodeValue(this);

    #endregion
}