using Play.Codecs;

namespace Play.Ber.DataObjects;

public interface IRetrieveBerEncodingMetadata
{
    #region Instance Members

    public PlayEncodingId GetEncodingId();

    #endregion
}