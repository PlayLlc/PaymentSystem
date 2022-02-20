using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Interchange.Codecs;

public class AlphaNumericSpecialDataFieldCodec : AlphaNumericSpecialEmvCodec, IInterchangeDataFieldCodec
{
    #region Static Metadata

    public static readonly InterchangeEncodingId Identifier =
        IInterchangeDataFieldCodec.GetEncodingId(typeof(AlphaNumericSpecialDataFieldCodec));

    #endregion

    #region Instance Members

    public InterchangeEncodingId GetIdentifier() => Identifier;

    #endregion
}