using Play.Interchange.Codecs;

namespace Play.Interchange.DataFields;

public interface IRetrieveInterchangeFieldMetadata
{
    #region Instance Members

    public DataFieldId GetDataFieldId();
    public ushort GetByteCount();
    public InterchangeEncodingId GetEncodingId();

    #endregion
}