using Play.Emv.Ber;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;

namespace Play.Emv.Icc;

public abstract class TemplateFactory<T> where T : Template
{
    #region Static Metadata

    protected static readonly EmvCodec _Codec = new();

    #endregion

    #region Instance Members

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public abstract T Create(RApduSignal value);

    #endregion
}