using Play.Codecs;

namespace Play.Emv.Acquirer.Codecs;

public class BinaryDataFieldCodec : BinaryEmvCodec, IInterchangeDataFieldCodec
{
    #region Static Metadata

    #region Metadata

    public static readonly PlayEncodingId Identifier = IInterchangeDataFieldCodec.GetEncodingId(typeof(BinaryDataFieldCodec));

    #endregion

    #endregion

    #region Instance Members

    public PlayEncodingId GetIdentifier() => Identifier;

    #endregion
}