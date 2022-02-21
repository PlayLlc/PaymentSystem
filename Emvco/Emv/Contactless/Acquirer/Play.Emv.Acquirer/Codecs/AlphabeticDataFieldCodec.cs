using Play.Codecs;

namespace Play.Emv.Acquirer.Codecs;

/// <summary>
///     An encoder for encoding and decoding alphabetic ASCII characters
/// </summary>
/// <remarks>
///     Strict parsing is enforced. Exceptions will be raised if invalid data is attempted to be parsed
/// </remarks>
public class AlphabeticDataFieldCodec : AlphabeticEmvCodec, IInterchangeDataFieldCodec
{
    #region Static Metadata

    #region Metadata

    public static readonly PlayEncodingId Identifier = IInterchangeDataFieldCodec.GetEncodingId(typeof(AlphabeticDataFieldCodec));

    #endregion

    #endregion

    #region Instance Members

    public PlayEncodingId GetIdentifier() => Identifier;

    #endregion
}