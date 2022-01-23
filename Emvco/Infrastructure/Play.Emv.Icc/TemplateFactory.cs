using Play.Ber.Emv;
using Play.Ber.Emv.DataObjects;

namespace Play.Icc.Emv;

public abstract class TemplateFactory<T> where T : Template
{
    #region Static Metadata

    protected static readonly EmvCodec _Codec = new();

    #endregion

    #region Instance Members

    public abstract T Create(RApduSignal value);

    #endregion
}