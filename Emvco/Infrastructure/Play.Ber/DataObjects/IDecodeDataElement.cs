using Play.Ber.DataObjects;

namespace Play.Emv.Ber.DataElements;

public interface IDecodeDataElement
{
    public PrimitiveValue Decode(TagLengthValue value);
}