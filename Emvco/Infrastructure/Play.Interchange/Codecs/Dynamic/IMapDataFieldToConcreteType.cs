namespace Play.Interchange.Codecs.Dynamic;

public interface IMapDataFieldToConcreteType
{
    public T Decode<T>(ReadOnlySpan<byte> value);
}