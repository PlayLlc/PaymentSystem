namespace Play.Codecs;

public interface IGetPlayCodecMetadata
{
    #region Instance Members

    protected static PlayEncodingId GetEncodingId(Type encoder) => new(encoder);
    public PlayEncodingId GetEncodingId();

    #endregion
}