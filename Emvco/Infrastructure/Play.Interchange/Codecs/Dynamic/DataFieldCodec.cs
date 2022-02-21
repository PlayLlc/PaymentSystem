using Play.Codecs;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic;

internal abstract class DataFieldCodec
{
    #region Instance Members

    public abstract DataFieldId GetDataFieldId();
    public abstract PlayEncodingId GetEncodingId();

    #endregion
}