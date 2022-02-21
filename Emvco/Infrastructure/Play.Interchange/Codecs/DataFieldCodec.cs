using Play.Codecs;
using Play.Interchange.DataFields;

namespace Play.Interchange.Messages.DataFields;

internal abstract class DataFieldCodec
{
    #region Instance Members

    public abstract DataFieldId GetDataFieldId();
    public abstract PlayEncodingId GetPlayEncodingId();

    #endregion
}