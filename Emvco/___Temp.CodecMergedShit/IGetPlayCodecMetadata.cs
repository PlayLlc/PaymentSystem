namespace ___Temp.CodecMergedShit;

public interface IGetPlayCodecMetadata
{
    protected static PlayEncodingId GetEncodingId(Type encoder) => new(encoder.FullName);
    public PlayEncodingId GetEncodingId();
}