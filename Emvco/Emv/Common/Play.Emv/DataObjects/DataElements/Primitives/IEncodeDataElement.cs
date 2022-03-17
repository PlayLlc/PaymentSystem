using Play.Ber.DataObjects;

namespace Play.Emv.Ber.DataObjects;

public interface IEncodeDataElement : IEncodeBerDataObjects
{
    public byte[] EncodeValue();
    public byte[] EncodeTagLengthValue();
}