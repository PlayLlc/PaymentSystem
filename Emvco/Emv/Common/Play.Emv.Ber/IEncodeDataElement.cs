using Play.Ber.DataObjects;

namespace Play.Emv.DataElements;

public interface IEncodeDataElement : IEncodeBerDataObjects
{
    public byte[] EncodeValue();
    public byte[] EncodeTagLengthValue();
}