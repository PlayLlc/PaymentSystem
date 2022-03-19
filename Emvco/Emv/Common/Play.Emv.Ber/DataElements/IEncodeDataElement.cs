using Play.Ber.DataObjects;

namespace Play.Emv.Ber.DataElements;

public interface IEncodeDataElement : IEncodeBerDataObjects
{
    public byte[] EncodeValue();
    public byte[] EncodeTagLengthValue();
}