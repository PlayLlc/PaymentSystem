using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Interchange.Codecs;

public class AlphaNumericSpecialInterchangeDataFieldCodec : AlphaNumericSpecialEmvCodec, IInterchangeDataFieldCodec
{
    #region Static Metadata

    public static readonly InterchangeEncodingId Identifier =
        IInterchangeDataFieldCodec.GetEncodingId(typeof(AlphaNumericSpecialInterchangeDataFieldCodec));

    #endregion

    #region Instance Members

    public InterchangeEncodingId GetIdentifier() => Identifier;

    #endregion
}