namespace Play.Interchange.Messages.DataFields.ValueObjects;

public interface IMapDataFieldToConcreteType
{
    public T Decode<T>(ReadOnlySpan<byte> value);
}