namespace Play.Ber.DataObjects;

public interface IDecodeDataElement
{
    public PrimitiveValue Decode(TagLengthValue value);
}