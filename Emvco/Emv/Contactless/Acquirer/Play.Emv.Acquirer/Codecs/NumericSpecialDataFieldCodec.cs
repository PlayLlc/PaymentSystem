using Play.Codecs;
using Play.Emv.Codecs;

namespace Play.Emv.Acquirer.Codecs;

public class NumericSpecialDataFieldCodec : NumericSpecialEmvCodec, IInterchangeDataFieldCodec
{
    #region Static Metadata

    #region Metadata

    public static readonly PlayEncodingId Identifier = IInterchangeDataFieldCodec.GetEncodingId(typeof(NumericSpecialDataFieldCodec));

    #endregion

    #endregion

    #region Instance Members

    public PlayEncodingId GetIdentifier() => Identifier;

    #endregion
}