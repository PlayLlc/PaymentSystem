using Play.Ber.InternalFactories;
using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Interchange.Codecs;

public class VariableInterchangeCodec : VariableEmvCodec, IInterchangeCodec
{
    #region Static Metadata

    public static readonly InterchangeEncodingId Identifier = IInterchangeCodec.GetEncodingId(typeof(VariableInterchangeCodec));

    #endregion

    #region Instance Members

    public InterchangeEncodingId GetIdentifier() => Identifier;

    #endregion
}