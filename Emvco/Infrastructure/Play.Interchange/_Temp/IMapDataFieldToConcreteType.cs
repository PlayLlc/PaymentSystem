namespace Play.Interchange.Messages.DataFields;

public interface IMapDataFieldToConcreteType
{
    public T Decode<T>(ReadOnlySpan<byte> value);
}