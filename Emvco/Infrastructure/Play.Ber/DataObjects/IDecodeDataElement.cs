namespace Play.Ber.DataObjects;

public interface IDecodeDataElement
{
    #region Serialization

    public PrimitiveValue Decode(TagLengthValue value);

    #endregion
}