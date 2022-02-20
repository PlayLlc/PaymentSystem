using Play.Codecs;

namespace Play.Interchange.Messages.DataFields;

public abstract class DataFieldMapper
{
    #region Instance Members

    public abstract DataFieldId GetDataFieldId();
    public abstract PlayEncodingId GetPlayEncodingId();

    #endregion
}