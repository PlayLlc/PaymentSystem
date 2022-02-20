using Play.Codecs;
using Play.Interchange.DataFields;

namespace Play.Interchange.Messages.DataFields;

public abstract class DataFieldMapper
{
    #region Instance Members

    public abstract DataFieldId GetDataFieldId();
    public abstract PlayEncodingId GetPlayEncodingId();

    #endregion
}