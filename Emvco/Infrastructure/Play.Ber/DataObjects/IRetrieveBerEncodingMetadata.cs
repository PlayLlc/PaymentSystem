using Play.Ber.Codecs;

namespace Play.Ber.DataObjects;

public interface IRetrieveBerEncodingMetadata
{
    #region Instance Members

    public BerEncodingId GetBerEncodingId();

    #endregion
}