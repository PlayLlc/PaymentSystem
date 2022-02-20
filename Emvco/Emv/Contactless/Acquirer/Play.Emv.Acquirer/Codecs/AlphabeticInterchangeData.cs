using Play.Emv.Codecs;
using Play.Interchange.Codecs;

namespace Play.Emv.Interchange.Codecs;

/// <summary>
///     An encoder for encoding and decoding alphabetic ASCII characters
/// </summary>
/// <remarks>
///     Strict parsing is enforced. Exceptions will be raised if invalid data is attempted to be parsed
/// </remarks>
public class AlphabeticDataFieldCodec : AlphabeticEmvCodec, IInterchangeDataFieldCodec
{
    #region Static Metadata

    public static readonly InterchangeEncodingId Identifier = IInterchangeDataFieldCodec.GetEncodingId(typeof(AlphabeticDataFieldCodec));

    #endregion

    #region Instance Members

    public InterchangeEncodingId GetIdentifier() => Identifier;

    #endregion
}