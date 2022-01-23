using Play.Ber.DataObjects;

namespace Play.Ber.Emv.DataObjects;

public interface IEncodeDataElement : IEncodeBerDataObjects
{
    public byte[] EncodeValue();
    public byte[] EncodeTagLengthValue();
}