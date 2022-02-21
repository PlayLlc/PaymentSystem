using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Interchange.Codecs;

public class CompressedNumericDataFieldCodec : CompressedNumericEmvCodec, IInterchangeDataFieldCodec
{
    #region Static Metadata

    public static readonly PlayEncodingId Identifier = IInterchangeDataFieldCodec.GetEncodingId(typeof(CompressedNumericDataFieldCodec));

    #endregion

    #region Instance Members

    public PlayEncodingId GetIdentifier() => Identifier;

    #endregion
}