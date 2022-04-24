using Play.Ber.DataObjects;

namespace Play.Emv.Ber.DataElements;

public interface IEncodeDataElement : IEncodeBerDataObjects
{
    #region Serialization

    public byte[] EncodeValue();
    public byte[] EncodeTagLengthValue();

    #endregion
}