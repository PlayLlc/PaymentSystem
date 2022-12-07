using Play.Ber.DataObjects;

namespace Play.Ber.Codecs;

public partial class BerCodec
{
    #region Serialization

    private byte[] EncodeValue(SetOf value) => value.EncodeValue(this);

    #endregion
}