namespace Play.Interchange.Codecs.Dynamic;

public interface IMapDataFieldToConcreteType
{
    #region Serialization

    public T Decode<T>(ReadOnlySpan<byte> value);

    #endregion
}