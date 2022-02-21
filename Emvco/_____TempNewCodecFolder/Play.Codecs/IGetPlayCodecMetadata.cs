namespace Play.Codecs;

public interface IGetPlayCodecMetadata
{
    protected static PlayEncodingId GetEncodingId(Type encoder) => new(encoder.FullName);
    public PlayEncodingId GetEncodingId();
}